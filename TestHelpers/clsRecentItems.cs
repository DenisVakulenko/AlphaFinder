using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestHelpers {
    public class clsRecentItems {
        string Path;
        string[] Items;
        int LastItemIndex;
        int MaxNumber;

        public clsRecentItems(string newPath, int newMaxNumber) {
            Path = newPath;
            MaxNumber = newMaxNumber;

            Items = new string[MaxNumber];
            LastItemIndex = 0;
            
            if (System.IO.File.Exists(Path)) {
                string[] File = System.IO.File.ReadAllLines(Path);
                for (int i = 0; i < MaxNumber; i++) {
                    if (i < File.Length) {
                        Items[i] = File[i];
                    }
                    else {
                        Items[i] = "";
                    }
                }
            }
            else {
                System.IO.File.Create(Path);
                
                for (int i = 0; i < MaxNumber; i++) {
                    Items[i] = "";
                }
            }
        }

        public void AddItem(string Item) {
            if (--LastItemIndex < 0) LastItemIndex = MaxNumber - 1;
            
            Items[LastItemIndex] = Item;
        }

        public string[] GetSomeItems(int Number) {
            if (Number > MaxNumber) Number = MaxNumber;
            string[] List = new string[Number];

            for (int n = 0; n < Number; n++) {
                List[n] = Items[(n + LastItemIndex) % MaxNumber];
            }
            return List;
        }

        public string[] GetAllItems() {
            string[] List = new string[MaxNumber+1];

            for (int n = 0; n < MaxNumber; n++) {
                List[n+1] = Items[(n + LastItemIndex) % MaxNumber];
                if (List[n + 1] == "") break;
            }

            return List;
        }

        public void Save() {
            string[] List = new string[MaxNumber];

            for (int n = 0; n < MaxNumber; n++) {
                List[n] = Items[(n + LastItemIndex) % MaxNumber];
            }
            System.IO.File.WriteAllLines(Path, List);
        }
    }
}
