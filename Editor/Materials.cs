using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Unity2018UpgradeTools
{
    using ADB = AssetDatabase;
    
    public static class Materials
    {
        public static IEnumerable<Material> SearchMaterials(string[] searchDirectories)
        {
            return ADB.FindAssets("t:material", searchDirectories).Select(ADB.GUIDToAssetPath).Select(ADB.LoadAssetAtPath<Material>);
        }
        
        public static IOrderedEnumerable<IGrouping<string, string>> AnalyzeShaderUsage(this IEnumerable<Material> materials)
        {
            return materials
                .Select(_ => new {Path = ADB.GetAssetPath(_), Shader = _.shader.name})
                .GroupBy(_ => _.Shader, _ => _.Path)
                .OrderBy(_ => _.Key);
        }

        public static IEnumerable<Material> FindMissingGPUInstancingMaterials(this IEnumerable<Material> materials)
        {
            var hasInstancing = typeof(ShaderUtil).GetMethod("HasInstancing", BindingFlags.NonPublic | BindingFlags.Static);
            if (hasInstancing == null)
            {
                return null;
            }

            return materials
                .Where(_ => (bool) hasInstancing.Invoke(null, new object[] { _.shader }))
                .Where(_ => !_.enableInstancing);
        }

        public static void Log(this IEnumerable<IGrouping<string, string>> self)
        {
            foreach (var e in self)
            {
                Debug.Log($"{e.Key}{e.Aggregate("", (s, _) => s + "\n  " + _)}");
            }
        }

        public static void Log(this IEnumerable<Material> self)
        {
            foreach (var m in self)
            {
                Debug.Log(ADB.GetAssetPath(m));
            }
        }

        public static void Update(this IEnumerable<Material> self, Action<Material> f)
        {
            foreach (var m in self)
            {
                f(m);
            }
            
            ADB.SaveAssets();
        }

        public static void EnableGPUInstancing(this IEnumerable<Material> self)
        {
            self.Update(m => m.enableInstancing = true);
        }
    }
}