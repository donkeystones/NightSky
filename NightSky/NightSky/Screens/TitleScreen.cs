using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NightSky.Screens;


namespace NightSky {
    public class TitleScreen : GameScreen {
        #region menuItems
        //MenuItems
        //Options object handles the particle density variable

        private Game1 game; //this is just for closing the game
        private KeyboardState keyState;
        private ParticleGenerator rain;
        private Texture2D StartBar, OptionsBar, ExitBar, background, logo;
        private int selected, delay;
        private double lastChange;

        #endregion

        public override void LoadContent(ContentManager Content) {
            base.LoadContent(Content);
            //creates new particle generator(in this case rain)
            rain = new ParticleGenerator(Content.Load<Texture2D>("Ambient/Rain"), 800, 50);
            
            //creates a black background
            background = Content.Load<Texture2D>("Ambient/nattbakgrund");
            logo = Content.Load<Texture2D>("MenuItems/logo");
            game = new Game1();

            //Menu select variables
            lastChange = 0;
            selected = 0;
            delay = 200;

            //Menu bars
            StartBar = Content.Load<Texture2D>("MenuItems/Start");
            OptionsBar = Content.Load<Texture2D>("MenuItems/Options");
            ExitBar = Content.Load<Texture2D>("MenuItems/Exit");
        }

        public override void UnloadContent() {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            //Update the rain
            rain.Update(gameTime);
            keyState = Keyboard.GetState();
           
            //move up the selected area
            if (keyState.IsKeyDown(Keys.Up)) {
                if (gameTime.TotalGameTime.TotalMilliseconds > lastChange + delay) {
                    selected--;
                    lastChange = gameTime.TotalGameTime.TotalMilliseconds;
                }
                if (selected <= -1)
                    selected = 2;
            }
            if (keyState.IsKeyDown(Keys.Down)) {
                if (gameTime.TotalGameTime.TotalMilliseconds > lastChange + delay) {
                    selected++;
                    lastChange = gameTime.TotalGameTime.TotalMilliseconds;
                }

                if (selected > 1)
                    selected = 0;                    
            }
            //If start is selected and the user press enter = start the game!
            if (keyState.IsKeyDown(Keys.Enter) && selected == 0) {
                ScreenManager.Instance.AddScreen(new Level1());
            }
            //If options is selected and the user presses enter = load the options screen!
            if (keyState.IsKeyDown(Keys.Enter) && selected == 1) {
                game.ExitGame();
            }
            //If exit is selected and the user presses enter = Exit the game
            
            //updates rain position
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            //draws rain to screen
            rain.Draw(spriteBatch);
            // if selected is equal to 0 then make the start bar yellow
            if (selected == 0) {
                spriteBatch.Draw(StartBar, new Rectangle(50, 52, StartBar.Width, StartBar.Height), Color.Yellow);
            } else {
                spriteBatch.Draw(StartBar, new Rectangle(50, 50, StartBar.Width, StartBar.Height), Color.White);
            }
            // if selected is equal to 1 then make the options bar yellow
            
            // if selected is equal to 2 then make the exit bar yellow
            if (selected == 1) {
                spriteBatch.Draw(ExitBar, new Rectangle(50, 152, ExitBar.Width, ExitBar.Height), Color.Yellow);
            } else {
                spriteBatch.Draw(ExitBar, new Rectangle(50, 150, ExitBar.Width, ExitBar.Height), Color.White);
            }
            spriteBatch.Draw(logo, new Rectangle(790 - logo.Width, 590 - logo.Height, logo.Width, logo.Height), Color.White);
        }

    }
}
