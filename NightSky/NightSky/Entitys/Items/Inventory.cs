using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NightSky.Entitys.Items {
    class Inventory {

        List<Items> items = new List<Items>();
        private int selected = 0;
        private int x, y;

        public Inventory() { 
            
        }

        public void Update() {

            foreach (Items i in items) { 
                
            }

        }

        public void Draw() {

        }

    }
}
