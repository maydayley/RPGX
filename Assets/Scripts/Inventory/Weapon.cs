using System.Collections;
using System.Collections.Generic;
using RPG2.ScriptableObjects;
using UnityEngine;

namespace RPG2.Inventory {

    [CreateAssetMenu(menuName = "Items/Weapon")]
    public class Weapon : ScriptableObject {
        public VarString idle_0;
        public VarString idle_1;
        public GameObject modelPrefab;
        public ActionHandler[] actions;
        public LeftHandPos leftHandPos;

        public ActionHandler GetActionHandler(InputType input) {
            for (int i = 0; i < actions.Length; i++) {
                if (actions[i].input == input)
                    return actions[i];
            }
            return null;
        }

        public Action GetAction(InputType input) {
            ActionHandler ah = GetActionHandler(input);
            return ah.action;
        }
    }

    [System.Serializable]
    public class ActionHandler {
        public InputType input;
        public Action action;
    }
}

public enum InputType {
    rb,
    lb,
    lt,
    rt
}
