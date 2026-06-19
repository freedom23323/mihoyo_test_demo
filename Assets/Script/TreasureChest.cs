using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    /*[Header("奖励设置 (可选)")]
    [SerializeField] private ItemID rewardItemID; // 如果想开箱给东西，可以选一个对应的物品ID
    [SerializeField] private int rewardAmount = 1; // 奖励的数量*/

    [Header("UI 提示")]
    [SerializeField] private GameObject hintText;   // 挂在宝箱身上的提示文字（例如：“按 F 键开启宝箱”）

    private bool isPlayerInZone = false;            // 玩家是否在交互范围内
    private PlayerInventory cachedPlayerInventory;  // 缓存玩家的背包组件

    void Start()
    {
        // 游戏开始时，默认隐藏提示文字
        if (hintText != null)
        {
            hintText.SetActive(false);
        }
    }

    void Update()
    {
        // 玩家在范围内，且按下了 F 键
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F))
        {
            OpenChest();
        }
    }

    // 核心：开启宝箱的逻辑
    private void OpenChest()
    {
        Debug.Log($"【宝箱】开启了宝箱！");

        /*// 联动背包系统：如果拿到了玩家背包组件，就把奖励塞进背包里
        if (cachedPlayerInventory != null)
        {
            cachedPlayerInventory.AddItem(rewardItemID, rewardAmount);
            Debug.Log($"【宝箱】获得了奖励：{rewardItemID} x{rewardAmount}");
        }*/

        // 隐藏提示字，防止销毁时的残留延迟
        if (hintText != null)
        {
            hintText.SetActive(false);
        }

        // 交互完毕，销毁整个宝箱物体（让它消失）
        Destroy(gameObject);
    }

    // ─── 玩家范围检测 ───

    private void OnTriggerEnter(Collider other)
    {
        // 只有带有 "Player" Tag 的物体进入才会触发
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            
            // 顺手获取并缓存玩家身上的背包组件，方便给奖励
            cachedPlayerInventory = other.GetComponent<PlayerInventory>();

            // 显示“按 F 开启”的提示
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
            cachedPlayerInventory = null;

            // 玩家离开，隐藏提示
            if (hintText != null)
            {
                hintText.SetActive(false);
            }
        }
    }
}