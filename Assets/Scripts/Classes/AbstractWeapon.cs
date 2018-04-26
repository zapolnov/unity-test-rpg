
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public abstract class AbstractWeapon : MonoBehaviour
    {
        private readonly HashSet<GameObject> mObjectsHit = new HashSet<GameObject>();

        public abstract bool ColliderIsActive();
        public abstract float Damage();

        private void OnTriggerEnter(Collider other)
        {
            CheckCollision(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            CheckCollision(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
        }

        private void Update()
        {
            if (mObjectsHit.Count > 0 && !ColliderIsActive())
                mObjectsHit.Clear();
        }

        private void CheckCollision(GameObject victim)
        {
            if (!ColliderIsActive())
                return;
            if (mObjectsHit.Contains(victim))
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

            mObjectsHit.Add(victim);
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
                    enemy.OnDie();
                else
                    enemy.OnHit();
            }

            AbstractBloodParticles bloodParticles = victim.GetComponent<AbstractBloodParticles>();
            if (bloodParticles != null) {
                Collider collider = victim.GetComponent<Collider>();
                bloodParticles.SpawnBloodParticles(collider.ClosestPointOnBounds(transform.position));
            }
        }
    }
}
