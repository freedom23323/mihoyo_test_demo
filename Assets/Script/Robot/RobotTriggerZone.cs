using UnityEngine;

public class RobotTriggerZone : MonoBehaviour
{
    [Header("联动机器人")]
    [SerializeField] private RobotWaypointPatrol targetRobot; // 拖入你想要控制的那个机器人

    [Header("触发设置")]
    [SerializeField] private bool moveStateOnEnter = true;   // 玩家进入时，希望机器人移动(true)还是停止(false)
    [SerializeField] private bool triggerOnlyOnce = true;    // 是否只触发一次（防止玩家反复踩踏刷Log）

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 判定是否是玩家进入，且如果设置了只触发一次，检查是否已触发
        if (other.CompareTag("Player") && (!triggerOnlyOnce || !hasTriggered))
        {
            if (targetRobot != null)
            {
                // 核心：远程调用机器人的新函数
                targetRobot.SetIsMoving(moveStateOnEnter);
                
                hasTriggered = true;
                Debug.Log($"玩家进入触发区域 [{gameObject.name}]，已成功发送指令给机器人。");
                
                // 如果是一次性陷阱/机关，可以在这里选择直接销毁触发器
                // Destroy(gameObject); 
            }
            else
            {
                Debug.LogError($"触发器 [{gameObject.name}] 未绑定目标机器人(Target Robot)！");
            }
        }
    }
}