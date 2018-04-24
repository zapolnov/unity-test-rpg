
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public float strafeSpeed;
        public float forwardSpeed;
        private bool attacking;

        private Animator mAnimator;
        private Rigidbody mRigidBody;

        void Start()
        {
            mAnimator = GetComponent<Animator>();
            mRigidBody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (Input.GetButton("Fire1")) {
                attacking = true;
            } else {
                if (attacking) {
                    var currentAnim = mAnimator.GetCurrentAnimatorStateInfo(0);
                    if (currentAnim.IsName("Attack") && currentAnim.normalizedTime >= 1.0f)
                        attacking = false;
                }
            }

            if (attacking) {
                mRigidBody.velocity = new Vector3();
            } else {
                float x = Input.GetAxis("Horizontal") * strafeSpeed;
                float y = Input.GetAxis("Vertical") * forwardSpeed;
                Vector3 direction = transform.forward * y + transform.right * x;
                mRigidBody.velocity = direction;
            }

            mAnimator.SetFloat("speed", mRigidBody.velocity.magnitude);
            mAnimator.SetBool("attack", attacking);
        }
    }
}
