
using UnityEngine;

namespace Game
{
    public class SkeletonController : AbstractEnemy
    {
        public GameObject indicators;
        public float attackDistance;

        private bool mRunningToPlayer;

        private Animator mAnimator;

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
            if ((player.transform.position - transform.position).sqrMagnitude <= attackDistance * attackDistance) {
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

            mAnimator.SetFloat("speed", MovingSpeed() * 0.5f);
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
