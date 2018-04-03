# Unity 2018 Upgrade Tools

This library helps you to upgrade your existing project to Unity 2018.

## Analyze Shader Usage

```C#
using Unity2018UpgradeTools;
using UnityEditor;

public class AnalyzeShaderUsage
{
    [MenuItem("Assets/Analyze Shader Usage")]
    static void Analyze()
    {
        Materials.ShowShaderUsage(new[] { "Assets/Materials" });
    }
}
```

This will generate logs in console that indicates which shaders are used by materials.
It's useful to find shaders/materials you should rewrite for SRP compatible.
