
using UnityEngine;

namespace Game
{
    public class PlayerWeapon : AbstractWeapon
    {
        public PlayerController playerController;

        public override bool ColliderIsActive()
        {
            return playerController.WeaponColliderIsActive();
        }

        public override float Damage()
        {
            GameController controller = GameController.Instance;
            return controller.playerDefinition.CurrentDamage(controller.playerState.level);
        }
    }
}
