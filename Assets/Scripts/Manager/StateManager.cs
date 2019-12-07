using System.Collections;
using System.Collections.Generic;
using RPG2.ScriptableObjects;
using UnityEngine;
using RPG2.Manager;

namespace RPG2 {
    public class StateManager : MonoBehaviour {
        #region VARIABLES
        public PlayerStats playerStats;
        public States state;
        public MoveInput input;
        public GameObject currentModel;
        public InventoryManager inventoryManager;
        public WeaponManager weaponManager;
        ResourcesManager resourcesManager;
        #region REFERENCES
        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public Rigidbody rbody;
        [HideInInspector]
        public AnimatorHook animHook;
        [HideInInspector]
        public Collider controllerCollider;
        #endregion
        [HideInInspector]
        public LayerMask ignoreLayer;
        [HideInInspector]
        public LayerMask ignoreForGroundCheck;
        public float delta;
        public Transform mTransform;
        public CharacterState characterState;
        public enum CharacterState {
            move,
            onAir,
            armsInteracting,
            overrideLayerInteracting
        }
        #endregion
        #region  INITIALIZE
        public void Initialize() {
            mTransform = this.transform;
            SetupAnimator();
            rbody = GetComponent<Rigidbody>();
            rbody.angularDrag = 999;
            rbody.drag = 9;
            rbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            gameObject.layer = 8;
            ignoreLayer = ~(1 << 9);
            ignoreForGroundCheck = ~(1 << 9 | 1 << 10);
            animHook = currentModel.AddComponent<AnimatorHook>();
            animHook.Initialize(this);
        }

        void SetupAnimator() {
            if (currentModel == null) {
                animator = GetComponentInChildren<Animator>();
                currentModel = animator.gameObject;
            }
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
            animator.applyRootMotion = false;
            animator.GetBoneTransform(HumanBodyBones.LeftHand).localScale = Vector3.one;
            animator.GetBoneTransform(HumanBodyBones.RightHand).localScale = Vector3.one;
        }
        #endregion
        #region  FIXED_UPDATE
        public void Fixed_Tick(float d) {
            delta = d;
            state.onGround = OnGround();
            switch (characterState) {
                case CharacterState.move:
                    HandleRotation();
                    HandleMovement();
                    break;
                case CharacterState.onAir:
                    break;
                case CharacterState.armsInteracting:
                    break;
                case CharacterState.overrideLayerInteracting:
                    rbody.drag = 0;
                    Vector3 velo = rbody.velocity;
                    Vector3 tvelo = input.animDelta;
                    tvelo *= 55f;
                    tvelo.y = velo.y;
                    rbody.velocity = tvelo;
                    break;
                default:
                    break;
            }
        }

        void HandleRotation() {
            Vector3 targetDir = (state.isLockedOn == false) ?
                input.moveDir :
                (input.lockOnTransform == null) ?
                input.lockOnTransform.position - mTransform.position :
                input.moveDir;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = mTransform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(mTransform.rotation, tr, delta * input.mCount * playerStats.rotateSpeed);
            mTransform.rotation = targetRotation;
        }

        void HandleMovement() {
            Vector3 v = mTransform.forward;
            if (input.mCount > 0)
                rbody.drag = 0;
            else
                rbody.drag = 8;
            if (state.isLockedOn)
                v = input.moveDir;
            if (!state.isRunning)
                v *= input.mCount * playerStats.moveSpeed;
            else
                v *= input.mCount * playerStats.runSpeed;
            v.y = rbody.velocity.y;
            rbody.velocity = v;
        }
        #endregion
        #region UPDATE
        public void Tick(float d) {
            delta = d;
            state.onGround = OnGround();
            switch (characterState) {
                case CharacterState.move:
                    bool interact = CheckForInteractionInput();
                    if (!interact)
                        HandleMovementAnimation();
                    break;
                case CharacterState.onAir:
                    break;
                case CharacterState.armsInteracting:

                    break;
                case CharacterState.overrideLayerInteracting:

                    state.animIsInteracting = animator.GetBool("isInteracting");
                    if (state.animIsInteracting == false) {
                        if (state.isInteracting) {
                            state.isInteracting = false;
                            ChangeState(CharacterState.move);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        bool CheckForInteractionInput() {
            Action a = null;
            if (input.rb) {
                a = GetAction(InputType.rb);
                if (a != null) {
                    HandleAction(a);
                    return true;
                }
            }
            if (input.rt) {
                a = GetAction(InputType.rt);
                if (a != null) {
                    HandleAction(a);
                    return true;
                }
            }
            if (input.lb) {
                a = GetAction(InputType.lb);
                if (a != null) {
                    HandleAction(a);
                    return true;
                }
            }
            if (input.lt) {
                a = GetAction(InputType.lt);
                if (a != null) {
                    HandleAction(a);
                    return true;
                }
            }
            return false;
        }

        #region MANAGER FUNCS
        void HandleAction(ScriptableObjects.Action a) {
            switch (a.actionType) {
                case ScriptableObjects.ActionType.attack:
                    AttackAction aa = (AttackAction) a.actionObject;
                    PlayAttackAction(a, aa);
                    break;
                case ScriptableObjects.ActionType.block:
                    break;
                case ScriptableObjects.ActionType.parry:
                    break;
                case ScriptableObjects.ActionType.spell:
                    break;
                default:
                    break;
            }
        }

        ScriptableObjects.Action GetAction(InputType inp) {
            WeaponManager.ActionContainer ac = weaponManager.GetAction(inp);
            if (ac == null)
                return null;
            return ac.action;
        }

        void PlayInteractAnimation(string a) {
            animator.CrossFade(a, 0.2f);
        }

        void PlayAttackAction(Action a, AttackAction aa) {
            animator.SetBool(StaticStrings.mirror, a.isLeft);
            PlayInteractAnimation(aa.animation.strValue);
            if (aa.changeSpeed) {
                animator.SetFloat("speed", aa.animSpeed);
            }
            ChangeState(CharacterState.overrideLayerInteracting);
        }

        void HandleMovementAnimation() {
            if (state.isLockedOn) {

            } else {
                animator.SetBool(StaticStrings.run, state.isRunning);
                animator.SetFloat(StaticStrings.vertical, input.mCount, 0.15f, delta);
            }
        }

        void ChangeState(CharacterState st) {
            characterState = st;
            switch (st) {
                case CharacterState.move:
                    animator.applyRootMotion = false;
                    break;
                case CharacterState.onAir:
                    animator.applyRootMotion = false;
                    break;
                case CharacterState.armsInteracting:
                    animator.applyRootMotion = false;
                    break;
                case CharacterState.overrideLayerInteracting:
                    animator.applyRootMotion = true;
                    animator.SetBool("isInteracting", true);
                    state.isInteracting = true;
                    break;
                default:
                    break;
            }
        }
        #endregion
        bool OnGround() {
            bool ret = false;
            Vector3 origin = mTransform.position;
            origin.y += 0.7f;
            Vector3 dir = -Vector3.up;
            float dis = 1.4f;
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, dis, ignoreForGroundCheck)) {
                ret = true;
                Vector3 targetPosition = hit.point;
                mTransform.position = targetPosition;
            }
            return ret;
        }
    }
    #endregion
    #region CLASSES
    [System.Serializable]
    public class MoveInput {
        public float mCount;
        public float horizontal;
        public float vertical;
        public Vector3 moveDir;
        public Transform lockOnTransform;
        public Vector3 animDelta;
        public bool rt;
        public bool lt;
        public bool rb;
        public bool lb;
    }

    [System.Serializable]
    public class States {
        public bool onGround;
        public bool isRunning;
        public bool isLockedOn;
        public bool isInAction;
        public bool isAbleToMove;
        public bool isDamageOn;
        public bool isRotating;
        public bool isAttackEnabled;
        public bool isSpellcasting;
        public bool isUsingItem;
        public bool isAbledToBeParried;
        public bool isParryOn;
        public bool isLeftHand;
        public bool animIsInteracting;
        public bool isInteracting;
        public bool closeWeapon;
        public bool isInvisible;
    }

    [System.Serializable]
    public class NetworkStates {
        public bool isLocal;
        public bool isInRoom;
    }

    [System.Serializable]
    public class WeaponManager {
        public ActionContainer[] actions;

        public void Initialize() {
            actions = new ActionContainer[4];
            for (int i = 0; i < actions.Length; i++) {
                ActionContainer a = new ActionContainer();
                a.input = (InputType) i;
                actions[i] = a;
            }
        }

        public ActionContainer GetAction(InputType t) {
            for (int i = 0; i < actions.Length; i++) {
                if (actions[i].input == t)
                    return actions[i];
            }
            return null;
        }

        [System.Serializable]
        public class ActionContainer {
            public InputType input;
            public RPG2.ScriptableObjects.Action action;
        }
    }

    [System.Serializable]
    public class InventoryManager {
        public RPG2.Inventory.Items rh_slot;
        public RPG2.Inventory.Items lh_slot;
        public RPG2.Inventory.Items usable;
        public RPG2.Inventory.Items spell;
    }

    #endregion
}
