using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QHacks2025
{
    public class Button
    {
        //Stores the distance between edge of the button and the label
        const float BUTTON_PADDING_HORI = 6.75f;

        //Stores the position of the button and label
        private int xPos;
        private int yPos;
        public Rectangle rec;
        private Vector2 labelPos;

        //Stores the button image and text of the button
        private Texture2D sprite;
        private string label;

        //Pre: The button sprite, the x and y position and the text of the button label
        //Desc: Creates an instance of the button class
        public Button(Texture2D buttonSprite, int xPosition, int yPosition, string text)
        {
            //Stores the x and y position of the button
            xPos = xPosition;
            yPos = yPosition;

            //Stores the texture of the button
            sprite = buttonSprite;

            //Stores the text used for the label
            label = text;

            //Creates the position of the text label and button
            Vector2 labelWidthHeight = Game1.labelFont.MeasureString(label);
            rec = new Rectangle(xPos, yPos, (int)(labelWidthHeight.X + (2 * BUTTON_PADDING_HORI)), (int)labelWidthHeight.Y);
            labelPos = new Vector2(xPos + CenterSpriteFontX(label, Game1.labelFont, rec.Width),
                                   yPos + CenterSpriteFontY(label, Game1.labelFont, rec.Height));

        }

        //Pre: The spritebatch to draw the button and the colour of the button
        //Post: None
        //Desc: Draws the button and the label
        public void DrawButton(SpriteBatch spriteBatch, Color buttonClr)
        {
            //Draws the button and followed by the label
            spriteBatch.Draw(sprite, rec, buttonClr);
            spriteBatch.DrawString(Game1.labelFont, label, labelPos, Color.Red);
        }

        //Pre: The text and SpriteFont of the text to display and the width of the total available space
        //Post: The X position that will center the inputted text
        //Desc: Returns the X position that will center the inputted text based on it's font
        public static float CenterSpriteFontX(string text, SpriteFont font, int enviromentWidth)
        {
            return (enviromentWidth / 2) - (font.MeasureString(text).X / 2);
        }

        //Pre: The text and SpriteFont of the text to display and the height of the total available space
        //Post: The Y position that will center the inputted text
        //Desc: Returns the Y position that will center the inputted text based on it's font
        public static float CenterSpriteFontY(string text, SpriteFont font, int enviromentHeight)
        {
            return (enviromentHeight / 2) - (font.MeasureString(text).Y / 2);
        }
    }
}