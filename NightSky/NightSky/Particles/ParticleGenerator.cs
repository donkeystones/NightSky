using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NightSky.Screens {
    public class ParticleGenerator {

        Texture2D texture;

        float spawnWidth;
        float density;

        List<Rain> rain = new List<Rain>();

        float timer;

        Random rand1, rand2;
        public ParticleGenerator(Texture2D texture, float spawnWidth, float density){
            this.texture = texture;
            this.spawnWidth = spawnWidth;
            this.density = density;
            rand1 = new Random();
            rand2 = new Random();
        }

        public void createParticle() {

            //double anything = rand1.Next();
            rain.Add(new Rain(texture,
                new Vector2(-50 + (float)rand1.NextDouble() * spawnWidth, 0),
                new Vector2(1, rand1.Next(6,8))));
        }

        public void Update(GameTime gameTime) {

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (timer > 0) {
                timer -= 1f / density;
                createParticle();
            }

            for (int i = 0; i < rain.Count; i++) {
                //updates all the rain drops
                rain[i].Update();

                if (rain[i].Position.Y > 800) {
                    //if rain gets outside the screen then delete it
                    rain.RemoveAt(i);
                    i--;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Rain r in rain) {
                r.Draw(spriteBatch);
            }
        }

    }
}
