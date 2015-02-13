using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NightSky {
    public class Animation {

        protected Texture2D image;
        protected Color color;
        protected Rectangle sourceRect;
        float rotation, scale;
        Vector2 origin, position;
        protected ContentManager content;
        protected bool isActive;
        protected float alpha;

        public virtual float Alpha {
            get { return alpha; }
            set { alpha = value; }
        }

        public bool IsActive {

            set { isActive = value; }
            get { return isActive;}
        }

        public float Scale {

            set { scale = value; }

        }

        public virtual void LoadContent(ContentManager content, Texture2D image, Vector2 position) {
            content = new ContentManager(content.ServiceProvider, "Content");
            this.image = image;
            this.position = position;
            if (image != null)
                sourceRect = new Rectangle(0, 0, image.Width, image.Height);
            rotation = 0.0f;
            scale = alpha = 1.0f;

            isActive = false;
        }

        public virtual void UnloadContent() {

            content.Unload();
            position = Vector2.Zero;
            sourceRect = Rectangle.Empty;
            image = null;
        }

        public virtual void Update(GameTime gameTime) {
        
        }

        public virtual void Draw(SpriteBatch spriteBatch) {

            if (image != null) {
                origin = new Vector2(sourceRect.Width / 2,
                                    sourceRect.Height / 2);
                spriteBatch.Draw(image,
                    position + origin,
                    sourceRect,
                    Color.White * alpha,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,0.0f);
            }

        }

    }
}
