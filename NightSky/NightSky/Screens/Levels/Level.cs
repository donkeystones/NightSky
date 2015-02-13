using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NightSky.Screens.Levels;
using NightSky.Content.Mobs;

namespace NightSky.Screens {
    public class Level1 : GameScreen{
        private Map map;
        private Player player;
        private Texture2D background;
        private Texture2D wormText,slimeText;
        private SpawnBlock spawnBlock;
        private Vector2 spawnPos;

        private float spawnTime;

        private TimeSpan enemySpawnTime;
        private TimeSpan previousSpawnTime;
        //Mob lists to keep track of any mob in the game!
        private List<Slime> slimes = new List<Slime>();
        private List<Worm> worms = new List<Worm>();

        public override void LoadContent(ContentManager Content) {
            base.LoadContent(Content);
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(5.0f);

            slimeText = Content.Load<Texture2D>("Mobs/SlimeSheet");
            background = Content.Load<Texture2D>("Ambient/nattbakgrund");
            wormText = Content.Load<Texture2D>("Mobs/WormSheet");

            spawnBlock = new SpawnBlock();
            //slime variables and stuff 
           
            player = new Player();
            player.Position = new Vector2(7*32,17*32);
            player.Load(Content);

            map = new Map();
            Tiles.Content = Content;
            #region Map
            map.Generate(new int[,]{
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            },32);
            #endregion
        }

        public override void UnloadContent() {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime) {
            spawnTime += (float)gameTime.TotalGameTime.Seconds;
            spawnPos = spawnBlock.Position;
            spawnBlock.Update(gameTime);
            GameOver();

            base.Update(gameTime);
            if (worms.Count < 1) {
                if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime) {
                    worms.Add(new Worm(wormText, spawnPos));

                }
            }
            if (slimes.Count < 2) {
                if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime) {
                    slimes.Add(new Slime(slimeText, true, spawnPos));
                    previousSpawnTime = gameTime.TotalGameTime;
                }
            }

            player.Update(gameTime);
            foreach (Worm worm in worms)
                worm.Update(gameTime);
            foreach (Slime slime in slimes)
                slime.Update(gameTime);
            foreach (CollisionTiles tile in map.CollisionTiles) {
                foreach (Slime slime in slimes)
                    slime.Collision(tile.Rectangle, map.Width, map.Height);
                foreach (Worm worm in worms)
                    worm.Collision(tile.Rectangle, map.Width, map.Height);
                player.Collision(tile.Rectangle, map.Width, map.Height);
            }
            int i = 0;
            foreach (Slime slime in slimes.ToArray()) {
                    player.Kill(slime.DestRect, slime);
                    if (slime.IsAlive == false)
                        slimes.RemoveAt(i);
                    i++;
            }
        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            player.Draw(spriteBatch);
            foreach (Worm worm in worms)
                worm.Draw(spriteBatch);            
            foreach(Slime slime in slimes)
                slime.Draw(spriteBatch);
            
            map.Draw(spriteBatch);
        }

        private void GameOver() {
            if (player.IsAlive == false) {
                ScreenManager.Instance.AddScreen(new GameOver());
            }
        }

    }
}
