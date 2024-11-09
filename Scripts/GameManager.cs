using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public bool isPause = false;
    public SaveManager saveManager;
    public LoadManager loadManager;
    public EquipmentUI equipUI;
    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        saveManager = GetComponent<SaveManager>();
        loadManager = GetComponent<LoadManager>();  
    }

}

