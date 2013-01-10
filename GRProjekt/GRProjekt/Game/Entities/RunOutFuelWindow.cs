using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GRProjekt.Game;

namespace GRProjekt.Game.Entities
{
    public class RunOutFuelWindow
    {
        #region Variables & Encapsulation
        private Texture2D _background;
        public Texture2D Background
        {
            get { return this._background; }
            set { this._background = value; }
        }

        private Rectangle _backgroundBounds;
        public Rectangle BackgroundBounds
        {
            get { return this._backgroundBounds; }
            set { this._backgroundBounds = value; }
        }

        private Texture2D _button;
        public Texture2D Button
        {
            get { return this._button; }
            set { this._button = value; }
        }

        private Rectangle _buttonBounds;
        public Rectangle ButtonBounds
        {
            get { return this._buttonBounds; }
            set { this._buttonBounds = value; }
        }
        #endregion

        #region Constructors
        public RunOutFuelWindow(Texture2D background, Rectangle backgroundBounds, Texture2D button, Rectangle buttonBounds)
        {
            this._background        = background;
            this._backgroundBounds  = backgroundBounds;
            this._button            = button;
            this._buttonBounds      = buttonBounds;
        }
        #endregion

        #region Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(this._background, this._backgroundBounds, Color.White);
            spriteBatch.Draw(this._button, this._buttonBounds, Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Metoda sprawdza czy naciśnięty został przycisk
        /// </summary>
        /// <returns>True jeśli kliknięto na przycisk</returns>
        public bool Update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && this._buttonBounds.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                return true;
            else
                return false;
        }
        #endregion
    }
}
