using UnityEngine;

public class ObjectSelfRotator : MonoBehaviour
{
    [Header("控制开关")]
    [SerializeField] private bool isRotating = true; // 外部控制开关：勾选则旋转，取消勾选则静止

    [Header("旋转设置")]
    [SerializeField] private Vector3 customLocalAxis = Vector3.up; // 自定义本地旋转轴（默认沿自身Y轴自转）
    [SerializeField] private float rotateSpeed = 90f;             // 旋转速度（度/秒），正数顺时针，负数逆时针

    void Update()
    {
        // 如果开关关闭，则不执行旋转
        if (!isRotating) return;

        // 核心：使用 Rotate 并指定 Space.Self，使其严格按照自身本地轴旋转
        // Time.deltaTime 确保在不同帧率下旋转速度完全一致
        transform.Rotate(customLocalAxis * rotateSpeed * Time.deltaTime, Space.Self);
    }

    // ─────── 供外部机关/触发器调用的公开接口 ───────

    /// <summary>
    /// 设置旋转状态（开启或关闭）
    /// </summary>
    public void SetRotating(bool state)
    {
        isRotating = state;
        Debug.Log($"{gameObject.name} 的自转状态被设置为: {state}");
    }

    /// <summary>
    /// 切换旋转状态（原先开就关，原先关就开）
    /// </summary>
    public void ToggleRotating()
    {
        isRotating = !isRotating;
        Debug.Log($"{gameObject.name} 的自转状态被切换为: {isRotating}");
    }

    /// <summary>
    /// 动态修改旋转速度
    /// </summary>
    public void ChangeSpeed(float newSpeed)
    {
        rotateSpeed = newSpeed;
    }
}