using UnityEngine;
using System.Collections;

public class ObjectRotator : MonoBehaviour
{
    [Header("旋转设置")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // 默认沿本地Y轴(0,1,0)旋转
    [SerializeField] private float rotateAngle = -90f;          // 逆时针旋转90度（Unity中顺时针为正，逆时针为负）
    [SerializeField] private float duration = 0.5f;             // 旋转动画持续时间（秒）

    private bool isPlayerInZone = false; // 玩家是否在交互范围内
    private bool isRotating = false;     // 物体是否正在旋转中（防止重复触发乱套）
    private Quaternion targetRotation;   // 目标旋转角度

    void Start()
    {
        // 初始化目标旋转为当前旋转
        targetRotation = transform.localRotation;
    }

    void Update()
    {
        // 玩家在范围内、按下F键、且当前没有在播放旋转动画
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.F) && !isRotating)
        {
            // 计算新的目标本地旋转：在当前目标基础上，沿本地轴旋转 -90 度
            targetRotation = targetRotation * Quaternion.AngleAxis(rotateAngle, rotationAxis);
            
            // 开启协程，平滑旋转到目标角度
            StartCoroutine(AnimateRotation());
        }
    }

    // 平滑旋转的协程动画
    private IEnumerator AnimateRotation()
    {
        isRotating = true;
        float elapsed = 0f;
        Quaternion startRotation = transform.localRotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // 使用平滑插值（平滑起步和收尾）
            t = Mathf.SmoothStep(0f, 1f, t); 
            
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        // 确保精准达到目标角度
        transform.localRotation = targetRotation;
        isRotating = false;
    }

    // --- 玩家范围检测 ---

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            Debug.Log("靠近可旋转物体，按 F 键触发旋转");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            Debug.Log("离开物体范围");
        }
    }
}