using UnityEngine;
using UnityEngine.Events; // 引入事件命名空间

public class HexagonController : MonoBehaviour
{
    [System.Serializable]
    public struct HexEdge
    {
        public GameObject edgeObject;   // 边游戏物体
        public GameObject nodeA;        // 该边连接的第一个顶点
        public GameObject nodeB;        // 该边连接的第二个顶点
    }

    [Header("六边形配置")]
    [SerializeField] private HexEdge[] hexagonEdges = new HexEdge[6]; // 六边形的6条边
    [SerializeField] private GameObject[] allEdges = new GameObject[6]; // 填入所有的6个边物体，用于统一检测存活

    [SerializeField] private GroupGlowController MasterGlowController;
    [SerializeField] private GroupGlowController DoorGlowController;
    //[Header("全破发事件触发")]
    //[SerializeField] private UnityEvent onAllNodesDestroyed; // 当所有顶点都被破坏时执行的事件

    private bool hasTriggeredFinalAction = false; // 是否已经触发过最终功能，防止重复触发

    void Update()
    {
        // 1. 实时更新边的显示/隐藏
        UpdateEdgesVisibility();

        // 2. 检查是否所有顶点都已经被破坏
        if (!hasTriggeredFinalAction)
        {
            CheckAllEdgesDestroyed();
        }
    }

    private void UpdateEdgesVisibility()
    {
        for (int i = 0; i < hexagonEdges.Length; i++)
        {
            HexEdge edge = hexagonEdges[i];
            if (edge.edgeObject == null) continue;

            if (edge.nodeA != null && edge.nodeA.activeInHierarchy &&
                edge.nodeB != null && edge.nodeB.activeInHierarchy)
            {
                if (!edge.edgeObject.activeSelf) edge.edgeObject.SetActive(true);
            }
            else
            {
                if (edge.edgeObject.activeSelf) edge.edgeObject.SetActive(false);
            }
        }
    }

    private void CheckAllEdgesDestroyed()
    {
        if (allEdges == null || allEdges.Length == 0) return;

        // 假设全被破坏了
        bool allDestroyed = true;

        // 遍历所有顶点，只要有一个还在场景中激活，说明没全灭
        for (int i = 0; i < allEdges.Length; i++)
        {
            if (allEdges[i] != null && allEdges[i].activeInHierarchy)
            {
                allDestroyed = false;
                break; // 跳出循环
            }
        }

        // 如果真的全灭了
        if (allDestroyed)
        {
            hasTriggeredFinalAction = true;
            ExecuteFinalFunction();
        }
    }

    private void ExecuteFinalFunction()
    {
        Debug.Log("【核心机关解锁】六边形的所有顶点已全部被破坏！");
        MasterGlowController.SetGroupGlow(false);
        DoorGlowController.SetGroupGlow(true);
    }
}