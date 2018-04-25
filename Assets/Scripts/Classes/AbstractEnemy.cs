
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        public enum State
        {
            Idle,
            MovingToPosition,
            FollowingObject,
            Dead,
        }

        public EnemyDefinition definition;
        public State state = State.Idle;

        private Vector3 mOriginalPosition;
        private NavMeshAgent mNavMeshAgent;
        private Vector3 mTargetPosition;    // used if state is MovingToPosition
        private GameObject mTargetObject;   // used if state is FollowingObject

        protected virtual void Awake()
        {
            mNavMeshAgent = GetComponent<NavMeshAgent>();
            mOriginalPosition = transform.position;
        }

        protected virtual void Update()
        {
            switch (state) {
                case State.Dead:
                    break;

                case State.Idle:
                    mNavMeshAgent.isStopped = true;
                    break;

                case State.MovingToPosition:
                    if ((mTargetPosition - transform.position).sqrMagnitude < 0.1f)
                        SetIdle();
                    break;

                case State.FollowingObject:
                    mNavMeshAgent.SetDestination(CalcTargetPosition(mTargetObject));
                    break;
            }
        }

        public abstract void onHit();
        public abstract void onDie();

        public void SetIdle()
        {
            if (state == State.Dead)
                return;

            state = State.Idle;
            mNavMeshAgent.isStopped = true;
        }

        public void SetDead()
        {
            state = State.Dead;

            mNavMeshAgent.isStopped = true;
            mNavMeshAgent.enabled = false;

            var collider = GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;
        }

        private void MoveTo(Vector3 destination, float speed, State newState = State.MovingToPosition)
        {
            if (state == State.Dead)
                return;

            state = newState;
            mTargetPosition = destination;
            mNavMeshAgent.speed = speed;
            mNavMeshAgent.SetDestination(destination);
            mNavMeshAgent.isStopped = false;
        }

        private Vector3 CalcTargetPosition(GameObject targetObject)
        {
            NavMeshAgent targetAgent = targetObject.GetComponent<NavMeshAgent>();

            Vector3 pos1 = transform.position;
            Vector3 pos2 = targetObject.transform.position;

            float radius1 = mNavMeshAgent.radius;
            float radius2 = (targetAgent != null ? targetAgent.radius : 0.0f);

            Vector3 direction = (pos2 - pos1).normalized;
            return pos2 - direction * (radius1 + radius2);
        }

        private void MoveTo(GameObject targetObject, float speed)
        {
            mTargetObject = targetObject;
            MoveTo(CalcTargetPosition(mTargetObject), speed, State.FollowingObject);
        }

        public void WalkTo(Vector3 destination) { MoveTo(destination, definition.walkSpeed); }
        public void RunTo(Vector3 destination) { MoveTo(destination, definition.runSpeed); }
        public void WalkTo(GameObject targetObject) { MoveTo(targetObject, definition.walkSpeed); }
        public void RunTo(GameObject targetObject) { MoveTo(targetObject, definition.runSpeed); }

        public void WalkToOriginalPosition() { WalkTo(mOriginalPosition); }
        public void RunToPlayer() { RunTo(GameController.Instance.playerController.gameObject); }

        protected float MovingSpeed()
        {
            if (!gameObject.activeInHierarchy)
                return 0.0f;
            if (mNavMeshAgent.isStopped)
                return 0.0f;

            if (!mNavMeshAgent.pathPending) {
                if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance) {
                    if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude < 0.0001f)
                        return 0.0f;
                }
            }

            return mNavMeshAgent.speed;
        }
    }
}
