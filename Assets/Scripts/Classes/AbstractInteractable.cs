
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public abstract class AbstractInteractable : MonoBehaviour
    {
        private struct MaterialPatcher
        {
            private Renderer mRenderer;
            private Material[] mOriginalMaterials;
            private Material[] mPatchedMaterials;

            public MaterialPatcher(GameObject highlightTarget, Color highlightColor)
            {
                    mRenderer = highlightTarget.GetComponent<Renderer>();
                    mOriginalMaterials = mRenderer.sharedMaterials;
                    mPatchedMaterials = new Material[mOriginalMaterials.Length];
                    for (int i = 0; i < mOriginalMaterials.Length; i++) {
                        mPatchedMaterials[i] = new Material(mOriginalMaterials[i]);
                        mPatchedMaterials[i].color = highlightColor;
                    }
            }

            public void SetHighlighted(bool flag)
            {
                if (flag)
                    mRenderer.sharedMaterials = mPatchedMaterials;
                else
                    mRenderer.sharedMaterials = mOriginalMaterials;
            }
        }

        public GameObject[] highlightTargets;
        public Color highlightColor = Color.green;

        private readonly List<MaterialPatcher> mMaterialPatchers = new List<MaterialPatcher>();

        void Awake()
        {
            if (highlightTargets == null)
                mMaterialPatchers.Add(new MaterialPatcher(gameObject, highlightColor));
            else {
                foreach (var highlightTarget in highlightTargets)
                    mMaterialPatchers.Add(new MaterialPatcher(highlightTarget, highlightColor));
            }
        }

        void LateUpdate()
        {
            bool highlighted = (GameController.Instance.playerController.targetObjectForInteraction == this);
            foreach (var patcher in mMaterialPatchers)
                patcher.SetHighlighted(highlighted);
        }

        public virtual bool CanInteract() { return true; }
        public abstract void Interact();
    }
}
