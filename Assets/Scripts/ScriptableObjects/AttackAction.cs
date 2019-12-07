using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2.ScriptableObjects {
    [CreateAssetMenu (menuName = "Action/Attack")]
    public class AttackAction : ScriptableObject {
        public VarString animation;
        public bool canBeParried = true;
        public bool changeSpeed = false;
        public float animSpeed = 1f;
        public bool canParry = false;
        public bool canBackstab = false;
    }
}