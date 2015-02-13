using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace NightSky.Screens.Levels {
    class SpawnBlock {

        private Texture2D texture;
        private Rectangle rectangle;
        private Vector2 velocity,position = new Vector2(0,0);
        private bool hitLeft = true, hitRight = false;
        public Vector2 Position {
            get { return position; }
        }
        public SpawnBlock() {
        }

        public void LoadContent(ContentManager Content) {
            texture = Content.Load<Texture2D>("Mobs/WormSheet");
        }

        public void Update(GameTime gameTime) {
            position += velocity;

            rectangle = new Rectangle((int)position.X, (int)position.Y, 10, 10);
            if (position.X < 0) {
                hitRight = false;
                hitLeft = true; 
        }
            if (position.X > 800 - rectangle.Width) {
                hitLeft = false;
                hitRight = true;
            }

            if (hitLeft)
                velocity.X = +1f;
            if (hitRight)
                velocity.X = -1f;

            Console.WriteLine(position.X + " :X");
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture,rectangle,Color.White);
        }
    }
}
