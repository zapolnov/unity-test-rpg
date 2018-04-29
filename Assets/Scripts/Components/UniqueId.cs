//
// Based on a solution from
// https://answers.unity.com/questions/487121/automatically-assigning-gameobjects-a-unique-and-c.html
//

using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Game
{
    [ExecuteInEditMode]
    public class UniqueId : MonoBehaviour
    {
        public string guid;

      #if UNITY_EDITOR
        private static Dictionary<string, UniqueId> mRegistry = new Dictionary<string, UniqueId>();

        void Update()
        {
            if (Application.isPlaying)
                return;

            string sceneName = gameObject.scene.name;
            if (sceneName == null)  // this is a prefab
                return;

            if (guid != null && guid.Length >= 0) {
                string[] parts = guid.Split(':');
                if (parts.Length == 2 && parts[0] == sceneName) {
                    if (!mRegistry.ContainsKey(guid)) {
                        mRegistry[guid] = this;
                        return;
                    }

                    if (mRegistry[guid] == this)
                        return;
                }
            }

            guid = sceneName + ':' + Guid.NewGuid();
            EditorUtility.SetDirty(this);
            EditorSceneManager.MarkSceneDirty(gameObject.scene);

            mRegistry[guid] = this;
        }

        void OnDestroy()
        {
            if (guid != null && guid.Length > 0)
                mRegistry.Remove(guid);
        }
      #endif
    }
}
