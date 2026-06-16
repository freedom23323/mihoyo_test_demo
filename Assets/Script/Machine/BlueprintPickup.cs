using UnityEngine;

public class BlueprintPickup : MonoBehaviour
{
    [Header("蓝图设置")]
    [SerializeField] private ItemID blueprintToUnlock;   // 在下拉菜单中选择这个蓝图解锁后对应的物品ID

    [Header("提示UI（可选）")]
    [SerializeField] private GameObject hintText;        // 挂在蓝图身上的提示文字（例如：“按 F 键学习蓝图”）

    private bool isPlayerInZone = false;                 // 玩家是否在拾取范围内
    private PlayerInventory cachedPlayerInventory;       // 缓存进入范围的玩家背包组件

    void Start()
    {
        // 初始隐藏提示字体
        if (hintText != null)
        {
            hintText.SetActive(false);
        }
    }

    void Update()
    {
        // 玩家在范围内、按下 F 键、且成功获取到了玩家的背包组件
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F) && cachedPlayerInventory != null)
        {
            // 1. 调用背包系统的解锁蓝图方法（内部已做去重处理）
            cachedPlayerInventory.UnlockBlueprint(blueprintToUnlock);

            Debug.Log($"成功学会了蓝图: {blueprintToUnlock}！现在可以制作该物品了。");

            // 2. 拾取/学习成功，销毁场景中的这个蓝图物体
            Destroy(gameObject);
        }
    }

    // --- 玩家范围检测 ---

    private void OnTriggerEnter(Collider other)
    {
        // 确保是带有 "Player" 标签的玩家走过来了
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            
            // 动态获取并缓存玩家身上的背包组件
            cachedPlayerInventory = other.GetComponent<PlayerInventory>();

            // 显示“按F键学习”的提示
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
            
            // 玩家离开，清空引用
            cachedPlayerInventory = null;

            // 隐藏提示
            if (hintText != null)
            {
                hintText.SetActive(false);
            }
        }
    }
}