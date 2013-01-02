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
    public class NewGame : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Members

        private Microsoft.Xna.Framework.Game game;
        private bool playing;
        private Texture2D gameTexture;
        private SpriteBatch spriteBatch;
        private Ship.Ship ship;
        private Planets planets;
        private World world;
        private Network network;
        private float time;

        #endregion

        #region Constructor

        public NewGame(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            this.game = game;
            this.playing = false;
            this.ship = new Ship.Ship(300, 100);
            this.planets = new Planets();

            this.world = new World(this.game.GraphicsDevice.Viewport.AspectRatio);
            this.network = new Network();
            this.time = 0.0f;
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
            this.ship.LoadContent(game.Content);
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
                    if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding) && planets[i].GetPlanetType == PlanetType.Star)
                    {

                    }
                    else if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding) && planets[i].GetPlanetType == PlanetType.RingPlanet)
                    {
                    }
                    else if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding) && planets[i].GetPlanetType == PlanetType.GasPlanet)
                    {
                    }
                    else if (this.ship.ObjectSphereBounding.Intersects(planets[i].ObjectSphereBounding) && planets[i].GetPlanetType == PlanetType.LavaPlanet)
                    {
                    }
                    else if (planets[i].ObjectSphereBounding.Intersects(this.ship.ObjectSphereBounding))
                    {
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
                spriteBatch.End();

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                this.network.Draw(gameTime);
                foreach (var planet in this.planets) planet.Draw(gameTime);
                ship.Draw(gameTime);

                Rectangle speedometer = new Rectangle(20, 450, 100, 100);
                ship.DrawPanel(spriteBatch, speedometer);
            }
            base.Draw(gameTime);
        }

        #endregion
    }
}
