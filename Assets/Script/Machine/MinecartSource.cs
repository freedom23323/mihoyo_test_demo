using StarterAssets;
using UnityEngine;

public class MinecartSource : MonoBehaviour
{
    public GameObject interactHintUI; // “按 F 拿取矿石”的世界空间UI
    private bool isPlayerInZone = false;
    private ThirdPersonController cachedPlayer;

    void Start()
    {
        if (interactHintUI != null) interactHintUI.SetActive(false);
    }

    void Update()
    {
        // 玩家在区域内、按下 F，且【手上没有矿】才能拿
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F))
        {
            if (cachedPlayer != null && !cachedPlayer.isCarryingOre)
            {
                cachedPlayer.PickUpOre(); // 玩家获得矿
                Debug.Log("玩家从矿车拿取了矿石！");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ThirdPersonController carrier = other.GetComponent<ThirdPersonController>();
            if (carrier != null)
            {
                isPlayerInZone = true;
                cachedPlayer = carrier;
                // 只有手上有空位，才显示交互提示
                if (!carrier.isCarryingOre && interactHintUI != null) interactHintUI.SetActive(true);
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