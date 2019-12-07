using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2 {
    public class AnimatorHook : MonoBehaviour {
        Animator animator;
        StateManager state;

        public void Initialize(StateManager st) {
            state = st;
            animator = state.animator;
        }

        private void OnAnimatorMove() {
            state.input.animDelta = animator.deltaPosition;
            transform.localPosition = Vector3.zero;
        }
    }
}
