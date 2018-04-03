# Unity 2018 Upgrade Tools

This library helps you to upgrade your existing project to Unity 2018.

## Analyze Shader Usage

```C#
using Unity2018UpgradeTools;
using UnityEditor;

public class Unity2018Upgrading
{
    [MenuItem("Assets/Analyze Shader Usage")]
    static void Analyze()
    {
        Materials.SearchMaterials(new[] { "Assets/Materials" }).AnalyzeShaderUsage().Log();
    }
}
```

This will generate logs in console that indicates which shaders are used by materials.

It's useful to find shaders/materials you should rewrite for SRP compatible.

## Find & Update Missing GPU Instancing Materials

```C#
using Unity2018UpgradeTools;
using UnityEditor;

public class Unity2018Upgrading
{
    [MenuItem("Assets/Bulk Update Missing GPU Instancing/Dry Run")]
    static void ShowMissingGPUInstancing()
    {
        Materials.SearchMaterials(new[] { "Assets/Materials" }).FindMissingGPUInstancingMaterials().Log();
    }

    [MenuItem("Assets/Bulk Update Missing GPU Instancing/Run")]
    static void UpdateMissingGPUInstancing()
    {
        Materials.SearchMaterials(new[] { "Assets/Materials" }).FindMissingGPUInstancingMaterials().EnableGPUInstancing();
    }
}
```

This will find materials with a shader that has a GPU Instancing variant but with a `Enable GPU Instancing` field is off.

- `Dry Run` will display such materials in console.
- `Run` will set material's `Enable GPU Instancing` to on.
