
using UnityEngine;

namespace Game
{
    public class SkeletonBloodParticles : AbstractBloodParticles
    {
        public override void SpawnBloodParticles(Vector3 position)
        {
            GameController.Instance.particleManager.SpawnSkeletonParticles(position);
        }
    }
}
