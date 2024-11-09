using UnityEngine;

public class BuildingInteraction : MonoBehaviour
{
    public void DestructBuilding()
    {
        // TODO: 자원 반환 로직

        // 건물 제거
        Destroy(gameObject);
    }
}
