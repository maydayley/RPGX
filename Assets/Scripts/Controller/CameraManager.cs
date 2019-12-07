using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG2 {
    public class CameraManager : MonoBehaviour {
        public bool lockOn;
        public float fSpeed = 9f;
        public float mSpeed = 2f;
        public float controllerSpeed = 7f;

        public Transform target;
        public Transform lockOnTransform;
        public Transform mTransform; 
        public Transform pivot;
        public Transform camPos;

        StateManager state;
        float smooth = .1f;
        public float minAng = -35f;
        public float maxAng = 35f;
        public float defZ;
        float curZ;
        public float zSpeed = 19f;
        float smoothX;
        float smoothY;
        float smoothXvelo;
        float smoothYvelo;
        public float lookAng;
        public float tiltAng;
        bool useRightAxis;
        bool changeTargetLeft;
        bool changeTargetRight;
        public void Initialize(StateManager st) {
            state = st;
            target = st.transform;
            curZ = defZ;
            mTransform = this.transform;
        }

        public void Tick(float d) {
            float h = Input.GetAxis(StaticStrings.mouseX);
            float v = Input.GetAxis(StaticStrings.mouseY);
            //float c_h = Input.GetAxis(StaticStrings.rightAxisX);
            //float c_v = Input.GetAxis(StaticStrings.rightAxisY);
            float pSpeed = mSpeed;
            // changeTargetLeft = Input.GetKeyUp(KeyCode.V);
            // changeTargetRight = Input.GetKeyUp(KeyCode.B);

            // if (lockOnTarget != null) {
            //     if (lockOnTransform == null) {
            //         lockOnTransform = lockOnTarget.GetTarget();
            //         state.enemyTransform = lockOnPos;
            //     }
            //     if (Mathf.Abs(c_h) > 0.6f) {
            //         if (!useRightAxis) {
            //             lockOnTransform = lockonTarget.GetTarget((c_h > 0));
            //             state.lockOnTransform = lockOnTransform;
            //             useRightAxis = true;
            //         }
            //     }
            //     if (changeTargetLeft || changeTargetRight) {
            //         lockOnTransform = lockOnTarget.GetTarget(changeTargetLeft);
            //         state.lockOnTransform = lockOnTransform;
            //     }
            // }
            // if (useRightAxis) {
            //     if (Mathf.Abs(c_h) < 0.6f) {
            //         useRightAxis = false;
            //     }
            // }
            // if (c_h != 0 || c_v != 0) {
            //     h = c_h;
            //     v = -c_v;
            //     pSpeed = controllerSpeed;
            // }
            PlayerFollow(d);
            HandlePivotPosition();
            CameraRotation(d, v, h, pSpeed);

        }

        void PlayerFollow(float d) {
            float speed = d * fSpeed;
            Vector3 playerPos = Vector3.Lerp(transform.position, target.position, speed);
            transform.position = playerPos;
        }

        void CameraRotation(float d, float v, float h, float pSpeed) {
            if (smooth > 0) {
                smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelo, smooth);
                smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelo, smooth);
            } else {
                smoothX = h;
                smoothY = v;
            }

            tiltAng -= smoothY * pSpeed;
            tiltAng = Mathf.Clamp(tiltAng, minAng, maxAng);
            pivot.localRotation = Quaternion.Euler(tiltAng, 0, 0);

            // if (lockOn && lockOnTarget != null) {
            //     Vector3 enemyDir = lockOnPos.position - transform.position;
            //     enemyDir.Normalize();
            //     //enemyDir.y = 0;

            //     if (enemyDir == Vector3.zero)
            //         enemyDir = transform.forward;
            //     Quaternion enemyRot = Quaternion.LookRotation(enemyDir);
            //     transform.rotation = Quaternion.Slerp(transform.rotation, enemyRot, d * 9f);
            //     lookAng = transform.eulerAngles.y;
            //     return;
            // }

            lookAng += smoothX * pSpeed;
            transform.rotation = Quaternion.Euler(0, lookAng, 0);
        }

        void HandlePivotPosition() {
            float targetZ = defZ;
            CameraCollision(defZ, ref targetZ);
            curZ = Mathf.Lerp(curZ, targetZ, state.delta * zSpeed);
            Vector3 tp = Vector3.zero;
            tp.z = curZ;
            camPos.localPosition = tp;
        }

        void CameraCollision(float targetZ, ref float actualZ) {
            float step = Mathf.Abs(targetZ);
            int stepCount = 2;
            float stepInc = step / stepCount;

            RaycastHit hit;
            Vector3 origin = pivot.position;
            Vector3 direction = -pivot.forward;

            if (Physics.Raycast(origin, direction, out hit, state.ignoreForGroundCheck)) {
                float distance = Vector3.Distance(hit.point, origin);
                actualZ = -(distance / 2);
            } else {
                for (int s = 0; s < stepCount; s++) {
                    for (int i = 0; i < 4; i++) {
                        Vector3 dir = Vector3.zero;
                        Vector3 secondOrigin = origin + (direction * s) * stepInc;
                        switch (i) {
                            case 0:
                                dir = camPos.right;
                                break;
                            case 1:
                                dir = -camPos.right;
                                break;
                            case 2:
                                dir = camPos.up;
                                break;
                            case 3:
                                dir = -camPos.up;
                                break;
                        }
                        if (Physics.Raycast(secondOrigin, dir, out hit, 0.2f, state.ignoreForGroundCheck)) {
                            float distance = Vector3.Distance(secondOrigin, origin);
                            actualZ = -(distance / 2);
                            if (actualZ < 0.2f)
                                actualZ = 0;

                            return;
                        }
                    }
                }
            }
        }

        public static CameraManager singleton;

        void Awake() {
            singleton = this;
        }
    }
}
