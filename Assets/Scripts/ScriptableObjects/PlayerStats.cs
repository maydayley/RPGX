using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2 {
    [CreateAssetMenu(menuName = "Single Instances/Player Stats")]
    public class PlayerStats : ScriptableObject {
        public float moveSpeed;
        public float runSpeed;
        public float rotateSpeed;
    }
}
