using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NightSky.Screens.Levels;
using NightSky.Content.Mobs;
using NightSky.Entitys.Items;
using Microsoft.Xna.Framework.Input;

//TODO: Create a movingobject/physical object for every mob including player for easier management.

namespace NightSky.Screens {
    public class Level1 : GameScreen{
        private Map map;
        private Player player;
        private Texture2D background;
        private Texture2D wormText,slimeText;
        private Texture2D swordText;
        private Texture2D explosionText;

        private Explosion explosion;
        //Sword
        private Sword sword;

        //Random number for the enemy spawn function
        private Random rand;
        private int random;

        private float spawnTime;

        private TimeSpan enemySpawnTime;
        private TimeSpan previousSpawnTime;
        
        //Mob lists to keep track of any mob in the game!
        private List<Slime> slimes = new List<Slime>();
        private List<Worm> worms = new List<Worm>();
        private List<Explosion> explosions = new List<Explosion>();

        //List to keep track of all items on the map
        private List<Items> Items = new List<Items>();

        public override void LoadContent(ContentManager Content) {
            base.LoadContent(Content);
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(5.0f);
            rand = new Random();
            //Mob/texture inits
            slimeText = Content.Load<Texture2D>("Mobsheets/SlimeSheet");
            background = Content.Load<Texture2D>("Ambient/nattbakgrund");
            wormText = Content.Load<Texture2D>("Mobsheets/WormSheet");
            swordText = Content.Load<Texture2D>("Items/Swords/SwordTest");
            explosionText = Content.Load<Texture2D>("Ambient/ExplosionSpriteSheet");
            
            
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

            
            foreach (Items item in Items) {
                item.Update();
            }

            foreach (Explosion ex in explosions) {
                ex.Update(gameTime);
            }
            random = rand.Next(0,800);
            //MAKE IT RAIN!
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                //Creates a sword with InventoryType = weapon
                sword = new Sword(swordText, new Vector2(random, 10));
                Items.Add(sword);
                explosion = new Explosion(explosionText, new Vector2(random, 10));
                explosions.Add(explosion);
                                
                //Creates a limit so there can not be more than 100 swords
                if (Items.Count() > 100) {
                    Items.Remove(sword);
                }
            }
            base.Update(gameTime);            
            //updates time it takes for mobs too spawn
            spawnTime += (float)gameTime.TotalGameTime.Seconds;
            
            //checks if player is dead
            GameOver();

            //spawns worm
            if (worms.Count < 1) {
                if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime) {
                 // worms.Add(new Worm(wormText, new Vector2(random, 0)));
                }
            }

            //spawns slime
            if (slimes.Count < 10) {
                if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime) {
                    //slimes.Add(new Slime(slimeText, true, new Vector2(random, 0)));
                    previousSpawnTime = gameTime.TotalGameTime;
                }
            }
            //Updates player
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

                foreach (Items item in Items) 
                    item.Collision(tile.Rectangle, map.Width, map.Height);
                
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
            foreach (Items item in Items) { 
                item.Draw(spriteBatch);
            }

            foreach (Explosion ex in explosions) {
                ex.Draw(spriteBatch);
            }

        }

        private void GameOver() {
            //if player bool IsAlive == false send player to game over screen
            if (player.IsAlive == false) {
                ScreenManager.Instance.AddScreen(new GameOver());
            }
        }

    }
}
