using UnityEngine;

public class RobotWaypointPatrol : MonoBehaviour
{
    [Header("控制开关")]
    [SerializeField] private bool isMoving = true; // 公开的开关：控制机器人是否开始/继续移动

    [Header("路点设置")]
    [SerializeField] private Transform[] waypoints; // 存放所有路点的数组
    [SerializeField] private float moveSpeed = 3f;      // 机器人移动速度
    [SerializeField] private float rotationSpeed = 5f;  // 机器人转向路点的旋转速度
    [SerializeField] private float arrivalDistance = 0.2f; // 判定到达路点的距离阈值

    [Header("巡逻模式")]
    [SerializeField] private bool loop = true;          // 是否循环巡逻（从最后一个点走回第一个点）

    [Header("物理悬空检测")]
    [SerializeField] private float groundCheckDistance = 0.3f; // 射线射出距离（略长于脚底到地面的距离）
    
    private int currentWaypointIndex = 0;               // 当前目标路点的索引
    private bool isPatrolComplete = false;              // 非循环模式下，是否已经走完所有点
    private bool isGrounded = true;
    void Update()
    {
        CheckGround();
        // 如果公共开关被关闭，或者巡逻已结束，则机器人原地待命
        if (!isMoving || isPatrolComplete || waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        Patrol();
    }

    private void Patrol()
    {
        // 获取当前目标路点
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // 1. 移动逻辑
        // 计算去往目标点的方向（忽略Y轴高度差，防止机器人平移倾斜）
        Vector3 targetPosition = new Vector3(targetWaypoint.position.x, transform.position.y, targetWaypoint.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 2. 转向逻辑（让机器人面朝它要去的路点）
        Vector3 moveDirection = targetPosition - transform.position;
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 3. 到达路点判定
        if (Vector3.Distance(transform.position, targetPosition) < arrivalDistance)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        currentWaypointIndex++;

        // 如果走到了最后一个路点
        if (currentWaypointIndex >= waypoints.Length)
        {
            if (loop)
            {
                // 循环模式：重置回第一个路点
                currentWaypointIndex = 0;
            }
            else
            {
                // 非循环模式：停在最后一个点，并结束移动
                currentWaypointIndex = waypoints.Length - 1;
                isPatrolComplete = true;
                isMoving = false;
                Debug.Log($"{gameObject.name} 已到达终点，停止巡逻。");
            }
        }
    }
    private void CheckGround()
    {
        // 射线起点：机器人的中心位置（稍微往上抬一点，防止从脚底射出导致漏检）
        Vector3 rayStart = transform.position + Vector3.up * 0.3f;
        
        // 向正下方发射一条长度为 (0.1 + groundCheckDistance) 的射线
        float totalDist = 0.1f + groundCheckDistance;
        
        // 执行射线检测
        isGrounded = Physics.Raycast(rayStart, Vector3.down, totalDist);

        // 如果突然踩空了
        if (!isGrounded && isMoving)
        {
            isMoving = false; // 瞬间关闭移动开关，不再执行路线
            Debug.LogWarning($"{gameObject.name} 踩空了！已断开巡逻路线，坠落中...");
            Destroy(gameObject);
        }
    }
    public void SetIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;
    }

    // ─── 辅助可视化：在编辑模式下画出巡逻路线 ───
    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            // 画出路点位置的小球
            Gizmos.DrawSphere(waypoints[i].position, 0.2f);

            // 画出路点之间的连线
            if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
            else if (loop && waypoints[0] != null)
            {
                // 如果是循环模式，最后一点和第一点连线
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
            }
        }
    }
}