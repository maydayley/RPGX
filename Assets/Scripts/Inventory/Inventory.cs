using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2.Inventory {
    [CreateAssetMenu (menuName = "Single Instances/Inventory")]
    public class Inventory : ScriptableObject {
        public List<Items> items = new List<Items>();
        Dictionary<string, int> dict = new Dictionary<string, int> ();

        public void Initialize () {
            #if UNITY_EDITOR
            items = EditorUtilities.FindAssetsByType<Items>();
            #endif
            for (int i = 0; i < items.Count; i++) {
                if (dict.ContainsKey (items[i].name)) {

                } else {
                    dict.Add (items[i].name, i);
                }
            }
        }

        public Items GetItem (string id) {
            Items ret = null;
            int index = -1;
            if (dict.TryGetValue (id, out index)) {
                ret = items[index];
            }

            if (index == -1)
                Debug.Log ("No item found" + id);
            return ret;
        }
    }
}