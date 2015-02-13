using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using NightSky.Content.Mobs;

namespace NightSky.Screens.Levels {
    class Player {        
        private Texture2D texture;
        private Rectangle sourceRect;
        private Rectangle destRect;
        private float elapsed;
        private float delay;
        private int frames;
        private int dir;
        /*
          dir = 1 = left
          dir = 2 = right
        */

        private int points;
        public int Points {
            get { return points; }
            set { points = value; }
        }

        private Vector2 position;
        private Vector2 velocity;

        private bool hasJumped = true,isAlive;

        public bool IsAlive {
            get { return isAlive; }
        }

        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }

        public Player() { }

        public void Load(ContentManager Content) {
            texture = Content.Load<Texture2D>("Mobs/PlayerAnimSheet");
            delay = 200f;
            frames = 0;
            dir = 2;
            isAlive = true;
        }

        public void Update(GameTime gameTime) {
            position += velocity;
            Input(gameTime);
            if (velocity.Y < 10)
                velocity.Y += 0.4f;
        }

        private void Input(GameTime gameTime) {

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //player going right
            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) {
                dir = 2;
                //Right animation
                destRect = new Rectangle((int)position.X, (int)position.Y, 32, 32);
                sourceRect = new Rectangle(32 * frames, 32 * 0, 32, 32);
                if (elapsed >= delay) {
                    if (frames > 1)
                        frames = 0;
                    else frames++;
                    elapsed = 0;
                }
                velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;

            }
                //Player going left
            else if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) {
                dir = 1;
                //Left animation
                destRect = new Rectangle((int)position.X, (int)position.Y, 32, 32);
                sourceRect = new Rectangle(32 * frames, 32 * 1, 32, 32);
                if (elapsed >= delay) {
                    if (frames >= 1)
                        frames = 0;
                    else frames++;
                    elapsed = 0;
                }
                velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 3;
            } else {
                //checks if player is turned right or left!
                if (dir == 2) {
                    destRect = new Rectangle((int)position.X, (int)position.Y, 32, 32);
                    sourceRect = new Rectangle(32 * 2, 0, 32, 32);
                } else if (dir == 1) {
                    destRect = new Rectangle((int)position.X, (int)position.Y, 32, 32);
                    sourceRect = new Rectangle(32 * 2, 32 * 1, 32, 32);
                }

                velocity.X = 0f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !hasJumped) {
                position.Y -= 5f;
                velocity.Y = -9f;
                hasJumped = true;
            }
            
        }
        public void Kill(Rectangle newRectangle, Slime slime) {
            //If player hits the top of the slime = Slime.IsAlive = false
            if (destRect.TouchTopOf(newRectangle)) {
                destRect.Y = newRectangle.Y - destRect.Height;
                position.Y -= 5f;
                velocity.Y = -9f;
                hasJumped = true;
                slime.IsAlive = false;
                points++;
            } 
            //If player hits the sides/bottom of the slime = IsAlive = false
            if (destRect.TouchBottomOf(newRectangle) ||
                  destRect.TouchLeftOf(newRectangle) ||
                  destRect.TouchRightOf(newRectangle)) {
                      isAlive = false;
            }

        }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset) {
            if (destRect.TouchTopOf(newRectangle)) {
                destRect.Y = newRectangle.Y - destRect.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }
            if (destRect.TouchLeftOf(newRectangle)) {
                position.X = newRectangle.X - destRect.Width - 2;
            }
            if (destRect.TouchRightOf(newRectangle)) {
                position.X = newRectangle.X + newRectangle.Width + 2;
            }
            if (destRect.TouchBottomOf(newRectangle)) {
                velocity.Y = 1f;
            }

            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - destRect.Width) position.X = xOffset - destRect.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > yOffset - destRect.Height) position.Y = yOffset - destRect.Height;

        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, destRect,sourceRect, Color.White);
        }

    }
}
