using UnityEngine;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Machine/Blueprint")]
public class BlueprintData : ScriptableObject
{
    public string blueprintID;    // 图纸唯一ID（如：Item_01）
    public string itemName;       // 物体名称（如：冰盾发射器）
    public GameObject prefabToSpawn; // 点击后要在地面上生成的 Prefab
}