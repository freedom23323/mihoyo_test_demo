using UnityEngine;

public class BatchChangeMaterial : MonoBehaviour
{
    [Header("拖入你想替换的目标材质")]
    public Material targetMaterial;

    [ContextMenu("一键替换所有子物体材质")]
    public void ChangeAllMaterials()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("请先在 Inspector 中指定目标材质！");
            return;
        }

        // 获取当前物体及所有子物体身上的 MeshRenderer
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material = targetMaterial;
        }

        Debug.Log($"成功一键替换了 {renderers.Length} 个物体的材质！");
    }
}
