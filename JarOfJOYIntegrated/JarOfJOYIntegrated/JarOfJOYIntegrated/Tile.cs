using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace JarOfJOYIntegrated
{
    public class Tile
    {
        // word on tile
        public String Word;

        // Set the coordinates to draw the sprite at.
        //Vector2 spritePosition = Vector2.Zero;
        //Vector2 spritePosition = new Vector2(0.0f, 100.0f);
        public Vector2 spritePosition;
        public Vector2 fontPosition;

        public Boolean isClicked;

        public int spriteRow;
        public int spritePage; 

        public Tile()
        {
            Word = null;

        }

        public Tile(String W)
        {
            this.Word = W;
        }



    }
}
