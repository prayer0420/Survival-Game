using UnityEngine;

public class Building : MonoBehaviour
{
    public enum Type
    {
        Normal,
        Wall,
        Foundation, // Pillar�� Foundation �������� �����ϵ���
        Pillar
    }

    public Type type;

    public BuildData buildData;
}
