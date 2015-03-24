using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using NightSky.Screens.Levels;

namespace NightSky.Content.Mobs {
    class Slime {

        private Player player;
        private bool isAlive = true;

        public bool IsAlive {
            get { return isAlive; }
            set { isAlive = value; }
        }

        private Rectangle destRect;
        public Rectangle DestRect {
            get { return destRect; }
        }

        private Vector2 position;
        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }

        private Vector2 velocity;
        //slime spritesheet properties
        private Texture2D texture;
        public Rectangle sourceRect;
        private float delay, jDelay = 3f;
        private int frames = 0;

        //wall hit bools
        private bool hitLeft = true, hitRight = false;
        private bool hasJumped;
        //to controll the position of the slime spawn

        public Slime(Texture2D texture, bool isAlive, Vector2 position) {
            this.texture = texture;
            this.isAlive = isAlive;
            this.position = position;
        }

        public void Load(ContentManager Content) {
            player = new Player();
        }

        public void Update(GameTime gameTime) {
            //adds speed to the slime
            position += velocity;
            //sets the slime spritesheet rectangle to control animation
            
            destRect = new Rectangle((int)position.X, (int)position.Y, 20, 20);
            sourceRect = new Rectangle(20 * frames, 0, 20, 20);            
            
            delay += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            //gravitation
            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            if (hasJumped)
                frames = 0;
            else frames = 1;
            //if the slime hits the wall change direction
            if (hitLeft) {
                if (delay > jDelay && hasJumped == false) {
                    velocity.X = +2f;
                    position.Y -= 3f;
                    velocity.Y = -6f;
                    hasJumped = true;
                }
            }
            if (hitRight) {
                if (delay > jDelay && hasJumped == false) {
                    velocity.X = -2f;
                    position.Y -= 4f;
                    velocity.Y = -6f;
                    hasJumped = true;
                }
            }
            if (delay > jDelay + 0.3f)
                delay = 0;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset) {
            //if slime hits top of a rectangle.
            if (destRect.TouchTopOf(newRectangle)) {
                destRect.Y = newRectangle.Y - destRect.Height;
                velocity.Y = 0f;
                velocity.X = 0f;
                hasJumped = false;
            }
            //if the slime touches the left side of the rectangle
            if (destRect.TouchLeftOf(newRectangle)) {
                hitRight = false;
                hitLeft = true;
            }
            //if the slime touches the right side of the rectangle
            if (destRect.TouchRightOf(newRectangle)) {
                hitRight = true;
                hitLeft = false;
            }
            //if the slime touches the bottom of a block
            if (destRect.TouchBottomOf(newRectangle))
                velocity.Y = 1f;
            //if hit left
            if (position.X < 0) {
                if(hitRight)
                    velocity.X = 0;

                hitRight = false;
                hitLeft = true;
            }
            //if hit right
            if (position.X > xOffset - destRect.Width) {
                if(hitLeft)
                    velocity.X = 0;

                hitLeft = false;
                hitRight = true;
            }
        }
    }
}
