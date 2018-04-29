
using UnityEngine;

namespace Game
{
    public class SetSkyBox : MonoBehaviour
    {
        public Material material;

        void Start()
        {
            var skybox = GameController.Instance.playerController.camera.GetComponent<Skybox>();
            skybox.material = material;
        }
    }
}
