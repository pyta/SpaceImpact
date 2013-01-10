using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GRProjekt.Game.Entities;
using GRProjekt.Exceptions;
using Microsoft.Xna.Framework.Input;

namespace GRProjekt.Game
{
    public enum MenuList
    {
        game,
        planetMenu,
        runOutFuel
    }

    public class NewGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Members

        private Texture2D planetMenuTexture;
        private MenuList currentItem;
        private Microsoft.Xna.Framework.Game game;
        private bool playing;
        private Texture2D gameTexture;
        private SpriteBatch spriteBatch;
        private Ship.Ship ship;
        private Planets planets;
        private World world;
        private Network network;
        private float time;

        /// <summary>
        /// Informacja o tym czy statek jest zadokowany przy jakiejś planecie
        /// </summary>
        private bool _isDocked = false;
        public bool IsDocked
        {
            get { return this._isDocked; }
            set { this._isDocked = value; }
        }

        public ShipPanel Panel { get; set; }
        public RunOutFuelWindow RunOutFuelWindow { get; set; }
        public Stars Stars { get; set; }

        #endregion

        #region Constructor

        public NewGame(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            this.game = game;
            this.playing = false;
            this.ship = new Ship.Ship();
            this.planets = new Planets();

            this.world = new World(this.game.GraphicsDevice.Viewport.AspectRatio);
            this.network = new Network();
            this.time = 0.0f;

            this.currentItem = MenuList.game;
        }

        #endregion

        #region Propeteries

        public Boolean Playing
        {
            get { return this.playing; }
            set { this.playing = value; }
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            base.Initialize();
        }
            
        protected override void LoadContent()
        {
            base.LoadContent();
            this.gameTexture = game.Content.Load<Texture2D>("Graphics/Game/space");
            this.planetMenuTexture = game.Content.Load<Texture2D>("Graphics/MainMenu/authorBackground");
            this.ship.LoadContent(game.Content);
            // Panel (liczniki + inne pierdoły)
            this.Panel = new ShipPanel(
                game.Content.Load<Texture2D>("Graphics/Game/speed"), 
                game.Content.Load<Texture2D>("Graphics/Game/pointer"),
                game.Content.Load<Texture2D>("Graphics/Game/fuel"),
                game.Content.Load<Texture2D>("Graphics/Game/fulePointer"));
            // Okienko informujące o braku tanku
            this.RunOutFuelWindow = new RunOutFuelWindow(
                 game.Content.Load<Texture2D>("Graphics/Game/runOutFuel"),
                 new Rectangle((this.GraphicsDevice.Viewport.Width - 486) / 2, (this.GraphicsDevice.Viewport.Height - 358) / 2, 486, 358),
                 game.Content.Load<Texture2D>("Graphics/Game/okBut"),
                 new Rectangle(((this.GraphicsDevice.Viewport.Width - 486) / 2) + 275, ((this.GraphicsDevice.Viewport.Height - 358) / 2) + 250, 150, 75));
            // Gwiazdki
            this.Stars = new Stars(
                game.Content.Load<Texture2D>("Graphics/Game/star"),
                this.GraphicsDevice.Viewport.Width,
                this.GraphicsDevice.Viewport.Height);

            this.network.LoadContent(game.Content);

            foreach(var planet in this.planets)  planet.LoadContent(game.Content);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.playing == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.N) && this.time >0.06f)
                {
                    this.network.NetworkVisible = !this.network.NetworkVisible;
                }
                if (this.time > 0.06f) time = 0;

                #region Zachowania statku
                try
                {
                    this.ship.Update();
                }
                catch (RunOutFuelException)
                {
                    this.currentItem = MenuList.runOutFuel;
                }
                catch (Exception)
                { 
                    // BUG
                }
                #endregion

                foreach (var planet in this.planets) planet.Update();
                this.time += 0.01f;

                this.Stars.Update(this.ship.Speed);
                
                // kolizje z planetami 
                for (int i = 0; i < this.planets.Count; ++i)
                {
                    planets[i].UpdatePosition();

                    if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding))
                    {
                        planets[i].Dock();
                        this._isDocked = true;
                    }

                    if (planets[i].IsDocked())
                    {
                        World.Current.shipPosition = planets[i].GetPlanetPositon();
                        World.Current.cameraPosition = planets[i].GetPlanetPositon() - new Vector3(100, -600, 0);
                        currentItem = MenuList.planetMenu;
                        ship.PlanetCollision();
                    }

                    #region Stary kod
                    //if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding) && planets[i].GetPlanetType == PlanetType.Star)
                    //{
                    //    World.Current.shipPosition = planets[i].GetPlanetPositon();
                    //    ship.PlanetCollision();                        
                    //}
                    //else if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding) && planets[i].GetPlanetType == PlanetType.RingPlanet)
                    //{
                    //    World.Current.shipPosition = planets[i].GetPlanetPositon();
                    //    ship.PlanetCollision();
                    //}
                    //else if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding) && planets[i].GetPlanetType == PlanetType.GasPlanet)
                    //{
                    //    World.Current.shipPosition = planets[i].GetPlanetPositon();
                    //    ship.PlanetCollision();
                    //}
                    //else if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding) && planets[i].GetPlanetType == PlanetType.LavaPlanet)
                    //{
                    //    World.Current.shipPosition = planets[i].GetPlanetPositon();
                    //    ship.PlanetCollision();
                    //}
                    //else if (planets[i].ObjectSphereBounding.Intersects(this.ship.ObjectSphereBounding))
                    //{
                    //    World.Current.shipPosition = planets[i].GetPlanetPositon();
                    //    ship.PlanetCollision();
                    //}
                    #endregion

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        for (int j = 0; j < this.planets.Count; ++j)
                        {
                            if (planets[i].IsDocked())
                            {
                                planets[j].UnDock();
                                //World.Current.shipPosition += new Vector3(100, 500, 1100);
                                currentItem = MenuList.game;
                                World.Current.shipPosition += new Vector3(0,700,0);
                                ship.ShipStart();
                                this._isDocked = false;
                            }
                        }
                    }
                }

                switch (this.currentItem)
                { 
                    case MenuList.runOutFuel:
                        if (this.RunOutFuelWindow.Update())
                        { 
                            // naciśnięto na przycisk
                            this._isDocked = true;
                            planets[1].Dock();
                        }
                    break;
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.playing == true)
            {
                //GraphicsDevice.Clear(Color.CornflowerBlue);

                spriteBatch.Begin();
                spriteBatch.Draw(gameTexture, this.game.GraphicsDevice.Viewport.Bounds, Color.White);
                spriteBatch.End();

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                this.network.Draw(gameTime);
                foreach (var planet in this.planets) planet.Draw(gameTime);
                ship.Draw(gameTime);

                switch (this.currentItem)
                { 
                    case MenuList.planetMenu:
                        spriteBatch.Begin();
                        spriteBatch.Draw(planetMenuTexture, new Vector2(200, 110), Color.White);
                        spriteBatch.End();
                    break;
                    case MenuList.runOutFuel:
                        this.RunOutFuelWindow.Draw(spriteBatch);
                    break;
                }

                if (!this._isDocked)
                {
                    // Rysowanie panelu
                    spriteBatch.Begin();
                    this.Panel.Draw(spriteBatch, new Rectangle(50, 475, 100, 100), ship.Speed, new Rectangle(650, 475, 100, 100), ship.Fuel);
                    this.Stars.Draw(spriteBatch);
                    spriteBatch.End();
                }

            }
            base.Draw(gameTime);
        }

        #endregion
    }
}
