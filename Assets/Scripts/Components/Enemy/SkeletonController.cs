
using UnityEngine;

namespace Game
{
    public class SkeletonController : AbstractEnemy
    {
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

            mAnimator.SetFloat("speedh", MovingSpeed());
        }
    }
}
