
using UnityEngine;

namespace Game
{
    public class RotateTowardsCamera : MonoBehaviour
    {
        void Update()
        {
            var camera = Camera.main;
            if (camera == null)
                return;

            var camTrans = camera.transform;
            transform.LookAt(transform.position + camTrans.rotation * Vector3.forward, camTrans.rotation * Vector3.up);
        }
    }
}
