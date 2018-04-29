
using UnityEngine;

namespace Game
{
    public abstract class AbstractInteractable : MonoBehaviour
    {
        public GameObject highlightTarget;
        public Color highlightColor = Color.green;

        private Renderer mRenderer;
        private Material[] mOriginalMaterials;
        private Material[] mPatchedMaterials;

        void Awake()
        {
            if (highlightTarget == null)
                highlightTarget = gameObject;

            mRenderer = highlightTarget.GetComponent<Renderer>();
            mOriginalMaterials = mRenderer.sharedMaterials;
            mPatchedMaterials = new Material[mOriginalMaterials.Length];
            for (int i = 0; i < mOriginalMaterials.Length; i++) {
                mPatchedMaterials[i] = new Material(mOriginalMaterials[i]);
                mPatchedMaterials[i].color = highlightColor;
            }
        }

        void LateUpdate()
        {
            if (GameController.Instance.playerController.targetObjectForInteraction == this)
                mRenderer.sharedMaterials = mPatchedMaterials;
            else
                mRenderer.sharedMaterials = mOriginalMaterials;
        }

        public virtual bool CanInteract() { return true; }
        public abstract void Interact();
    }
}
