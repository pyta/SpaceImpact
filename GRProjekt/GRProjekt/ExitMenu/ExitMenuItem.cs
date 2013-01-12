using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace GRProjekt.ExitMenu
{
    class ExitMenuItem
    {
        #region Members

        private Texture2D itemtexture;
        private Texture2D itemtexture2;
        private Vector2 destinationVector;

        public ButtonState buttonState { get; set; }
        public bool mouseOver { get; set; }

        #endregion

        #region Constructor

        public ExitMenuItem(Vector2 destinationVector)
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

        public void LoadContent(ContentManager content, string path, string path2)
        {
            this.itemtexture = content.Load<Texture2D>(path);
            this.itemtexture2 = content.Load<Texture2D>(path2);
        }

        public void Transform(int i)
        {
            switch (i)
            {
                case 0:
                    this.mouseOver = true;
                    break;
                case 1:
                    this.mouseOver = false;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(mouseOver == false)
                spriteBatch.Draw(itemtexture, this.destinationVector, Color.White);
            else
                spriteBatch.Draw(itemtexture2, this.destinationVector, Color.White);
        }

        #endregion
    }
}
