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

    public class SplashScreen : GameScreen
    {

        KeyboardState keyState;
        Texture2D image;

        public override void LoadContent(ContentManager Content) {
            base.LoadContent(Content);
            image = Content.Load<Texture2D>("SplashScreen/TwosmomaniaLogo");
        }

        public override void UnloadContent() {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            keyState = Keyboard.GetState();
            if (gameTime.TotalGameTime.Seconds >= 3 || keyState.IsKeyDown(Keys.Enter)) {
                ScreenManager.Instance.AddScreen(new TitleScreen());
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(image, new Rectangle(0, 0, 800, 600), Color.White);
        }

    }  
}
