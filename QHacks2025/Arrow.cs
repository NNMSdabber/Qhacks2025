using GameUtility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QHacks2025
{
    internal class Arrow
    {
        private Vector2 arrowPos;
        private Rectangle arrowRec;
        private float speed;
        private int direction;
        private bool isAvailable;
        public Arrow(Vector2 arrowPos, float speed,int direction)
        {
            this.arrowPos = arrowPos;
            arrowRec = new Rectangle((int)arrowPos.X, (int)arrowPos.Y, Game1.arrowImg[0].Width, Game1.arrowImg[0].Height);
            this.speed = speed;
            this.direction = direction;
            isAvailable = true;
        }

        public void Update(GameTime gameTime)
        {
            arrowPos.Y += speed;
            arrowRec.Y = (int)arrowPos.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isAvailable) { spriteBatch.Draw(Game1.arrowImg[direction], arrowRec, Color.White); }        
        }

        public Rectangle GetArrowRect() { return arrowRec; }
        public int GetDirection() { return direction; }
        
        public bool GetIsAvailable() { return isAvailable; }

        public void SetIsAvailable(bool isAvailable) { this.isAvailable = isAvailable; }



    }
}
