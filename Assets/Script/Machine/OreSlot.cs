using StarterAssets;
using UnityEngine;

public class OreSlot : MonoBehaviour
{
    [Header("关联的发光控制器（把那台机器的父物体拖进来）")]
    public GroupGlowController machineGlowManager;

    [Header("槽位自己的视觉表现（比如槽里原本是空的，放了矿后显示的矿石模型）")]
    public GameObject slotOreVisual;

    public GameObject interactHintUI; // “按 F 放入矿石”的世界空间UI
    
    private bool isPlayerInZone = false;
    private ThirdPersonController cachedPlayer;
    private bool hasOreInside = false; // 槽位自己是否已经被填满了

    void Start()
    {
        if (interactHintUI != null) interactHintUI.SetActive(false);
        if (slotOreVisual != null) slotOreVisual.SetActive(false);
    }

    void Update()
    {
        // 玩家在区域内、按下 F、槽位是空的，且【玩家手上有矿】
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F) && !hasOreInside)
        {
            if (cachedPlayer != null && cachedPlayer.isCarryingOre)
            {
                cachedPlayer.DropOre(); // 扣除玩家手上的矿
                hasOreInside = true;

                // 槽位视觉表现：显示槽内的矿石
                if (slotOreVisual != null) slotOreVisual.SetActive(true);

                // 【核心联动】联动你之前的脚本，让整台机器的所有子节点批量发光！
                if (machineGlowManager != null)
                {
                    machineGlowManager.SetGroupGlow(true); 
                }

                // 成功放入后，隐藏提示 UI
                if (interactHintUI != null) interactHintUI.SetActive(false);
                Debug.Log("矿石已成功交付，机器开始发光运转！");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasOreInside)
        {
            ThirdPersonController carrier = other.GetComponent<ThirdPersonController>();
            if (carrier != null)
            {
                isPlayerInZone = true;
                cachedPlayer = carrier;

                // 只有玩家手里拿着矿，靠近槽位才会提示“按 F 放入”
                if (carrier.isCarryingOre && interactHintUI != null) 
                    interactHintUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            cachedPlayer = null;
            if (interactHintUI != null) interactHintUI.SetActive(false);
        }
    }
}