using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("物品设置")]
    [SerializeField] private ItemID itemToPick;       // 在下拉菜单中选择这把钥匙对应的枚举ID
    [SerializeField] private int amount = 1;          // 拾取该物体时增加的数量（默认1个）

    [Header("提示UI（可选）")]
    [SerializeField] private GameObject hintText;     // 挂在钥匙身上的提示文字（例如：“按 F 键拾取”）

    private bool isPlayerInZone = false;              // 玩家是否在拾取范围内
    private PlayerInventory cachedPlayerInventory;    // 缓存进入范围的玩家背包组件

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
            // 1. 将物品和数量添加进玩家的哈希表背包中
            cachedPlayerInventory.AddItem(itemToPick, amount);

            Debug.Log($"成功捡起 {itemToPick} x{amount}！");

            // 2. 拾取成功，销毁场景中的这个钥匙物体
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

            // 显示“按F拾取”的提示
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