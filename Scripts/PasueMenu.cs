using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 관련 네임스페이스 추가
using UnityEngine.InputSystem; // Input System 네임스페이스 추가

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject BaseUI;

    [Header("BGM Selection Buttons")]
    public Button Button_BGM1;
    public Button Button_BGM2;
    public Button Button_BGM3;

    [Header("Volume Sliders")]
    public Slider Slider_BGMVolume;
    public Slider Slider_SFXVolume;
    public Slider Slider_MasterVolume;

    [Header("Other Buttons")]
    public Button SaveButton;
    public Button LoadButton;
    public Button ExitButton;

    private PlayerInput playerInput;
    private InputAction pauseAction;

    private PlayerController playerController; 

    void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();

        pauseAction = playerInput.actions["Pause"];

        pauseAction.performed += OnPauseInput;

        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        Button_BGM1.onClick.AddListener(() => OnBGMButtonClicked(0));
        Button_BGM2.onClick.AddListener(() => OnBGMButtonClicked(1));
        Button_BGM3.onClick.AddListener(() => OnBGMButtonClicked(2));

        InitializeVolumeSliders();

        SaveButton.onClick.AddListener(ClickSave);
        LoadButton.onClick.AddListener(ClickLoad);
        ExitButton.onClick.AddListener(ClickExit);

        BaseUI.SetActive(false);
    }

    private void OnDestroy()
    {
        pauseAction.performed -= OnPauseInput;
    }

    public void OnPauseInput(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.isPause)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
        }
    }

    private void OpenMenu()
    {
        GameManager.Instance.isPause = true;
        BaseUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.enabled = false;
    }

    private void CloseMenu()
    {
        GameManager.Instance.isPause = false;
        BaseUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.enabled = true;
    }

    public void ClickSave()
    {
        Debug.Log("Save");
        GameManager.Instance.saveManager.SaveGame();

        CloseMenu();
    }

    public void ClickLoad()
    {
        Debug.Log("Load");
        GameManager.Instance.loadManager.LoadGame();

        CloseMenu();
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void OnBGMButtonClicked(int index)
    {
        AudioManager.Instance.PlayBGM(index);
    }

    private void InitializeVolumeSliders()
    {
        // BGM 볼륨 슬라이더 초기화
        Slider_BGMVolume.minValue = 0f;
        Slider_BGMVolume.maxValue = 1f;
        Slider_BGMVolume.value = AudioManager.Instance.GetBGMVolume();
        Slider_BGMVolume.onValueChanged.AddListener(OnBGMVolumeChanged);

        Slider_SFXVolume.minValue = 0f;
        Slider_SFXVolume.maxValue = 1f;
        Slider_SFXVolume.value = AudioManager.Instance.GetSFXVolume();
        Slider_SFXVolume.onValueChanged.AddListener(OnSFXVolumeChanged);

        Slider_MasterVolume.minValue = 0f;
        Slider_MasterVolume.maxValue = 1f;
        Slider_MasterVolume.value = AudioManager.Instance.GetMasterVolume();
        Slider_MasterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
    }

    public void OnBGMVolumeChanged(float volume)
    {
        AudioManager.Instance.SetBGMVolume(volume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    public void OnMasterVolumeChanged(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
    }
}
