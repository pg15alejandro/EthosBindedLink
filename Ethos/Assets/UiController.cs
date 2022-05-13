//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [Header("Scriptable")]
    [SerializeField] private SOGameState _GameState;
    [SerializeField] private SOTransform _PlayerValue;

    [Header("Event System")]
    [SerializeField] private EventSystem _EventSystem;

    [Header("Game UI")]
    [SerializeField] private GameObject _GameUI;

    [Header("Main Menu")]
    [SerializeField] private GameObject _MainMenu;
    [SerializeField] private GameObject _MainFirstOpt;

    [Header("Pause Menu")]
    [SerializeField] private GameObject _PauseMenu;
    [SerializeField] private GameObject _PauseFirstOpt;

    [Header("Option Menu")]
    [SerializeField] private GameObject _OptionsMenu;
    [SerializeField] private GameObject _OptionsFirstOpt;

    [Header("Audio Menu")]
    [SerializeField] private GameObject _AudioMenu;
    [SerializeField] private GameObject _AudioFirstOpt;

    [Header("Video Menu")]
    [SerializeField] private GameObject _VideoMenu;
    [SerializeField] private GameObject _VideoFirstOpt;

    [Header("Debug Menu")]
    [SerializeField] private GameObject _DebugMenu;
    [SerializeField] private GameObject _DebugFirstOpt;

    [Header("Scene Names")]
    [SerializeField] private string _MainMenuScene;
    [SerializeField] private string _FirstGameScene;
    private bool _GamePaused;
    private List<GameObject> _List = new List<GameObject>();

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Play()
    {
        _GameState.MainMenu = false;
        //_PlayerValue.checkpoint = _PlayerValue.startPos;
        _PlayerValue.UseCheckpoint = false;
        _PlayerValue.ZoneLoad = 0;
        SceneManager.LoadScene(_FirstGameScene);
    }

    public void PauseMainMenu()
    {
        _GameState.MainMenu = true;
        _GamePaused = false;
        _GameState.GamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(_MainMenuScene);
    }

    public void LoadScene(string GameSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameSceneName);
    }

    public void Resume()
    {
        _GamePaused = false;
        _GameState.GamePaused = false;
        Time.timeScale = 1f;

        _List.Add(_VideoMenu);
        _List.Add(_AudioMenu);
        _List.Add(_DebugMenu);
        _List.Add(_OptionsMenu);
        
        foreach (var item in _List)
        {
            if (item.activeInHierarchy)
            {
                item.SetActive(false);
            }
        }
        _GameUI.SetActive(true);
        _PauseMenu.SetActive(false);
    }

    public void Lose(string sceneName)
    {
        Time.timeScale = 1f;
        _GameState.GamePaused = false;
        _GamePaused = false;

        SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (_GamePaused)
            {
                Resume();
                AkSoundEngine.SetState("game_state", "in_game");
            }
            else
            {
                Pause();
                AkSoundEngine.SetState("game_state", "paused");
            }
        }
    }

    private void Pause()
    {        
        _GamePaused = true;
        _GameState.GamePaused = true;
        Time.timeScale = 0;
        _GameUI.SetActive(false);
        _PauseMenu.SetActive(true);
        _GameState.MainMenu = false;
        _EventSystem.SetSelectedGameObject(_PauseFirstOpt);
    }

    public void Options()
    {
        if (_GameState.MainMenu)
            _MainMenu.SetActive(false);
        else
            _PauseMenu.SetActive(false);

        _OptionsMenu.SetActive(true);

        _EventSystem.SetSelectedGameObject(_OptionsFirstOpt);
    }

    public void OptionsBack()
    {
        _OptionsMenu.SetActive(false);

        if (_GameState.MainMenu)
        {
            _MainMenu.SetActive(true);
            _EventSystem.SetSelectedGameObject(_MainFirstOpt);
        }
        else
        {
            _PauseMenu.SetActive(true);
            _EventSystem.SetSelectedGameObject(_PauseFirstOpt);
        }

    }

    public void Credits(string CreditsSceneName)
    {
        SceneManager.LoadScene(CreditsSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Audio()
    {
        _OptionsMenu.SetActive(false);
        _AudioMenu.SetActive(true);

        _EventSystem.SetSelectedGameObject(_AudioFirstOpt);
    }

    public void AudioBack()
    {
        _OptionsMenu.SetActive(true);
        _AudioMenu.SetActive(false);

        _EventSystem.SetSelectedGameObject(_OptionsFirstOpt);
    }

    public void Video()
    {
        _OptionsMenu.SetActive(false);
        _VideoMenu.SetActive(true);

        _EventSystem.SetSelectedGameObject(_VideoFirstOpt);
    }

    public void VideoBack()
    {
        _OptionsMenu.SetActive(true);
        _VideoMenu.SetActive(false);

        _EventSystem.SetSelectedGameObject(_OptionsFirstOpt);
    }

    public void Debug()
    {
        _OptionsMenu.SetActive(false);
        _DebugMenu.SetActive(true);

        _EventSystem.SetSelectedGameObject(_DebugFirstOpt);
    }

    public void DebugBack()
    {
        _OptionsMenu.SetActive(true);
        _DebugMenu.SetActive(false);

        _EventSystem.SetSelectedGameObject(_OptionsFirstOpt);
    }

}

