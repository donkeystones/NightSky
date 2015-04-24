using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using NightSky.Screens.Levels;

namespace NightSky.Entitys.Items {
    //Sword inherets Item class
    class Sword : Items {

        private Vector2 oldpos;
        private double time;
        private bool boobing = false;
        private Vector2 velocity;
        private Rectangle destRect;
        private Vector2 position;
        private Texture2D texture;

        //Constructor
        public Sword(Texture2D texture, Vector2 position):base(texture,position) {
            this.position = position;
            this.texture = texture;
        }

        public override void Update() {

            destRect = new Rectangle((int)position.X,(int)position.Y,texture.Width,texture.Height);
            time += 0.1;
            //gives the sword a speed
            position += velocity;
            if (!boobing)
                oldpos = position;

            if (velocity.Y < 10 && !boobing)
                velocity.Y += 0.4f;
            //make the sword hover up and down
            if (boobing){
                position.Y = (float)Math.Sin(time)*2f + oldpos.Y - 14;
            }
        }

        //Collision method so that the sword doesn't go through the floor of the map
        public override void Collision(Rectangle newRectangle, int xOffset, int yOffset) {
            if (destRect.TouchTopOf(newRectangle) && !boobing) {
                destRect.Y = newRectangle.Y - destRect.Height;
                velocity.Y = 0f;
                boobing = true;
            }
        }    
    

        public override void Draw(SpriteBatch spriteBatch) {
            
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height),Color.White);
        }
    }
}
