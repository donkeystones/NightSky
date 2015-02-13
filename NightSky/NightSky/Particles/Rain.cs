﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NightSky.Screens {
    class Rain {
        
        Texture2D texture;
        Vector2 position;
        Vector2 velocity;

        public Vector2 Position {
            get { return position; }
        }

        public Rain(Texture2D texture, Vector2 position, Vector2 velocity) {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
        }

        public void Update() {
            position += velocity;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }
}
