using Script;
using UnityEngine;

public class CoolingTank : MonoBehaviour
{
    [Header("引用设置")]
    [SerializeField] private GameObject iceVisualObject;       // 降温槽内部的冰块模型（默认隐藏）
    [SerializeField] private CoolingTankDoor targetDoor;       // 联动开启的特殊门（拖入挂有CoolingTankDoor的物体）
    [SerializeField] private GameObject hintText;              // “按 F 放入冰块降温”的提示字
    [SerializeField] private GameObject key2BluePrint;
    
    [Header("物品设置")]
    [SerializeField] private ItemID requiredIceID = ItemID.ice; 
    [SerializeField] private bool consumeIce = true;            // 放入后是否扣除背包里的冰块

    [SerializeField] private GroupGlowController thermometer;
    [SerializeField] private Color thermometerColor= Color.magenta;
    public bool isHot = true;
    
    private bool isPlayerInZone = false;
    private PlayerInventory cachedPlayerInventory;
    private bool isActivated = false;                          // 降温槽是否已经激活过，防止重复触发

    void Start()
    {
        // 初始确保冰块模型隐藏，提示隐藏
        if (iceVisualObject != null) iceVisualObject.SetActive(false);
        if (hintText != null) hintText.SetActive(false);
        if (key2BluePrint!= null) key2BluePrint.SetActive(false);
    }

    void Update()
    {
        // 玩家在范围内、按下 F、未激活、且拿到了背包组件
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F) && !isActivated && cachedPlayerInventory != null)
        {
            // 检查玩家背包里是否有冰块
            if (cachedPlayerInventory.GetItemCount(requiredIceID) > 0)
            {
                ActivateTank();
            }
            else
            {
                Debug.LogWarning($"你没有 [{requiredIceID}]，无法为降温槽降温！");
            }
        }
    }

    // 激活降温槽的核心逻辑
    public void ActivateTank()
    {
        isActivated = true;

        // 1. 扣除玩家背包里的冰块
        if (consumeIce)
        {
            cachedPlayerInventory.RemoveItem(requiredIceID, 1);
        }

        // 2. 显示降温槽内部的冰块模型
        if (iceVisualObject != null)
        {
            iceVisualObject.SetActive(true);
        }

        // 3. 隐藏提示文字
        if (hintText != null)
        {
            hintText.SetActive(false);
        }

        if (thermometer != null)
        {
            thermometer.SetGlowColor(thermometerColor);
        }
        
        if (key2BluePrint!= null) key2BluePrint.SetActive(true);
        
        // 4. 核心联动：命令特殊的门打开
        if (targetDoor != null)
        {
            targetDoor.OpenDoorByTank();
        }
        
        else
        {
            Debug.LogError("未绑定联动的特殊门(Target Door)！");
        }

        Debug.Log("降温槽已成功放入冰块，触发开门！");
    }
    public void ActivateTankByRayCharger()
    {
        isActivated = true;

        if (iceVisualObject != null)
        {
            iceVisualObject.SetActive(true);
        }

        if (thermometer != null)
        {
            thermometer.SetGlowColor(thermometerColor);
        }
        
        if (key2BluePrint!= null) key2BluePrint.SetActive(true);
        
        if (targetDoor != null)
        {
            targetDoor.OpenDoorByTank();
        }
        
        else
        {
            Debug.LogError("未绑定联动的特殊门(Target Door)！");
        }

        Debug.Log("降温槽已有冰块，触发开门！");
    }
    // ─── 玩家范围检测 ───

    private void OnTriggerEnter(Collider other)
    {
        // 如果已经激活过了，就不再有任何提示
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            cachedPlayerInventory = other.GetComponent<PlayerInventory>();

            if (hintText != null) hintText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            cachedPlayerInventory = null;

            if (hintText != null) hintText.SetActive(false);
        }
    }
}