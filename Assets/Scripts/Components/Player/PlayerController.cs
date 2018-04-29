
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        public float interactRadius;
        public AbstractInteractable targetObjectForInteraction;

        private Animator mAnimator;
        private Rigidbody mRigidBody;
        private bool mAttacking;
        private Collider[] mColliderBuffer = new Collider[512];

        void Awake()
        {
            mAnimator = GetComponent<Animator>();
            mRigidBody = GetComponent<Rigidbody>();
            GetComponent<NavMeshAgent>().updateRotation = false;
        }

        void FixedUpdate()
        {
            if (Input.GetButtonDown("Fire1")) {
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
                targetObjectForInteraction = null;
            } else {
                // Player movement
                PlayerDefinition playerDefinition = GameController.Instance.playerDefinition;
                float x = Input.GetAxis("Horizontal") * playerDefinition.strafeSpeed;
                float y = Input.GetAxis("Vertical") * playerDefinition.forwardSpeed;
                Vector3 direction = transform.forward * y + transform.right * x;
                mRigidBody.velocity = direction;

                // See if player can interact with something
                int n = Physics.OverlapSphereNonAlloc(transform.position, interactRadius, mColliderBuffer,
                    -1, QueryTriggerInteraction.Ignore);
                AbstractInteractable nearestInteractable = null;
                float nearestDistance = 0.0f;
                for (int i = 0; i < n; i++) {
                    GameObject gameObject = mColliderBuffer[i].gameObject;
                    if (gameObject == this)
                        continue;

                    AbstractInteractable interactable = gameObject.GetComponent<AbstractInteractable>();
                    if (interactable != null && interactable.CanInteract()) {
                        Vector3 dir = gameObject.transform.position - transform.position;

                        float angle = Vector3.Angle(transform.forward, dir);
                        if (Mathf.Abs(angle) > 30.0f)
                            continue;

                        float sqrDistance = dir.sqrMagnitude;
                        if (nearestInteractable == null || sqrDistance < nearestDistance) {
                            nearestInteractable = interactable;
                            nearestDistance = sqrDistance;
                        }
                    }
                }

                targetObjectForInteraction = nearestInteractable;
            }

            // Use
            if (Input.GetButtonDown("Use") && targetObjectForInteraction != null)
                targetObjectForInteraction.Interact();

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
