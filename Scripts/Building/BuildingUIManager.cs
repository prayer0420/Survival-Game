using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIManager : MonoBehaviour
{
    // UI-related variables
    [SerializeField] private GameObject buildingPanel;
    [SerializeField] private GameObject[] slots;
    [SerializeField] private Image[] slotImages;
    [SerializeField] private TextMeshProUGUI[] slotNames;
    [SerializeField] private TextMeshProUGUI[] slotDescriptions;
    [SerializeField] private TextMeshProUGUI interactionText;

    // Lists for building data
    [SerializeField] public List<BuildData> fireBuildingList;
    [SerializeField] public List<BuildData> constructionBuildingList;
    [SerializeField] public List<BuildData> customBuildingList;

    private int currentTab = 0;
    private int currentPage = 1;
    private List<BuildData> currentBuildingList;
    private BuildingManager buildingManager;

    private void Start()
    {
        buildingManager = GetComponent<BuildingManager>();
        SetTab(0);
    }

    public void TogglePanel()
    {
        bool isActive = !buildingPanel.activeSelf;
        buildingPanel.SetActive(isActive);
        Cursor.lockState = isActive ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void SetTab(int tabIndex)
    {
        currentTab = tabIndex;
        currentPage = 1;

        switch (currentTab)
        {
            case 0:
                currentBuildingList = fireBuildingList;
                break;
            case 1:
                currentBuildingList = constructionBuildingList;
                break;
            case 2:
                currentBuildingList = customBuildingList;
                break;
        }

        Debug.Log("setTab");
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {
        ClearSlots();

        int startSlotIndex = (currentPage - 1) * slots.Length;

        for (int i = 0; i < slots.Length; i++)
        {
            int buildingIndex = startSlotIndex + i;
            if (buildingIndex >= currentBuildingList.Count) break;

            BuildData buildData = currentBuildingList[buildingIndex];
            slots[i].SetActive(true);
            slotImages[i].sprite = buildData.buildingImg;
            slotNames[i].text = buildData.buildingName;
            slotDescriptions[i].text = buildData.buildingDes;
        }
        Debug.Log("clearslots");

    }

    private void ClearSlots()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }
    }

    public void OnSlotSelected(int slotIndex)
    {
        int selectedSlotIndex = slotIndex + (currentPage - 1) * slots.Length;
        BuildData selectedBuildData = currentBuildingList[selectedSlotIndex];
        buildingManager.SetCurrentBuildData(selectedBuildData);
        buildingPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateInteractionText(string text)
    {
        interactionText.text = text;
    }
}
