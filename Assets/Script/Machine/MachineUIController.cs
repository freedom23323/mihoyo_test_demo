using UnityEngine;
using UnityEngine.UI;

public class MachineUIController : MonoBehaviour
{
    public GameObject uiPanel; // UI 的父节点（用于显示/隐藏）
    
    // 拖入场景中的 3 个按钮
    public Button[] craftButtons = new Button[3]; 
    // 拖入 3 个按钮文本，用来显示物体名字
    public Text[] buttonTexts = new Text[3];     

    private CraftingMachine currentMachine;

    void Start()
    {
        uiPanel.SetActive(false); // 默认隐藏 UI
    }

    // 打开UI并刷新状态
    public void OpenUI(BlueprintData[] blueprints, PlayerInventory player, CraftingMachine machine)
    {
        currentMachine = machine;
        uiPanel.SetActive(true);

        // 遍历 3 个选项
        for (int i = 0; i < 3; i++)
        {
            if (i >= blueprints.Length || blueprints[i] == null) continue;

            BlueprintData data = blueprints[i];
            //buttonTexts[i].text = data.itemName;

            // 【核心逻辑】检查玩家的 HashSet 里有没有这张图纸的 ID
            bool isUnlocked = player.unlockedBlueprintIDs.Contains(data.blueprintID);

            // 设置按钮是否可以点击（未解锁就置灰）
            craftButtons[i].interactable = isUnlocked;

            // 清除之前的监听事件，防止重复绑定
            craftButtons[i].onClick.RemoveAllListeners();

            if (isUnlocked)
            {
                // 如果解锁了，绑定点击事件：通知机器生成对应的 Prefab，并关闭 UI
                craftButtons[i].onClick.AddListener(() => {
                    currentMachine.SpawnObject(data.prefabToSpawn);
                    CloseUI();
                });
            }
        }
    }

    public void CloseUI()
    {
        uiPanel.SetActive(false);
    }
}