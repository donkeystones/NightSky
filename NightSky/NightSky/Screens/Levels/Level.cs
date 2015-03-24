using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NightSky.Screens.Levels;
using NightSky.Content.Mobs;

//TODO: Create a movingobject/physical object for every mob including player for easier management.

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

            //Mob/texture inits
            slimeText = Content.Load<Texture2D>("Mobsheets/SlimeSheet");
            background = Content.Load<Texture2D>("Ambient/nattbakgrund");
            wormText = Content.Load<Texture2D>("Mobsheets/WormSheet");

            spawnBlock = new SpawnBlock();
            //slime variables and stuff 
           

            //spawns player
            player = new Player();
            player.Position = new Vector2(7*32,17*32);
            player.Load(Content);

            //creates a new map
            map = new Map();
            Tiles.Content = Content;
            #region Map
            
            //generates map
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
            base.Update(gameTime);            
            //updates time it takes for mobs too spawn
            spawnTime += (float)gameTime.TotalGameTime.Seconds;
            
            //updates spawnblock were mobs spawn
            spawnPos = spawnBlock.Position;
            spawnBlock.Update(gameTime);
            
            //checks if player is dead
            GameOver();

            //spawns worm
            if (worms.Count < 1) {
                if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime) {
                    worms.Add(new Worm(wormText, spawnPos));
                }
            }

            //spawns slime
            if (slimes.Count < 10) {
                if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime) {
                    slimes.Add(new Slime(slimeText, true, spawnPos));
                    previousSpawnTime = gameTime.TotalGameTime;
                }
            }

            player.Update(gameTime);
          
            //Updates all worms
            foreach (Worm worm in worms)
                worm.Update(gameTime);
            
            //Updates all slimes
            foreach (Slime slime in slimes)
                slime.Update(gameTime);
            
            //Collision update
            foreach (CollisionTiles tile in map.CollisionTiles) {
            
                //Updates the collision bools in slimes
                foreach (Slime slime in slimes)
                    slime.Collision(tile.Rectangle, map.Width, map.Height);
                
                //Updates the collision bools in worms
                foreach (Worm worm in worms)
                    worm.Collision(tile.Rectangle, map.Width, map.Height);
                
                //Updates player collision
                player.Collision(tile.Rectangle, map.Width, map.Height);
            }
            foreach (Slime slime in slimes.ToArray()) {
                    //If player hits top of slime, the slime dies
                    player.Kill(slime.DestRect, slime);
                    if (slime.IsAlive == false)
                        slimes.Remove(slime);
            }

            //If player touches worm

        }

        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            
            //draws background
            spriteBatch.Draw(background, new Rectangle(0, 0, background.Width, background.Height), Color.White);
            
            //draws player
            player.Draw(spriteBatch);
            
            //draws worms
            foreach (Worm worm in worms)
                worm.Draw(spriteBatch);            
            
            //Draws slimes
            foreach(Slime slime in slimes)
                slime.Draw(spriteBatch);
            
            //draws the map
            map.Draw(spriteBatch);
        }

        private void GameOver() {
            //if player bool IsAlive == false send player to game over screen
            if (player.IsAlive == false) {
                ScreenManager.Instance.AddScreen(new GameOver());
            }
        }

    }
}
