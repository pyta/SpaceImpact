using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using GRProjekt.Game;

namespace GRProjekt.MainMenu
{
    public enum MenuList
    {
        newGame,
        authors,
        instruction,
        mainMenu,
        exitMenu
    }

    public sealed class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Members

        private Texture2D authorsTexture;
        private Texture2D backgroundTexture;   
        private Texture2D menuBorderTexture;
        private Texture2D titleTexture;
        private List<ManuItem> menuItems;
        private ManuItem backButton;
        private Model shipModel;
        private float modelRotation = 1.0f;
        private static MenuList currentItem;
        private Matrix[] transforms;
        private Microsoft.Xna.Framework.Game game;
        private SpriteBatch spriteBatch;
        private NewGame newGame;

        #endregion

        #region Constructor

        public Menu(Microsoft.Xna.Framework.Game game)
            :base(game)
        {
            this.game = game;
            menuItems = new List<ManuItem>();

            for (int i = 0; i < 4; i++)
            {
                menuItems.Add(new ManuItem(new Vector2(330, 230 + i * 60)));
            }
            backButton = new ManuItem(new Vector2(340,500));
            currentItem = MenuList.mainMenu;

            newGame = new NewGame(game);
            game.Components.Add(this.newGame);
        }

        #endregion

        #region Methods

        public override void Initialize()
        {
            base.Initialize();
        }

        public static MenuList CurrentItem
        {
            get { return currentItem; }
            set { currentItem = value; }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            this.menuBorderTexture = game.Content.Load<Texture2D>("Graphics/MainMenu/menuBorder4");
            this.titleTexture = game.Content.Load<Texture2D>("Graphics/MainMenu/title");
            this.authorsTexture = game.Content.Load<Texture2D>("Graphics/MainMenu/authors");
            
            this.backgroundTexture = game.Content.Load<Texture2D>("Graphics/MainMenu/menuBackground");

            menuItems[0].LoadContent(game.Content, "Graphics/MainMenu/newgameButton", "Graphics/MainMenu/newgameButtonA");
            menuItems[1].LoadContent(game.Content, "Graphics/MainMenu/instructionButton", "Graphics/MainMenu/instructionButtonA");
            menuItems[2].LoadContent(game.Content, "Graphics/MainMenu/authorsButton", "Graphics/MainMenu/authorsButtonA");
            menuItems[3].LoadContent(game.Content, "Graphics/MainMenu/exitButton", "Graphics/MainMenu/exitButton2A");
            backButton.LoadContent(game.Content, "Graphics/ExitMenu/returnButton", "Graphics/MainMenu/returnButton2A");

            shipModel = game.Content.Load<Model>("Models/Ship/guard3");
            transforms = new Matrix[shipModel.Bones.Count];
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            MouseState mouseState= Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed && currentItem == MenuList.mainMenu)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (menuItems[i].GetRectangle.Contains(mouseState.X, mouseState.Y) == true)
                    {
                        switch (i)
                        {
                            case 0: currentItem = MenuList.newGame; break;
                            case 2: currentItem = MenuList.authors; break;
                            case 3: game.Exit(); break;
                        }
                    }
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed && currentItem == MenuList.authors)
            {
                if (backButton.GetRectangle.Contains(mouseState.X, mouseState.Y) == true)
                {
                     currentItem = MenuList.mainMenu;
                }
            }

            foreach (var item in menuItems)
            {
                Rectangle buff = item.GetRectangle;
                if (buff.Contains(mouseState.X, mouseState.Y) == true && item.mouseOver == false)
                {
                    item.Transform(0);
                }
                buff.Y -= 5;
                if (!buff.Contains(mouseState.X, mouseState.Y) == true && item.mouseOver == true)
                {
                    item.Transform(1);
                }
            }

            Rectangle buff2 = backButton.GetRectangle;
            if (buff2.Contains(mouseState.X, mouseState.Y) == true && backButton.mouseOver == false)
            {
                backButton.Transform(0);
            }
            buff2.X -= 5;
            if (!buff2.Contains(mouseState.X, mouseState.Y) == true && backButton.mouseOver == true)
            {
                backButton.Transform(1);
            }

            if (currentItem == MenuList.newGame)
            {
                this.newGame.Playing = true;
            }

            modelRotation += 0.002f;
        }

        float rotation =0;

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (currentItem == MenuList.mainMenu)
            {
                spriteBatch.Begin();

                spriteBatch.Draw(backgroundTexture, this.game.GraphicsDevice.Viewport.Bounds, Color.White);
                spriteBatch.Draw(titleTexture, new Vector2(20, 20), Color.White);
                //spriteBatch.Draw(menuBorderTexture, new Vector2(200, 140), Color.White);

                spriteBatch.Draw(menuBorderTexture, new Vector2(400, 340), null, Color.White, rotation, new Vector2(200,206), 1, SpriteEffects.None, 1);

                foreach (var item in menuItems)
                {
                    item.Draw(gameTime, spriteBatch);
                }

                spriteBatch.End();

                //DepthStencilState depthState = new DepthStencilState();
                //depthState.DepthBufferEnable = true;
                //depthState.DepthBufferWriteEnable = true;
                //GraphicsDevice.DepthStencilState = depthState;

                //shipModel.CopyAbsoluteBoneTransformsTo(transforms);

                //foreach (ModelMesh mesh in shipModel.Meshes)
                //{
                //    foreach (BasicEffect effect in mesh.Effects)
                //    {
                //        effect.EnableDefaultLighting();
                //        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation) * Matrix.CreateScale(4.5f) *Matrix.CreateTranslation(new Vector3(-200, -50, 20));
                //        effect.View = Matrix.CreateLookAt(new Vector3(0.0f, 50.0f, 400.0f), new Vector3(0, 0, 0), Vector3.Up);

                //        effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90.0f),
                //            this.game.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
                //    }
                //    mesh.Draw();
                //}

                rotation+= 0.003f;
                if(rotation == 360)rotation= 0;
            }
            else if (currentItem == MenuList.authors)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(backgroundTexture, this.game.GraphicsDevice.Viewport.Bounds, Color.White);
                
                spriteBatch.Draw(titleTexture, new Vector2(20, 20), Color.White);
                spriteBatch.Draw(authorsTexture, new Vector2(170, 180), Color.White);
                backButton.Draw(gameTime, spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        #endregion
    }
}
