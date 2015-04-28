using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NightSky.Entitys.Items {
    class Explosion {
        
        private Vector2 position;
        private Texture2D texture;
        float timer = 0f;
        private float interval = 100f;
        public int frame = 0;

        private int SIZE = 32;

        private Rectangle sourceRect;
        private Rectangle destRect;
        public Explosion(Texture2D texture, Vector2 position) {
            this.texture = texture;
            this.position = position;
        }

        public void Update(GameTime gameTime) {
            sourceRect = new Rectangle(frame * SIZE, 0, SIZE, SIZE);
            destRect = new Rectangle((int)position.X, (int)position.Y, SIZE, SIZE);
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval) {
                frame++;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }

    }
}
