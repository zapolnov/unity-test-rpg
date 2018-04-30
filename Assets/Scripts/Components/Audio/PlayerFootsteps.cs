
using UnityEngine;

namespace Game
{
    public class PlayerFootsteps : MonoBehaviour
    {
        [FMODUnity.EventRef] public string soundEvent;
        private PlayerController mPlayerController;

        void Awake()
        {
            mPlayerController = GetComponent<PlayerController>();
        }

        void Start()
        {
            InvokeRepeating("PlayFootstepsSound", 0, 0.4f);
        }

        private void PlayFootstepsSound()
        {
            if (mPlayerController.Speed > 0.0f)
                FMODUnity.RuntimeManager.PlayOneShot(soundEvent);
        }
    }
}
