
using UnityEngine;

namespace Game
{
    public abstract class AbstractWeapon : MonoBehaviour
    {
        private bool mDidCollide;
        private bool mTriggerEntered;

        public abstract bool ColliderIsActive();
        public abstract float Damage();

        private void OnTriggerEnter(Collider other)
        {
            mTriggerEntered = true;
            CheckCollision(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            CheckCollision(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            mTriggerEntered = false;
        }

        private void Update()
        {
            if (mDidCollide && !mTriggerEntered && !ColliderIsActive())
                mDidCollide = false;
        }

        private void CheckCollision(GameObject victim)
        {
            if (mDidCollide)
                return;
            if (!ColliderIsActive())
                return;

            var health = victim.GetComponent<AbstractHealthComponent>();
            if (health == null)
                return;

            // Ensure that we are not colliding with ourselves
            Transform t = victim.transform;
            do {
                if (t.gameObject == gameObject)
                    return;
                t = t.parent;
            } while (t != null);

            mDidCollide = true;
            float damage = Damage();

            var armor = victim.GetComponent<AbstractArmor>();
            if (armor != null) {
                damage -= armor.CurrentArmor();
                if (damage <= 0.0f)
                    return;
            }

            health.ApplyDamage(damage);

            AbstractEnemy enemy = victim.GetComponent<AbstractEnemy>();
            if (enemy != null) {
                if (health.CurrentHealth() <= 0.0f)
                    enemy.onDie();
                else
                    enemy.onHit();
            }

            Collider collider = gameObject.GetComponent<Collider>();
            Vector3 position = collider.ClosestPointOnBounds(transform.position);
            GameController.Instance.particleManager.SpawnBloodParticles(position);
        }
    }
}
