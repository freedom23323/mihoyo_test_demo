using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour
{
    [Header("楼层高度设置")]
    [SerializeField] private float firstFloorY;          // 一楼的 Y 轴高度
    [SerializeField] private float secondFloorY;         // 二楼的 Y 轴高度

    [Header("电梯移动设置")]
    [SerializeField] private float moveSpeed = 3f;       // 电梯移动速度
    [SerializeField] private float startDelay = 1f;      // 玩家踩上去后，延迟几秒出发

    private int currentTargetFloor = 1;                  // 当前目标楼层 (1 或 2)
    private bool isMoving = false;                       // 电梯是否正在移动中
    private float targetY;                               // 实时目标 Y 坐标

    void Start()
    {
        // 游戏开始时，默认电梯在一楼
        targetY = firstFloorY;
        Vector3 pos = transform.position;
        pos.y = firstFloorY;
        transform.position = pos;
    }

    /// <summary>
    /// 公开方法：供外部呼叫开关调用
    /// </summary>
    /// <param name="floor">呼叫电梯去往的楼层(1或2)</param>
    public void CallElevator(int floor)
    {
        if (floor != 1 && floor != 2) return;

        // 更新目标楼层和高度
        currentTargetFloor = floor;
        targetY = (floor == 1) ? firstFloorY : secondFloorY;

        // 如果当前没在动，立即开始移动
        if (!isMoving)
        {
            StartCoroutine(MoveElevator());
        }
    }

    /// <summary>
    /// 电梯平滑移动的协程
    /// </summary>
    private IEnumerator MoveElevator()
    {
        isMoving = true;
        Debug.Log($"电梯出发，前往 {currentTargetFloor} 楼...");

        // 当电梯与目标高度的差距大于微小值时，持续移动
        while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
        {
            Vector3 currentPos = transform.position;
            // 使用 MoveTowards 实现匀速平滑移动
            currentPos.y = Mathf.MoveTowards(currentPos.y, targetY, moveSpeed * Time.deltaTime);
            transform.position = currentPos;

            yield return null;
        }

        // 精准校准最终高度
        Vector3 finalPos = transform.position;
        finalPos.y = targetY;
        transform.position = finalPos;

        isMoving = false;
        Debug.Log($"电梯已到达 {currentTargetFloor} 楼。");
    }

    // ─── 玩家踏上电梯自动前往另一层 ───

    private void OnTriggerEnter(Collider other)
    {
        // 如果是玩家踩了上来，且电梯当前是静止的
        if (other.CompareTag("Player") && !isMoving)
        {
            // 判定当前电梯停在哪一层，自动将目标设为“另一层”
            // 如果在1楼高度附近，目标就是2楼；反之就是1楼
            if (Mathf.Abs(transform.position.y - firstFloorY) < 0.5f)
            {
                currentTargetFloor = 2;
                targetY = secondFloorY;
            }
            else
            {
                currentTargetFloor = 1;
                targetY = firstFloorY;
            }

            // 延迟一会再启动（给玩家站稳的时间，体验更好）
            StartCoroutine(DelayStart());
        }
    }

    private IEnumerator DelayStart()
    {
        Debug.Log($"玩家已踏上电梯，{startDelay}秒后自动前往 {currentTargetFloor} 楼...");
        yield return new WaitForSeconds(startDelay);
        
        // 再次确认电梯此时没在运行，才开始移动
        if (!isMoving)
        {
            StartCoroutine(MoveElevator());
        }
    }

    // ─── 物理小优化：让玩家站在电梯上时不会滑落 ───
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 将玩家变成电梯的子物体，这样电梯动，玩家也会跟着绝对同步移动
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 玩家离开电梯，解除父子关系
            collision.transform.SetParent(null);
        }
    }
}