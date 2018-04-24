
using UnityEngine;

namespace Game
{
    public class PlayerHealth : AbstractHealthComponent
    {
        public override float CurrentHealth()
        {
            return GameController.Instance.playerState.health;
        }

        public override float MaxHealth()
        {
            GameController controller = GameController.Instance;
            return controller.playerDefinition.MaxHealth(controller.playerState.level);
        }

        public override void ApplyDamage(float damage)
        {
            GameController.Instance.playerState.health -= damage;
        }
    }
}
