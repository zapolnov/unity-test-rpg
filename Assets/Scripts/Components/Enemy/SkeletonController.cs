
using UnityEngine;

namespace Game
{
    public class SkeletonController : AbstractEnemy
    {
        public GameObject indicators;
        public float approachDistance;
        public float attackDistance;

        private Animator mAnimator;
        private bool mRunningToPlayer;
        private bool mAttacking;

        void Start()
        {
            mAnimator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            base.Update();

            if (state == State.Dead)
                return;

            var player = GameController.Instance.playerController;
            float sqrDistanceToPlayer = (player.transform.position - transform.position).sqrMagnitude;

            if (mAttacking) {
                var currentAnim = mAnimator.GetCurrentAnimatorStateInfo(0);
                if (!currentAnim.IsName("Attack") || currentAnim.normalizedTime >= 1.0f)
                    mAttacking = false;
            } else {
                if (sqrDistanceToPlayer <= attackDistance * attackDistance)
                    mAttacking = true;
            }

            if (mAttacking) {
                mRunningToPlayer = false;
                SetIdle();
            }  else {
                if (sqrDistanceToPlayer <= approachDistance * approachDistance) {
                    if (!mRunningToPlayer) {
                        mRunningToPlayer = true;
                        RunToPlayer();
                    }
                } else {
                    if (mRunningToPlayer) {
                        mRunningToPlayer = false;
                        WalkToOriginalPosition();
                    }
                }
            }

            mAnimator.SetFloat("speed", MovingSpeed() * 0.5f);
            mAnimator.SetBool("attack", mAttacking);
        }

        public override void onHit()
        {
            if (state == State.Dead)
                return;

            mAnimator.ResetTrigger("hit");
            mAnimator.SetTrigger("hit");
        }

        public override void onDie()
        {
            SetDead();
            indicators.SetActive(false);
            mAnimator.ResetTrigger("hit");
            mAnimator.SetTrigger("dead");
        }
    }
}
