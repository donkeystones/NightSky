using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NightSky.Entitys.Items {
    abstract class Items {
        //Items coordinates
        private Vector2 position;


        public Items(Texture texture,Vector2 position) {
            this.position = position;
        }

        public abstract void Update();
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Collision(Rectangle newRectangle, int xOffset, int yOffset);
    }
}
