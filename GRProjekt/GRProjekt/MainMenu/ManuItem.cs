using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GRProjekt.MainMenu
{
    public class ManuItem
    {
        #region Members

        private Texture2D itemtexture;
        private Vector2 destinationVector;

        public ButtonState buttonState{get;set;}
        public bool mouseOver { get; set; }

        #endregion

        #region Constructors

        public ManuItem(Vector2 destinationVector)
        {
            this.destinationVector = destinationVector;
            this.buttonState = ButtonState.Released;
            this.mouseOver = false;
        }

        #endregion

        #region propeteries

        public Rectangle GetRectangle
        {
            get 
            {
                return new Rectangle((int)destinationVector.X, (int)destinationVector.Y, itemtexture.Width, itemtexture.Height);
            }
        }

        #endregion

        #region Methods

        public void LoadContent(ContentManager content, string path)
        {
            this.itemtexture = content.Load<Texture2D>(path);
        }

        public void Transform(int i)
        {
            switch (i)
            {
                case 0:
                    destinationVector.Y += 5;
                    this.mouseOver = true;
                    break;
                case 1:
                    destinationVector.Y -= 5;
                    this.mouseOver = false;
                    break;
                case 3:
                    destinationVector.X+= 5;
                    this.mouseOver = true;
                    break;
                case 4:
                    destinationVector.X-= 5;
                    this.mouseOver = false;
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(itemtexture, this.destinationVector, Color.White);
        }

        #endregion
    }
}
