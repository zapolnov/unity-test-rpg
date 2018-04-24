
using UnityEngine;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        private Animator mAnimator;
        private Rigidbody mRigidBody;
        private bool mAttacking;

        void Start()
        {
            mAnimator = GetComponent<Animator>();
            mRigidBody = GetComponent<Rigidbody>();
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
    }
}
