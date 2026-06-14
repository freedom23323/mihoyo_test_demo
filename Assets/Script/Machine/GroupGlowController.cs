using UnityEngine;

public class GroupGlowController : MonoBehaviour
{
    // 缓存所有子节点的 Renderer
    private Renderer[] childRenderers;
    private MaterialPropertyBlock propBlock;

    [Header("全局发光状态开关")]
    public bool isGlowing = false;

    [Header("发光设置")]
    [ColorUsage(true, true)]
    public Color glowColor = Color.green;
    public float activeIntensity = 3.0f;

    // 用来记录上一次的状态，防止在 Update 里频繁执行重复逻辑（优化性能）
    private bool lastGlowingState;

    void Start()
    {
        // 【核心操作】自动获取父物体以及所有子物体身上的 Renderer 组件
        childRenderers = GetComponentsInChildren<Renderer>();
        propBlock = new MaterialPropertyBlock();

        // 游戏开始时同步一次初始状态
        UpdateGroupGlow();
        lastGlowingState = isGlowing;
    }

    void Update()
    {
        // 只有当我们在 Inspector 里勾选开关改变了状态，才触发更新，避免每帧大循环
        if (isGlowing != lastGlowingState)
        {
            UpdateGroupGlow();
            lastGlowingState = isGlowing;
        }
    }

    // 执行批量控光的函数
    private void UpdateGroupGlow()
    {
        if (childRenderers == null || childRenderers.Length == 0) return;

        // 计算出当前的 HDR 颜色
        Color finalColor = isGlowing ? glowColor * Mathf.Pow(2, activeIntensity) : Color.black;

        // 【大循环】遍历每一个子渲染器，把颜色送进去
        for (int i = 0; i < childRenderers.Length; i++)
        {
            if (childRenderers[i] == null) continue;

            // 获取当前子物体的属性块
            childRenderers[i].GetPropertyBlock(propBlock);
            
            // 修改发光通道
            propBlock.SetColor("_EmissionColor", finalColor);
            
            // 还给子物体
            childRenderers[i].SetPropertyBlock(propBlock);
        }
    }

    // 方便别的机关/触发器代码直接调用的公共接口
    public void SetGroupGlow(bool status)
    {
        isGlowing = status;
    }
}