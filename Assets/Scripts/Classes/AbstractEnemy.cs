
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections.Generic;

namespace Game
{
    [RequireComponent(typeof(UniqueId))]
    public abstract class AbstractEnemy : MonoBehaviour
    {
        public enum State
        {
            Idle,
            MovingToPosition,
            MovingToOriginalPosition,
            FollowingObject,
            Dead,
        }

        [Serializable]
        public struct SavedState
        {
            public Vector3 position;
            public Quaternion rotation;
            public bool isDead;
        }

        public EnemyDefinition definition;
        public State state = State.Idle;
        public string UniqueId { get { return mUniqueId.guid; } }

        protected Animator mAnimator;
        protected UniqueId mUniqueId;

        private Vector3 mOriginalPosition;
        private Quaternion mOriginalRotation;
        private NavMeshAgent mNavMeshAgent;
        private Vector3 mTargetPosition;    // used if state is MovingToPosition
        private GameObject mTargetObject;   // used if state is FollowingObject

        protected virtual void Awake()
        {
            mUniqueId = GetComponent<UniqueId>();
            mAnimator = GetComponent<Animator>();
            mNavMeshAgent = GetComponent<NavMeshAgent>();
            mOriginalPosition = transform.position;
            mOriginalRotation = transform.rotation;
        }

        protected virtual void Start()
        {
            Dictionary<string, AbstractEnemy.SavedState> enemies = GameController.Instance.enemies;
            if (enemies.ContainsKey(mUniqueId.guid)) {
                SavedState state = enemies[mUniqueId.guid];
                if (state.isDead) {
                    transform.position = state.position;
                    transform.rotation = state.rotation;
                    OnDie();
                }
            }
        }

        protected virtual void Update()
        {
            switch (state) {
                case State.Dead:
                    return;

                case State.Idle:
                    mNavMeshAgent.isStopped = true;
                    break;

                case State.MovingToPosition:
                    if ((mTargetPosition - transform.position).sqrMagnitude < 0.1f || MovingSpeed() < 0.000001f)
                        SetIdle();
                    break;

                case State.MovingToOriginalPosition:
                    if ((mOriginalPosition - transform.position).sqrMagnitude < 0.1f || MovingSpeed() < 0.000001f) {
                        mNavMeshAgent.updateRotation = false;
                        transform.rotation = Quaternion.Slerp(transform.rotation, mOriginalRotation, 0.1f);
                        if (Mathf.Abs(Quaternion.Angle(transform.rotation, mOriginalRotation)) < 0.1f) {
                            mNavMeshAgent.updateRotation = true;
                            SetIdle();
                        }
                    }
                    break;

                case State.FollowingObject:
                    mNavMeshAgent.SetDestination(CalcTargetPosition(mTargetObject));
                    if (MovingSpeed() > 0.0f)
                        mNavMeshAgent.updateRotation = true;
                    else {
                        mNavMeshAgent.updateRotation = false;
                        LookAt(mNavMeshAgent.destination);
                    }
                    break;
            }

            UpdateEnemyRegistry();
        }

        public abstract void OnHit();
        public abstract void OnDie();
        public abstract bool WeaponColliderIsActive();

        private void UpdateEnemyRegistry()
        {
            SavedState s = new SavedState();
            s.position = transform.position;
            s.rotation = transform.rotation;
            s.isDead = (state == State.Dead);
            GameController.Instance.enemies[mUniqueId.guid] = s;
        }

        protected void LookAt(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            if (direction.sqrMagnitude > 0.0f) {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
            }
        }

        public void SetIdle()
        {
            if (state == State.Dead)
                return;

            state = State.Idle;
            mNavMeshAgent.isStopped = true;
        }

        public bool IsDead()
        {
            return state == State.Dead;
        }

        public void SetDead()
        {
            state = State.Dead;

            mNavMeshAgent.isStopped = true;
            mNavMeshAgent.enabled = false;

            var collider = GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;

            UpdateEnemyRegistry();
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
            mNavMeshAgent.updateRotation = true;
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

        public void RunToPlayer() { RunTo(GameController.Instance.playerController.gameObject); }

        public void WalkToOriginalPosition()
        {
            MoveTo(mOriginalPosition, definition.walkSpeed, State.MovingToOriginalPosition);
        }

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
