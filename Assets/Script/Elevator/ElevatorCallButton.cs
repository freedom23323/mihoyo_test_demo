using UnityEngine;

public class ElevatorCallButton : MonoBehaviour
{
    [Header("绑定对应的电梯主控制器")]
    [SerializeField] private ElevatorController elevatorController;

    [Header("当前呼叫机所在的楼层")]
    [Range(1, 2)] 
    [SerializeField] private int thisFloor = 1; // 1楼的机关填1，2楼的机关填2

    [Header("提示UI（可选）")]
    [SerializeField] private GameObject hintText;   // “按 F 呼叫电梯”

    private bool isPlayerInZone = false;

    void Start()
    {
        if (hintText != null) hintText.SetActive(false);
    }

    void Update()
    {
        // 玩家在机关前按 F 键呼叫
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F))
        {
            if (elevatorController != null)
            {
                // 通知电梯：来我这一层！
                elevatorController.CallElevator(thisFloor);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (hintText != null) hintText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (hintText != null) hintText.SetActive(false);
        }
    }
}