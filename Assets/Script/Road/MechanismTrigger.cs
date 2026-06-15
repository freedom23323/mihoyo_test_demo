using UnityEngine;

public class MechanismTrigger : MonoBehaviour
{
    [Header("绑定对应的道路控制器")]
    [SerializeField] private RoadController roadController;

    private bool isPlayerInZone = false; // 玩家是否在交互范围内

    void Update()
    {
        // 当玩家在范围内，并且按下 F 键时触发
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F))
        {
            if (roadController != null)
            {
                roadController.ToggleRoad();
            }
        }
    }

    // 触发检测：玩家进入机关范围
    private void OnTriggerEnter(Collider other)
    {
        // 假设你的玩家物体 Tag 设置为 "Player"
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log("接近机关，按 F 键交互");
            // 这里可以触发 UI 提示，比如显示 “按 F 激活”
        }
    }

    // 触发检测：玩家离开机关范围
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            Debug.Log("离开机关范围");
            // 这里可以隐藏 UI 提示
        }
    }
}