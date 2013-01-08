using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GRProjekt.Game.Entities;
using Microsoft.Xna.Framework.Input;

namespace GRProjekt.Game
{
    public enum MenuList
    {
        game,
        planetMenu
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

        public ShipPanel Panel { get; set; }

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
            this.Panel = new ShipPanel(
                game.Content.Load<Texture2D>("Graphics/Game/speed"), 
                game.Content.Load<Texture2D>("Graphics/Game/pointer"),
                game.Content.Load<Texture2D>("Graphics/Game/fuel"),
                game.Content.Load<Texture2D>("Graphics/Game/fulePointer"));
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

                this.ship.Update();
                foreach (var planet in this.planets) planet.Update();
                this.time += 0.01f;
                
                // kolizje z planetami 
                for (int i = 0; i < this.planets.Count; ++i)
                {
                    planets[i].UpdatePosition();

                    if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding))
                    {
                        planets[i].Dock();
                    }

                    if (planets[i].IsDocked())
                    {
                        World.Current.shipPosition = planets[i].GetPlanetPositon();
                        World.Current.cameraPosition = planets[i].GetPlanetPositon() - new Vector3(100, -600, 0);
                        currentItem = MenuList.planetMenu;
                        ship.PlanetCollision();
                    }

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
                            }
                        }
                    }
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
                this.Panel.Draw(spriteBatch, new Rectangle(50, 475, 100, 100), ship.Speed, new Rectangle(650, 475, 100, 100), ship.Fuel);
                spriteBatch.End();

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                this.network.Draw(gameTime);
                foreach (var planet in this.planets) planet.Draw(gameTime);
                ship.Draw(gameTime);

                if (currentItem == MenuList.planetMenu)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(planetMenuTexture, new Vector2(200, 110), Color.White);
                    spriteBatch.End();
                }

            }
            base.Draw(gameTime);
        }

        #endregion
    }
}
