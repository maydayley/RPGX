using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2.Inventory {
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Items : ScriptableObject {
        // public string id;
        public ItemType type;
        public ItemInfo info;
        public Object obj;

        [System.Serializable]
        public class ItemInfo {
            public string itemName;
            public string itemDescription;
            public string skillDescription;
            public Sprite icon;
        }
    }

    public enum ItemType {
        armory,
        weapon,
        usable,
        spell
    }

}
