using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2.Inventory {
    [CreateAssetMenu (menuName = "Items/Armory")]
    public class Armory : ScriptableObject {
        public ArmorType armorType;
        public Mesh armorMesh;
        public Material[] materials;
        public bool baseBodyEnabled;
    }

}

public enum ArmorType {
    chest,
    legs,
    hands,
    head
}