using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<string> unlockedBlueprintIDs = new List<string>();

    public void UnlockBlueprint(string id)
    {
        // List 的去重需要手动判断一下
        if (!unlockedBlueprintIDs.Contains(id))
        {
            unlockedBlueprintIDs.Add(id);
        }
    }
}