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
        private Vector2 arrowPos2;
        private Rectangle arrowRec;
        private Rectangle arrow2Rec;
        private Rectangle arrow3Rec;
        private float speed;
        private int direction;
        private bool isAvailable;
        public Arrow(Vector2 arrowPos, float speed,int direction)
        {
            this.arrowPos = arrowPos;
            arrowPos2 = new Vector2(arrowPos.X, arrowPos.Y + 25);
            arrowRec = new Rectangle((int)arrowPos.X, (int)arrowPos.Y, Game1.arrowImg[0].Width, Game1.arrowImg[0].Height);
            arrow2Rec = new Rectangle(arrowRec.X, (int)arrowPos.Y, arrowRec.Width,
                (int)(Game1.arrowImg[0].Height*0.4));
            
            this.speed = speed;
            this.direction = direction;
            isAvailable = true;
        }

        public void Update(GameTime gameTime)
        {
            arrowPos.Y += speed;
            arrowPos2.Y += speed;
            arrowRec.Y = (int)arrowPos.Y;
            arrow2Rec.Y = (int)arrowPos2.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isAvailable)
            {
                spriteBatch.Draw(Game1.arrowImg[direction], arrowRec, Color.White);
            }        
        }

        public Rectangle GetArrowRect() { return arrowRec; }
        public Rectangle GetArrow2Rect() { return arrow2Rec; }
        public int GetDirection() { return direction; }
        
        public bool GetIsAvailable() { return isAvailable; }

        public void SetIsAvailable(bool isAvailable) { this.isAvailable = isAvailable; }



    }
}
