using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class PreviewObject : MonoBehaviour
{
    // 충돌한 오브젝트의 콜라이더 리스트
    private List<Collider> colliderList = new List<Collider>();

    [SerializeField]
    private int layerGround; // 지상 레이어
    private const int IGNORE_RAYCAST_LAYER = 2; // 충돌해도 콜라이더에 담지 않음

    [SerializeField]
    private Material greenMaterial;

    [SerializeField]
    private Material redMaterial;

    private Renderer[] renderers; // 모든 Renderer 컴포넌트를 저장

    private void Start()
    {
    }

    private void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (IsBuildable())
        {
            SetColor(greenMaterial);
        }
        else
        {
            SetColor(redMaterial);
        }
    }

    private void SetColor(Material mat)
    {
        // Get all Renderer components in this GameObject and its children
        Renderer[] renderers = this.GetComponentsInChildren<Renderer>();

        // Iterate through each renderer and update its materials
        foreach (Renderer renderer in renderers)
        {
            var newMaterials = new Material[renderer.materials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            renderer.materials = newMaterials;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Structure"))
        {
            Building building = other.GetComponent<Building>();
            if (building != null)
            {
                if (building.type == Building.Type.Foundation)
                    return;

                 colliderList.Add(other);
            }
        }
        else
        {
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            {
                colliderList.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Structure"))
        {
            Building building = other.GetComponent<Building>();
            if (building != null)
            {
                 colliderList.Remove(other);
            }
        }
        else
        {
            if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            {
                colliderList.Remove(other);
            }
        }
    }

    public bool IsBuildable()
    {
       return colliderList.Count == 0;
    }
}
