﻿
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        private Animator mAnimator;
        private Rigidbody mRigidBody;
        private bool mAttacking;

        void Start()
        {
            GameController.Instance.playerController = this;
            mAnimator = GetComponent<Animator>();
            mRigidBody = GetComponent<Rigidbody>();
            GetComponent<NavMeshAgent>().updateRotation = false;
        }

        void FixedUpdate()
        {
            if (Input.GetButton("Fire1")) {
                mAttacking = true;
            } else {
                if (mAttacking) {
                    var currentAnim = mAnimator.GetCurrentAnimatorStateInfo(0);
                    if (currentAnim.IsName("Attack") && currentAnim.normalizedTime >= 1.0f)
                        mAttacking = false;
                }
            }

            // Mouse look
            const float coeff = 5.0f;
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * coeff);

            if (mAttacking) {
                mRigidBody.velocity = new Vector3();
            } else {
                PlayerDefinition playerDefinition = GameController.Instance.playerDefinition;
                float x = Input.GetAxis("Horizontal") * playerDefinition.strafeSpeed;
                float y = Input.GetAxis("Vertical") * playerDefinition.forwardSpeed;
                Vector3 direction = transform.forward * y + transform.right * x;
                mRigidBody.velocity = direction;
            }

            mAnimator.SetFloat("speed", mRigidBody.velocity.magnitude);
            mAnimator.SetBool("attack", mAttacking);
        }

        public bool WeaponColliderIsActive()
        {
            // Collision checks should be activated only when sword is "falling" over the enemy, but not when it is
            // being raised or when player is standing still
            var currentAnim = mAnimator.GetCurrentAnimatorStateInfo(0);
            return (currentAnim.IsName("Attack") && currentAnim.normalizedTime >= 0.1f);
        }
    }
}
