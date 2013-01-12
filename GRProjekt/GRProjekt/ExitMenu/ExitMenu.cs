using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GRProjekt.ExitMenu
{
    public class ExitMenu
    {
        #region Members

        private List<ExitMenuItem> menuItems;
        private Texture2D menuBorderTexture;
        private SpriteBatch spriteBatch;
        private bool menuEnable;
        private Microsoft.Xna.Framework.Game game;

        #endregion

        #region Constructor

        public  ExitMenu()
        {
            menuItems = new List<ExitMenuItem>();

            for (int i = 0; i < 3; i++)
            {
                menuItems.Add(new ExitMenuItem(new Vector2(330, 180 + i * 60)));
            }

            this.menuEnable = false;
        }

        #endregion

        #region Propeteries

        public bool MenuEnable
        {
            get { return this.menuEnable; }
            set { this.menuEnable = value; }
        }

        #endregion

        #region Methods

        public  void LoadContent(Microsoft.Xna.Framework.Game game)
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            this.menuBorderTexture = game.Content.Load<Texture2D>("Graphics/ExitMenu/menuBorder3");

            menuItems[0].LoadContent(game.Content, "Graphics/ExitMenu/mainMenuButton", "Graphics/ExitMenu/mainMenuButtonA");
            menuItems[1].LoadContent(game.Content, "Graphics/ExitMenu/exitButton", "Graphics/ExitMenu/exitButtonA");
            menuItems[2].LoadContent(game.Content, "Graphics/ExitMenu/returnButton", "Graphics/ExitMenu/returnButtonA");

            this.game = game;
        }

        float rotation = 0;
        public bool Update()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (menuItems[i].GetRectangle.Contains(mouseState.X, mouseState.Y) == true)
                    {
                        switch (i)
                        {
                            case 0: return true;
                            case 1: game.Exit(); break;
                            case 2: this.menuEnable = false; break;
                        }
                    }
                }
            }

            foreach (var item in menuItems)
            {
                Rectangle buff = item.GetRectangle;
                if (buff.Contains(mouseState.X, mouseState.Y) == true && item.mouseOver == false)
                {
                    item.Transform(0);
                }
                if (!buff.Contains(mouseState.X, mouseState.Y) == true && item.mouseOver == true)
                {
                    item.Transform(1);
                }
            }

            rotation += 0.001f;
            if (rotation == 360) rotation = 0;

            return false;
        }

        public void Draw()
        {
            spriteBatch.Begin();

            //spriteBatch.Draw(menuBorderTexture, new Vector2(210, 50), Color.White);

            spriteBatch.Draw(menuBorderTexture, new Vector2(400, 270), null, Color.White, rotation, new Vector2(200, 206), 1, SpriteEffects.None, 1);

            foreach (var item in menuItems)
            {
                item.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
        #endregion
    }
}
