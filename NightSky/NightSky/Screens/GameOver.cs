using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NightSky.Screens.Levels {
    class GameOver : GameScreen{

        Texture2D background;
        Texture2D gameOver;
        Texture2D retry;
        Texture2D yn;       

        public override void LoadContent(ContentManager Content) {
            base.LoadContent(Content);
            background = Content.Load<Texture2D>("Ambient/nattbakgrund");
            gameOver = Content.Load<Texture2D>("Ambient/game over");
            retry = Content.Load<Texture2D>("Ambient/retry");
            yn = Content.Load<Texture2D>("Ambient/yes no");
        }

        public override void UnloadContent() {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            //if player presses Y, restart level
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                ScreenManager.Instance.AddScreen(new Level1());
            //if player presses N, goto title screen(aka Main menu)
            if (Keyboard.GetState().IsKeyDown(Keys.N))
                ScreenManager.Instance.AddScreen(new TitleScreen()); 
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            spriteBatch.Draw(background,new Rectangle(0,0,background.Width,background.Height),Color.White);
            spriteBatch.Draw(gameOver, new Rectangle(400 - gameOver.Width / 2, 100, gameOver.Width, gameOver.Height), Color.White);
            spriteBatch.Draw(retry, new Rectangle(155,300,retry.Width,retry.Height), Color.White);
            spriteBatch.Draw(yn,new Rectangle(440,305,yn.Width+50,yn.Height+50),Color.White);
        }

    }
}
