
using UnityEngine;

namespace Game
{
    public class SpiderController : AbstractEnemy
    {
        public GameObject indicators;
        public float approachDistance;
        public float attackDistance;

        private bool mRunningToPlayer;
        private bool mAttacking;
        private bool mSeenPlayer;

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
                LookAt(player.transform.position);
            }  else {
                if (sqrDistanceToPlayer <= approachDistance * approachDistance) {
                    if (!mSeenPlayer) {
                        mSeenPlayer = true;
                        FMODUnity.RuntimeManager.PlayOneShot(runToPlayerSound, transform.position);
                    }

                    if (!mRunningToPlayer) {
                        mRunningToPlayer = true;
                        RunToPlayer();
                    }
                } else {
                    mSeenPlayer = false;
                    if (mRunningToPlayer) {
                        mRunningToPlayer = false;
                        WalkToOriginalPosition();
                    }
                }
            }

            mAnimator.SetFloat("speed", MovingSpeed());
            mAnimator.SetBool("attack", mAttacking);
        }

        public override void OnHit()
        {
            if (state == State.Dead)
                return;

            mAnimator.ResetTrigger("hit");
            mAnimator.SetTrigger("hit");
        }

        public override void OnDie()
        {
            SetDead();
            indicators.SetActive(false);
            mAnimator.ResetTrigger("hit");
            mAnimator.SetTrigger("dead");
        }

        public override bool WeaponColliderIsActive()
        {
            // Collision checks should be activated only when sword is "falling" over the player, but not when it is
            // being raised or when spider is standing still
            var currentAnim = mAnimator.GetCurrentAnimatorStateInfo(0);
            return (currentAnim.IsName("Attack") && currentAnim.normalizedTime >= 0.1f);
        }
    }
}
