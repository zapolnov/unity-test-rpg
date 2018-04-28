
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
                foreach (var system in mInactive) {
                    if (system != null)
                        GameObject.Destroy(system.gameObject);
                }
                mInactive.Clear();

                foreach (var system in mActive) {
                    if (system != null)
                        GameObject.Destroy(system.gameObject);
                }
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
                    if (mActive[i] != null && mActive[i].IsAlive())
                        ++i;
                    else {
                        var particleSystem = mActive[i];
                        if (particleSystem != null) {
                            particleSystem.Stop();
                            particleSystem.gameObject.SetActive(false);
                            mInactive.Push(particleSystem);
                        }

                        mActive[i] = mActive[n - 1];
                        mActive.RemoveAt(n - 1);
                        --n;
                    }
                }
            }
        }

        public ParticleSystem bloodParticlesPrefab;
        public ParticleSystem greenBloodParticlesPrefab;
        public ParticleSystem skeletonParticlesPrefab;

        private ParticleSet mBloodParticles = new ParticleSet();
        private ParticleSet mGreenBloodParticles = new ParticleSet();
        private ParticleSet mSkeletonParticles = new ParticleSet();

        public void Clear()
        {
            mBloodParticles.Clear();
            mGreenBloodParticles.Clear();
            mSkeletonParticles.Clear();
        }

        public void SpawnBloodParticles(Vector3 position)
        {
            mBloodParticles.Spawn(position, bloodParticlesPrefab);
        }

        public void SpawnGreenBloodParticles(Vector3 position)
        {
            mGreenBloodParticles.Spawn(position, greenBloodParticlesPrefab);
        }

        public void SpawnSkeletonParticles(Vector3 position)
        {
            mSkeletonParticles.Spawn(position, skeletonParticlesPrefab);
        }

        public void Update()
        {
            mBloodParticles.Update();
            mGreenBloodParticles.Update();
            mSkeletonParticles.Update();
        }
    }
}
