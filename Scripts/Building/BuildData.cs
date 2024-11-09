using UnityEngine;

[CreateAssetMenu(fileName = "BuildData", menuName = "Building System/Build Data")]
public class BuildData : ScriptableObject
{
    public string buildingName;
    public Sprite buildingImg;
    public string buildingDes;
    public GameObject prefab;
    public GameObject previewPrefab;
    public Building.Type buildingType;

    public ItemData itemData; // �� �ǹ��� �ش��ϴ� ItemData

}
