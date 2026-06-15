using UnityEngine;

public class RoadController : MonoBehaviour
{
    [Header("道路物体")]
    [SerializeField] private GameObject roadObject; // 需要显示/隐藏的道路模型

    [Header("初始状态")]
    [SerializeField] private bool isRoadActive = false; // 默认道路是关闭的

    void Start()
    {
        // 根据初始设置初始化道路状态
        if (roadObject != null)
        {
            roadObject.SetActive(isRoadActive);
        }
    }

    // 供机关调用的核心方法：切换道路状态
    public void ToggleRoad()
    {
        if (roadObject == null) return;

        // 取反当前状态
        isRoadActive = !isRoadActive;
        roadObject.SetActive(isRoadActive);

        // 可以在这里添加一些音效或粒子效果
        if (isRoadActive)
        {
            Debug.Log("道路已开启！");
        }
        else
        {
            Debug.Log("道路已关闭！");
        }
    }
}