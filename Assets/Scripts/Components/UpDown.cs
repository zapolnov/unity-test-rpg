
using UnityEngine;

namespace Game
{
    public class UpDown : MonoBehaviour
    {
        public float delta;
        public float speed;

        private Vector3 mOriginalPosition;
        private float mOffset;
        private float mDirection;

        void Awake()
        {
            mOriginalPosition = transform.localPosition;
            mOffset = 0.0f;
            mDirection = 1.0f;
        }

        void Update()
        {
            mOffset += mDirection * speed * Time.deltaTime;
            for (;;) {
                if (mOffset >= delta) {
                    mOffset = delta - (mOffset - delta);
                    mDirection = -1.0f;
                }

                if (mOffset <= -delta) {
                    mOffset = -delta + (-mOffset - delta);
                    mDirection = 1.0f;
                    continue;
                }

                break;
            }

            transform.localPosition = mOriginalPosition + new Vector3(0.0f, mOffset, 0.0f);
        }
    }
}
