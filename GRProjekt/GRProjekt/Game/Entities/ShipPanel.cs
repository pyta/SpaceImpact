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
    public class ShipPanel
    {
        #region Members
        public Texture2D SpeedometerBackground  {get; set;}
        public Texture2D SpeedometerPointer     {get; set;}
        #endregion

        #region Constructors
        /// <summary>
        /// Konstruktor. Uzupełnia referencje do obiektów reprezentujących grafiki.
        /// </summary>
        /// <param name="speedometerBackground">Tło prędkościomierza</param>
        /// <param name="speedometerPointer">Wskaźnik prędkościomerza</param>
        public ShipPanel(Texture2D speedometerBackground, Texture2D speedometerPointer)
        { 
            this.SpeedometerBackground = speedometerBackground;
            this.SpeedometerPointer = speedometerPointer;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Metoda rysująca prędkościomierz statku
        /// </summary>
        /// <param name="spriteBatch">Obiekt typu SpriteBatch</param>
        /// <param name="speedometerBounds">Wymiary grafiki</param>
        /// <param name="currentSpeed">Aktualna prędkość statku</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle speedometerBounds, float currentSpeed)
        {
            Rectangle pointerRect = new Rectangle(speedometerBounds.X + 50, speedometerBounds.Y + 50, speedometerBounds.Width, speedometerBounds.Height);

            spriteBatch.Begin();
            spriteBatch.Draw(this.SpeedometerBackground, speedometerBounds, Color.White);
            spriteBatch.Draw(this.SpeedometerPointer, pointerRect, null, Color.White, MathHelper.ToRadians(currentSpeed), new Vector2(50, 50), SpriteEffects.None, 1);
            spriteBatch.End();
        }
        #endregion
    }
}
