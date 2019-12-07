using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2.Inventory {
    [CreateAssetMenu(menuName =  "Weapons/Left Hand Position")]
    public class LeftHandPos : ScriptableObject {
        public Vector3 pos;
        public Vector3 eulers;

    }
}
