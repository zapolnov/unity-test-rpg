
using UnityEngine;

namespace Game
{
    public class PlayerArmor : AbstractArmor
    {
        public override float CurrentArmor()
        {
            GameController controller = GameController.Instance;
            return controller.playerDefinition.CurrentArmor(controller.playerState.level);
        }
    }
}
