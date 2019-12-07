using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2.Manager {
    [CreateAssetMenu(menuName = "Single Instances/Resources Manager")]
    public class ResourcesManager : ScriptableObject {
        public Inventory.Inventory inventory;
        
        public void Init(){
            inventory.Initialize();
        }

        public Inventory.Items GetItem(string id) {
            return inventory.GetItem(id);
        }

        public Inventory.Weapon GetWeapon(string id) {
            Inventory.Items item = GetItem(id);
            return (Inventory.Weapon) item.obj;
        }

        public Inventory.Armory GetArmory(string id) {
            Inventory.Items item = GetItem(id);
            return (Inventory.Armory) item.obj;
        }
    }

}
