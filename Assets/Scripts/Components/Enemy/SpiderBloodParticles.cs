
using UnityEngine;

namespace Game
{
    public class SpiderBloodParticles : AbstractBloodParticles
    {
        public override void SpawnBloodParticles(Vector3 position)
        {
            GameController.Instance.particleManager.SpawnGreenBloodParticles(position);
        }
    }
}
