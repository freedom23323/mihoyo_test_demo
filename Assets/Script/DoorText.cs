using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // 在这里拖入你之前建好的 World Space Canvas 或者 TextMeshPro 物体
    public GameObject interactionUI;

    void Start()
    {
        // 游戏刚开始时，默认隐藏交互提示
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    // 当有物体进入感应范围时
    void OnTriggerEnter(Collider other)
    {
        // 检查进来的是不是玩家（确保你的玩家物体 Tag 设置为了 "Player"）
        if (other.CompareTag("Player"))
        {
            if (interactionUI != null) interactionUI.SetActive(true);
        }
    }

    // 当物体离开感应范围时
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionUI != null) interactionUI.SetActive(false);
        }
    }
}