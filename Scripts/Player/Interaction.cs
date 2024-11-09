using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInterable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float distance;
    public LayerMask layerMask;

    public GameObject curInteractObj;
    private IInterable curInteractable;
    public GameObject infoPanel;

    public TextMeshProUGUI propmtText;
    private Camera camera;
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.deltaTime;
            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                curInteractObj = hit.collider.gameObject;
                curInteractable = hit.collider.GetComponent<IInterable>();  
                SetPromptText();
            }
            else
            {
                curInteractObj = null; 
                curInteractable = null;
                propmtText.gameObject.SetActive(false);
                infoPanel.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        propmtText.gameObject.SetActive (true);
        infoPanel.gameObject.SetActive (true);
        propmtText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.phase == InputActionPhase.Started && curInteractable != null) 
        {
            curInteractable.OnInteract();
            curInteractObj = null;
            curInteractable = null;
            propmtText.gameObject.SetActive (true);
            infoPanel.gameObject.SetActive (true);
        }
    }
}
