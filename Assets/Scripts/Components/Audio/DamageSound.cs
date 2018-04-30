
using UnityEngine;

namespace Game
{
    public class DamageSound : MonoBehaviour
    {
        [FMODUnity.EventRef] public string damageSound;
        [FMODUnity.EventRef] public string deathSound;

        public void PlayDamageSound(Vector3 position)
        {
            FMODUnity.RuntimeManager.PlayOneShot(damageSound, position);
        }

        public void PlayDeathSound(Vector3 position)
        {
            FMODUnity.RuntimeManager.PlayOneShot(deathSound, position);
        }
    }
}
