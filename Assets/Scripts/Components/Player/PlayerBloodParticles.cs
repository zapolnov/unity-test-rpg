
using UnityEngine;

namespace Game
{
    public class PlayerBloodParticles : AbstractBloodParticles
    {
        public override void SpawnBloodParticles(Vector3 position)
        {
            GameController.Instance.particleManager.SpawnBloodParticles(position);
        }
    }
}
