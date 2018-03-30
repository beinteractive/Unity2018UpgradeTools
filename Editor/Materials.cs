using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unity2018UpgradeTools
{
    using ADB = AssetDatabase;
    
    public class Materials
    {
        public static void ShowShaderUsage(string[] searchDirectories)
        {
            foreach (var e in AnalyzeShaderUsage(searchDirectories))
            {
                Debug.Log($"{e.Key}{e.Aggregate("", (s, _) => s + "\n  " + _)}");
            }
        }

        public static IOrderedEnumerable<IGrouping<string, string>> AnalyzeShaderUsage(string[] searchDirectories)
        {
            return ADB
                .FindAssets("t:material", searchDirectories)
                .Select(_ => ADB.GUIDToAssetPath(_))
                .Select(_ => new {Path = _, Shader = ADB.LoadAssetAtPath<Material>(_).shader.name})
                .GroupBy(_ => _.Shader, _ => _.Path)
                .OrderBy(_ => _.Key);
        }
    }
}