using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity2018UpgradeTools
{
    using ADB = AssetDatabase;
    
    public static class GameObjects
    {
        public static IEnumerable<GameObject> Search(string[] searchDirectories)
        {
            return ADB.FindAssets("t:GameObject", searchDirectories).Select(ADB.GUIDToAssetPath).Select(ADB.LoadAssetAtPath<GameObject>);
        }

        public static IEnumerable<GameObject> AllInScene()
        {
            return Enumerable.Range(0, SceneManager.sceneCount).Select(SceneManager.GetSceneAt).SelectMany(_ => _.GetRootGameObjects());
        }

        public static IEnumerable<ParticleSystem> ParticleSystems(this IEnumerable<GameObject> self)
        {
            return self.SelectMany(_ => _.GetComponentsInChildren<ParticleSystem>());
        }

        public static void EnableGPUInstancing(this IEnumerable<ParticleSystem> self)
        {
            var quad = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
            
            foreach (var r in self.Select(_ => _.GetComponent<ParticleSystemRenderer>()))
            {
                if (r.enableGPUInstancing)
                {
                    continue;
                }

                r.enableGPUInstancing = true;

                if (r.renderMode == ParticleSystemRenderMode.Billboard)
                {
                    r.renderMode = ParticleSystemRenderMode.Mesh;
                    r.alignment = ParticleSystemRenderSpace.View;
                    r.mesh = quad;
                }
            }
            
            ADB.SaveAssets();
        }

        public static void Log(this IEnumerable<GameObject> self)
        {
            foreach (var g in self)
            {
                Debug.Log(g.name);
            }
        }

        public static void Log(this IEnumerable<ParticleSystem> self)
        {
            foreach (var ps in self)
            {
                Debug.Log(ps.name);
            }
        }
    }
}