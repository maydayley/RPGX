using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2.ScriptableObjects {
    [System.Serializable]
    public class Action {
        public bool isLeft;
        
        public ActionType actionType;
        public Object actionObject;
    }
    
    public enum ActionType {
        attack,
        block,
        spell,
        parry
    }
}