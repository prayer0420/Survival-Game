using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float range;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask buildingInteractionLayerMask;
    [SerializeField] private int yOffsetSpeed;
    [SerializeField] private int rotationSpeed;

    private Inventory playerInventory;
    private GameObject previewObject;
    private BuildData currentBuildData;
    private GameObject selectedBuilding;
    private GameObject modificationPreview;

    private RaycastHit hitInfo;
    private float yOffset = 0f;

    private bool isPreviewActive = false;
    private bool isModifyMode = false;

    private BuildingUIManager uiManager;

    public List<BuildData> allBuildData = new List<BuildData>(); // 모든 BuildData를 저장하는 리스트

    private void Start()
    {
        playerInventory = FindObjectOfType<Inventory>();
        uiManager = GetComponent<BuildingUIManager>();
        allBuildData.Clear();

        // UIManager로부터 모든 BuildData 수집
        if (uiManager != null)
        {
            allBuildData.AddRange(uiManager.fireBuildingList);
            allBuildData.AddRange(uiManager.constructionBuildingList);
            allBuildData.AddRange(uiManager.customBuildingList);
        }
        else
        {
            Debug.LogError("BuildingUIManager가 설정되어 있지 않습니다.");
        }
    }

    public BuildData GetBuildDataByItemId(int itemId)
    {
        foreach (BuildData buildData in allBuildData)
        {
            if (buildData.itemData != null && buildData.itemData.itemId == itemId)
            {
                return buildData;
            }
        }
        return null;
    }

    private void Update()
    {
        HandleBuildingInteraction();

        if (isPreviewActive)
        {
            UpdatePreviewPosition(previewObject);
        }
        else if (isModifyMode && modificationPreview != null)
        {
            UpdatePreviewPosition(modificationPreview);
        }
    }

    #region Building Placement

    public void StartBuilding()
    {
        if (!isPreviewActive && !isModifyMode)
        {
            uiManager.TogglePanel();
        }
    }

    public void PlaceBuilding()
    {
        if (isPreviewActive && previewObject.GetComponent<PreviewObject>().IsBuildable())
        {
            ItemData buildingItem = currentBuildData.itemData;
            playerInventory.RemoveItem(buildingItem);

            GameObject newBuilding = Instantiate(currentBuildData.prefab, previewObject.transform.position, previewObject.transform.rotation);
            Building buildingComponent = newBuilding.GetComponent<Building>();
            if (buildingComponent != null)
            {
                buildingComponent.buildData = currentBuildData;
            }

            Destroy(previewObject);
            isPreviewActive = false;
            currentBuildData = null;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buildingClip);

        }
    }

    public void CancelBuildingPlacement()
    {
        if (isPreviewActive)
        {
            Destroy(previewObject);
            isPreviewActive = false;
            currentBuildData = null;
        }
    }

    #endregion

    #region Modification

    public void ModifyBuilding()
    {
        if (selectedBuilding != null && !isModifyMode && !isPreviewActive)
        {
            StartModification(selectedBuilding);
        }
    }

    public void ApplyModification()
    {
        if (isModifyMode)
        {
            selectedBuilding.transform.position = modificationPreview.transform.position;
            selectedBuilding.transform.rotation = modificationPreview.transform.rotation;
            selectedBuilding.SetActive(true);

            Destroy(modificationPreview);
            modificationPreview = null;
            isModifyMode = false;
            selectedBuilding = null;
        }
    }

    public void CancelModification()
    {
        if (isModifyMode)
        {
            Destroy(modificationPreview);
            modificationPreview = null;
            selectedBuilding.SetActive(true);
            isModifyMode = false;
            selectedBuilding = null;
        }
    }

    public void DestroySelectedBuilding()
    {
        if (selectedBuilding != null && !isModifyMode && !isPreviewActive)
        {
            DestroyBuilding(selectedBuilding);
        }
    }

    private void StartModification(GameObject building)
    {
        isModifyMode = true;
        selectedBuilding = building;

        Building buildingComponent = selectedBuilding.GetComponent<Building>();
        if (buildingComponent != null && buildingComponent.buildData != null)
        {
            modificationPreview = Instantiate(buildingComponent.buildData.previewPrefab, selectedBuilding.transform.position, selectedBuilding.transform.rotation);
            modificationPreview.transform.localScale = selectedBuilding.transform.localScale;
            selectedBuilding.SetActive(false);
        }
    }

    #endregion

    #region Building Interaction

    private void HandleBuildingInteraction()
    {
        if (Physics.Raycast(player.position, player.forward, out hitInfo, range, buildingInteractionLayerMask))
        {
            GameObject hitObject = hitInfo.transform.gameObject;
            if (hitObject.CompareTag("Structure"))
            {
                selectedBuilding = hitObject;
                uiManager.UpdateInteractionText($"{hitObject.GetComponent<Building>().buildData.buildingName} - 수정(M) or 파괴(C)");
            }
        }
        else
        {
            uiManager.UpdateInteractionText("");
        }
    }

    private void DestroyBuilding(GameObject building)
    {
        BuildingInteraction interaction = building.GetComponent<BuildingInteraction>();
        if (interaction != null)
        {
            interaction.DestructBuilding();
        }
        selectedBuilding = null;
    }

    #endregion

    #region Preview Update

    private void UpdatePreviewPosition(GameObject obj)
    {
        if (Physics.Raycast(player.position, player.forward, out hitInfo, range, layerMask))
        {
            Vector3 position = hitInfo.point;
            position.y += yOffset;
            position = RoundPosition(position);
            obj.transform.position = position;
        }
    }

    private Vector3 RoundPosition(Vector3 position)
    {
        position.x = Mathf.Round(position.x * 2f) * 0.5f;
        position.y = Mathf.Round(position.y * 10f) * 0.1f;
        position.z = Mathf.Round(position.z * 2f) * 0.5f;
        return position;
    }

    public void AdjustYOffset(float value)
    {
        yOffset += value * Time.deltaTime * yOffsetSpeed;
    }

    public void RotatePreview(float direction)
    {
        float rotationAmount = 90f * direction;
        if (isPreviewActive && previewObject != null)
        {
            previewObject.transform.Rotate(0, rotationAmount, 0);
        }
        else if (isModifyMode && modificationPreview != null)
        {
            modificationPreview.transform.Rotate(0, rotationAmount, 0);
        }
    }

    public void SetCurrentBuildData(BuildData buildData)
    {
        currentBuildData = buildData;
        ItemData buildingItem = currentBuildData.itemData;
        if (buildingItem == null || playerInventory.GetItemCount(buildingItem) <= 0)
        {
            Debug.Log("Not enough items.");
            return;
        }

        previewObject = Instantiate(currentBuildData.previewPrefab, player.position + player.forward * 5f, Quaternion.identity);
        isPreviewActive = true;
    }

    #endregion
}
