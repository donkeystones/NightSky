using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NightSky.Screens.Levels {
    class Map {
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        
        //List of collisiontiles
        public List<CollisionTiles> CollisionTiles {
            get { return collisionTiles; }
        }

        private int width, height;
        public int Width {
            get { return width; }
        }

        public int Height {
            get { return height; }
        }

        public Map() {
            
        }

        //Generates map
        public void Generate(int[,] map, int size) {
            for (int x = 0; x < map.GetLength(1); x++) {
                for (int y = 0; y < map.GetLength(0); y++) {
                    int number = map[y, x];
                    //if the number in array is greater than 0 add it as collisionblock
                    if(number > 0)
                        collisionTiles.Add(new CollisionTiles(number, new Rectangle(x*size,y*size,size,size)));

                    width = (x + 1) * size;
                    height = (y + 1) * size;
                }
            }
        }

        //draws the map
        public void Draw(SpriteBatch spriteBatch) {
            foreach (CollisionTiles tile in collisionTiles)
                tile.Draw(spriteBatch);
        }
    }
}
