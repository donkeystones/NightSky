using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NightSky.Screens.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NightSky.Content.Mobs {
    class Worm {

        
        private Vector2 position;
        private Vector2 velocity;

        //spritesheet animation variables and rectangles
        private Texture2D texture;
        private Rectangle sourceRect;
        private Rectangle destRect;
        private float elapsed;
        private float delay = 400f;
        int frames = 0;

        bool hitLeft = false, hitRight = true;
        
        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }

        public Worm(Texture2D texture,Vector2 position) {
            this.texture = texture;
            this.position = position;
        }

        public void Load(ContentManager Content) {
            
        }

        public void Update(GameTime gameTime) {
            position += velocity;

            //this rectangle is the source rectangle
            destRect = new Rectangle((int)position.X, (int)position.Y, 21, 10);
            //this rectangle makes up the animation with help of the frames int!
            sourceRect = new Rectangle(21 * frames, 0, 21, 10);            

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay) {
                if (frames >= 1)
                    frames = 0;
                else frames++;
                elapsed = 0;
            }

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            if (hitLeft) {
                velocity.X = +1f;
            }
            if (hitRight) {
                velocity.X = -1f;
            }

        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset) {
            if (destRect.TouchTopOf(newRectangle)) {
                destRect.Y = newRectangle.Y - destRect.Height;
                velocity.Y = 0f;
            }

            if (destRect.TouchLeftOf(newRectangle)) {
                hitLeft = true;
                hitRight = false;
            }
            if (destRect.TouchRightOf(newRectangle)) {
                hitRight = true;
                hitLeft = false;
            }
            if (destRect.TouchBottomOf(newRectangle))
                velocity.Y = 4f;

            if (position.X < 0) {
                hitRight = false;
                hitLeft = true; 
            }
            if (position.X > xOffset - destRect.Width) {
                hitLeft = false;
                hitRight = true;
            }
        }    
    
    }
}