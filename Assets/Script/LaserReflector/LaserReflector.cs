using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflector : MonoBehaviour
{
    public int maxReflections = 3;      // 最大反射次数，防止死循环
    public float maxLaserDistance = 50f; // 激光最大射程
    private LineRenderer lineRenderer;   // 用于渲染激光视觉效果

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        DrawLaserPath();
    }

    void DrawLaserPath()
    {
        // 记录激光的所有拐点（起点 + 反射点 + 终点）
        System.Collections.Generic.List<Vector3> laserPoints = new System.Collections.Generic.List<Vector3>();
        
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        laserPoints.Add(origin);

        for (int i = 0; i < maxReflections; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, maxLaserDistance))
            {
                laserPoints.Add(hit.point); // 记录碰撞点

                // 情况 A：射中了冰盾，继续反射
                if (hit.collider.CompareTag("IceShield"))
                {
                    // 计算反射向量：入射方向 + 碰撞面法线
                    direction = Vector3.Reflect(direction, hit.normal);
                    origin = hit.point + direction * 0.01f; // 稍微偏移，防止穿模自带碰撞
                }
                // 情况 B：射中了可破坏障碍物
                else if (hit.collider.CompareTag("Destructible"))
                {
                    IDestructible obstacle = hit.collider.GetComponent<IDestructible>();
                    if (obstacle != null)
                    {
                        obstacle.OnLaserHit(hit.point); // 触发破坏接口
                    }
                    break; // 光线被障碍物阻挡，终止
                }
                else if (hit.collider.CompareTag("RayCharger"))
                {
                    IHittable obstacle = hit.collider.GetComponent<IHittable>();
                    if (obstacle != null)
                    {
                        obstacle.OnLaserHit(hit.point); // 触发激活接口
                    }
                    break; // 光线被障碍物阻挡，终止
                }
                else
                {
                    // 射中普通墙壁，终止
                    break;
                }
            }
            else
            {
                // 谁也没射中，延伸到最远距离
                laserPoints.Add(origin + direction * maxLaserDistance);
                break;
            }
        }

        // 更新 LineRenderer 的表现
        lineRenderer.positionCount = laserPoints.Count;
        lineRenderer.SetPositions(laserPoints.ToArray());
    }
}