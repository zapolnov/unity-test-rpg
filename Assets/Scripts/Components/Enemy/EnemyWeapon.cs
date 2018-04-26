
using UnityEngine;

namespace Game
{
    public class EnemyWeapon : AbstractWeapon
    {
        public AbstractEnemy enemy;

        public override bool ColliderIsActive()
        {
            return enemy.WeaponColliderIsActive();
        }

        public override float Damage()
        {
            return enemy.definition.damage;
        }
    }
}
