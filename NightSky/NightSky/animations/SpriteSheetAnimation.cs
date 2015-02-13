using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NightSky.animations {
    class SpriteSheetAnimation : Animation{

        int frameCounter;
        int switchFrame;

        Vector2 frames;

        public Vector2 Frames {
            set { frames = value; }
        }

        public override void LoadContent(ContentManager Content,Texture2D image,Vector2 position){
            base.LoadContent(Content,image,position);
            frameCounter = 0;
            switchFrame = 100;
            frames = new Vector2(3, 4);
        }

        public override void UnloadContent() {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            if (isActive) {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                //If frame counter is higher or equal to switchFrame, set frameCounter to 0
                if (frameCounter >= switchFrame)
                    frameCounter = 0;
            }else {
                frameCounter = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
        }


    }
}
