using UnityEngine;
using System.Collections;

public class DoorRotator : MonoBehaviour
{
    [Header("引用设置")]
    [SerializeField] private Transform doorPivot;     // 拖入你的父物体（门轴）
    [SerializeField] private GameObject hintText;     // 拖入挂在门身上的文字物体

    [Header("旋转设置")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // 沿Y轴旋转
    [SerializeField] private float rotateAngle = 90f;          // 瞬时针90度
    [SerializeField] private float duration = 0.5f;             // 旋转动画时间

    [Header("解锁设置")]
    [SerializeField] private ItemID requiredKeyID;            // 需要持有的钥匙枚举（删除了None，默认都是有效值）
    [SerializeField] private bool consumeKeyOnUse = false;     // 使用后是否消耗掉这把钥匙？

    private bool isPlayerInZone = false; 
    private bool isRotating = false;     
    private Quaternion targetRotation;   
    
    // 缓存进入范围的玩家背包组件
    private PlayerInventory cachedPlayerInventory;

    void Start()
    {
        if (doorPivot == null && transform.parent != null)
        {
            doorPivot = transform.parent;
        }

        if (hintText != null)
        {
            hintText.SetActive(false);
        }

        if (doorPivot != null)
        {
            targetRotation = doorPivot.localRotation;
        }
    }

    void Update()
    {
        // 玩家在范围内、按下F键、且没有在旋转
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F) && !isRotating && doorPivot != null)
        {
            // 核心验证：检查玩家背包里是否有这把钥匙
            if (cachedPlayerInventory != null && cachedPlayerInventory.GetItemCount(requiredKeyID) > 0)
            {
                // 如果设置了“使用后消耗钥匙”
                if (consumeKeyOnUse)
                {
                    cachedPlayerInventory.RemoveItem(requiredKeyID, 1);
                }

                // 计算门轴的目标旋转并播放动画
                targetRotation = targetRotation * Quaternion.AngleAxis(rotateAngle, rotationAxis);
                StartCoroutine(AnimateRotation());
            }
            else
            {
                Debug.LogWarning($"你没有对应的钥匙: {requiredKeyID}，无法开门！");
                // 这里你也可以扩展一段 UI 逻辑，比如屏幕弹窗提示：“需要钥匙：xxx”
            }
        }
    }

    private IEnumerator AnimateRotation()
    {
        isRotating = true;
        float elapsed = 0f;
        Quaternion startRotation = doorPivot.localRotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t); 
            
            doorPivot.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        doorPivot.localRotation = targetRotation;
        isRotating = false;
    }

    // --- 玩家范围检测 ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            
            // 尝试获取玩家身上的背包组件并缓存起来
            cachedPlayerInventory = other.GetComponent<PlayerInventory>();

            if (hintText != null)
            {
                hintText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            
            // 玩家离开时，清空背包组件引用引用，释放内存
            cachedPlayerInventory = null;

            if (hintText != null)
            {
                hintText.SetActive(false);
            }
        }
    }
}