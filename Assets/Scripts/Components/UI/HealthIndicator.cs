
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthIndicator : MonoBehaviour
    {
        public AbstractHealthComponent healthComponent;
        public Image visual;

        private void Update()
        {
            float value = healthComponent.CurrentHealth();
            float max = healthComponent.MaxHealth();
            visual.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value / max);
        }
    }
}
