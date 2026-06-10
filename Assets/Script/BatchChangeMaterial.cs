using UnityEngine;

public class BatchChangeMaterial : MonoBehaviour
{
    [Header("拖入你想替换的目标材质")]
    public Material targetMaterial;

    // 在 Inspector 面板中右键点击该脚本组件，或者点击右上角三个点，会看到这个按钮
    [ContextMenu("执行批量替换材质")]
    public void ApplyMaterialToChildren()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("请先在 Target Material 槽位中拖入材质球！");
            return;
        }

        // 核心：获取这个组（父物体）下面所有的 MeshRenderer（包括常规网格和普通网格）
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
        // 如果你的模型带骨骼动画，也可以同时获取 SkinnedMeshRenderer
        SkinnedMeshRenderer[] skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);

        int count = 0;

        // 遍历所有普通网格并替换材质
        foreach (var renderer in meshRenderers)
        {
            Material[] newMaterials = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = targetMaterial;
            }
            renderer.sharedMaterials = newMaterials;
            count++;
        }

        // 遍历所有骨骼网格并替换材质
        foreach (var renderer in skinnedRenderers)
        {
            Material[] newMaterials = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = targetMaterial;
            }
            renderer.sharedMaterials = newMaterials;
            count++;
        }

        Debug.Log($"成功为 {count} 个子物体网格替换了材质！");
    }
}