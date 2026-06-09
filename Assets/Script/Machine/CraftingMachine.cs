using UnityEngine;
using UnityEngine.UI; // 如果使用 TextMeshPro，请改为 using TMPro;

public class CraftingMachine : MonoBehaviour
{
    [Header("机器支持制作的3个图纸")]
    public BlueprintData[] availableBlueprints = new BlueprintData[3];

    [Header("物体生成的位置（地面上某个Transform）")]
    public Transform spawnPoint;

    [Header("关联的UI面板")]
    public MachineUIController uiController;

    [Header("交互提示 UI (比如写着 '按 E 交互' 的 Text 物体)")]
    public GameObject interactHintUI; 

    private bool isPlayerInZone = false;       // 玩家是否在触发区域内
    private PlayerInventory cachedPlayerInventory; // 缓存进入区域的玩家背包组件

    void Start()
    {
        // 游戏开始时，默认隐藏“按 E 交互”的提示
        if (interactHintUI != null)
        {
            interactHintUI.SetActive(false);
        }
    }

    void Update()
    {
        // 【核心按键监听】如果玩家在区域内，且按下了 E 键
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            // 确保拿到了玩家的背包数据，再触发交互
            if (cachedPlayerInventory != null)
            {
                Interact(cachedPlayerInventory);
            }
        }
    }

    // 当玩家进入 Trigger 区域
    private void OnTriggerEnter(Collider other)
    {
        // 检查进来的物体是不是玩家
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                isPlayerInZone = true;
                cachedPlayerInventory = inventory; // 缓存起来供 Update 里面的按键使用

                // 显示“按 E 交互”提示
                if (interactHintUI != null)
                {
                    interactHintUI.SetActive(true);
                }
            }
        }
    }

    // 当玩家离开 Trigger 区域
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            cachedPlayerInventory = null; // 清空缓存

            // 隐藏“按 E 交互”提示
            if (interactHintUI != null)
            {
                interactHintUI.SetActive(false);
            }

            // 【体验优化】如果玩家打开着制作UI直接走开了，自动帮他把制作UI也关闭
            if (uiController != null)
            {
                uiController.CloseUI();
            }
        }
    }

    // 开启制作 UI 面板
    public void Interact(PlayerInventory playerInventory)
    {
        if (uiController != null)
        {
            // 打开 UI，并将数据传过去
            uiController.OpenUI(availableBlueprints, playerInventory, this);
            
            // 打开复杂的制作面板后，可以把临时的“按 E 交互”提示先隐藏掉，防止挡镜头
            if (interactHintUI != null)
            {
                interactHintUI.SetActive(false);
            }
        }
    }

    // 真正把物体实例化到地面上
    public void SpawnObject(GameObject prefab)
    {
        if (prefab != null && spawnPoint != null)
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("物体生成成功！");
        }
    }
}