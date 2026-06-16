using System.Collections.Generic;
using UnityEngine;

// 统一管理所有游戏内的物品和蓝图ID
public enum ItemID
{
    DoorKey1,       // 门钥匙1
    DoorKey2,       // 门钥匙2
    DoorKey3,       // 门钥匙3
    ice             // 冰块
}
public class PlayerInventory : MonoBehaviour
{
    [Header("已解锁的蓝图列表")]
    // 换成 ItemID 后，在面板上会直接显示为可勾选的下拉菜单列表
    public List<ItemID> unlockedBlueprintIDs = new List<ItemID>();

    // 哈希表的键（Key）也同步改为 ItemID 枚举
    private Dictionary<ItemID, int> itemInventory = new Dictionary<ItemID, int>();

    [Header("调试用：在面板查看背包（仅供预览）")]
    [SerializeField] private List<InventoryItemDebug> inventoryDebugList = new List<InventoryItemDebug>();

    /// <summary>
    /// 解锁蓝图
    /// </summary>
    public void UnlockBlueprint(ItemID id)
    {

        if (!unlockedBlueprintIDs.Contains(id))
        {
            unlockedBlueprintIDs.Add(id);
            Debug.Log($"成功解锁蓝图: {id}");
        }
    }

    /// <summary>
    /// 获得物品
    /// </summary>
    public void AddItem(ItemID id, int amount = 1)
    {
        if (amount <= 0) return;

        if (itemInventory.ContainsKey(id))
        {
            itemInventory[id] += amount;
        }
        else
        {
            itemInventory.Add(id, amount);
        }

        Debug.Log($"获得了物品 [{id}] x{amount}，当前总数: {itemInventory[id]}");

        // 联动：获得物品时自动解锁对应蓝图
        UnlockBlueprint(id);

        UpdateDebugList(); 
    }

    /// <summary>
    /// 消耗物品
    /// </summary>
    public bool RemoveItem(ItemID id, int amount = 1)
    {

        if (!itemInventory.ContainsKey(id) || itemInventory[id] < amount)
        {
            Debug.LogWarning($"无法消耗物品 [{id}]: 数量不足或不存在该物品！");
            return false;
        }

        itemInventory[id] -= amount;
        Debug.Log($"消耗了物品 [{id}] x{amount}，剩余: {itemInventory[id]}");

        UpdateDebugList(); 
        return true;
    }

    /// <summary>
    /// 查询物品数量
    /// </summary>
    public int GetItemCount(ItemID id)
    {
        if (itemInventory.ContainsKey(id))
        {
            return itemInventory[id];
        }
        return 0;
    }

    // ─────── Unity Inspector 可视化结构 ───────

    [System.Serializable]
    public struct InventoryItemDebug
    {
        public ItemID itemID; // 调试列表里也会变成精美的下拉菜单
        public int count;
    }

    private void UpdateDebugList()
    {
        inventoryDebugList.Clear();
        foreach (var kvp in itemInventory)
        {
            inventoryDebugList.Add(new InventoryItemDebug { itemID = kvp.Key, count = kvp.Value });
        }
    }
}