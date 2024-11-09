using UnityEngine;

public class Building : MonoBehaviour
{
    public enum Type
    {
        Normal,
        Wall,
        Foundation, // Pillar는 Foundation 위에서만 존재하도록
        Pillar
    }

    public Type type;

    public BuildData buildData;
}
