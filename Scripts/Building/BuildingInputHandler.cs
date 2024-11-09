using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingInputHandler : MonoBehaviour
{
    private BuildingManager buildingManager;

    private void Awake()
    {
        buildingManager = GetComponent<BuildingManager>();
    }

    public void OnStartBuilding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buildingManager.StartBuilding();
        }
    }

    public void OnPlaceBuilding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buildingManager.PlaceBuilding();
        }
    }

    public void OnCancelBuilding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buildingManager.CancelBuildingPlacement();
        }
    }

    public void OnModifyBuilding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buildingManager.ModifyBuilding();
        }
    }

    public void OnApplyModification(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buildingManager.ApplyModification();
        }
    }

    public void OnCancelModification(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buildingManager.CancelModification();
        }
    }

    public void OnAdjustYOffset(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float value = context.ReadValue<float>();
            buildingManager.AdjustYOffset(value);
        }
    }

    public void OnRotatePreview(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float direction = context.ReadValue<float>();
            buildingManager.RotatePreview(direction);
        }
    }

    public void OnDestroyBuilding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buildingManager.DestroySelectedBuilding();
        }
    }
}
