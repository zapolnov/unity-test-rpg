
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class ParticleManager
    {
        private class ParticleSet
        {
            private readonly Stack<ParticleSystem> mInactive = new Stack<ParticleSystem>();
            private readonly List<ParticleSystem> mActive = new List<ParticleSystem>();

            public void Clear()
            {
                foreach (var system in mInactive)
                    GameObject.Destroy(system.gameObject);
                mInactive.Clear();

                foreach (var system in mActive)
                    GameObject.Destroy(system.gameObject);
                mActive.Clear();
            }

            public void Spawn(Vector3 position, ParticleSystem prefab)
            {
                ParticleSystem particleSystem;
                if (mInactive.Count == 0) {
                    var gameObject = GameObject.Instantiate(prefab.gameObject, position, new Quaternion());
                    particleSystem = gameObject.GetComponent<ParticleSystem>();
                }  else {
                    particleSystem = mInactive.Pop();
                    particleSystem.transform.position = position;
                    particleSystem.gameObject.SetActive(true);
                    particleSystem.Play();
                }

                mActive.Add(particleSystem);
            }

            public void Update()
            {
                int n = mActive.Count;
                for (int i = 0; i < n; ) {
                    if (mActive[i].IsAlive())
                        ++i;
                    else {
                        var particleSystem = mActive[i];
                        particleSystem.Stop();
                        particleSystem.gameObject.SetActive(false);
                        mInactive.Push(particleSystem);

                        mActive[i] = mActive[n - 1];
                        mActive.RemoveAt(n - 1);
                        --n;
                    }
                }
            }
        }

        public ParticleSystem bloodParticlesPrefab;

        private ParticleSet mBloodParticles = new ParticleSet();

        public void Clear()
        {
            mBloodParticles.Clear();
        }

        public void SpawnBloodParticles(Vector3 position)
        {
            mBloodParticles.Spawn(position, bloodParticlesPrefab);
        }

        public void Update()
        {
            mBloodParticles.Update();
        }
    }
}
