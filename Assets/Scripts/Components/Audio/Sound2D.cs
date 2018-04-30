
using UnityEngine;

namespace Game
{
    public class Sound2D : MonoBehaviour
    {
        [FMODUnity.EventRef] public string soundEvent;

        public void PlaySound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(soundEvent);
        }
    }
}
