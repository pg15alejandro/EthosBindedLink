//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DebugController : MonoBehaviour
{
    [SerializeField] private EventSystem _EventSytem;
    [SerializeField] private SOResources _Resources;
    [SerializeField] private SOGameState _GameState;
    [SerializeField] private GameObject _DebugMenu;
    [SerializeField] private GameObject _DebugFirstOpt;

    [SerializeField] private GameObject _SpawnEnemy;
    [SerializeField] private GameObject _EnemyFirstOpt;

    [SerializeField] private GameObject _SpawnDummy;
    [SerializeField] private GameObject _DummyFirstOpt;

    [SerializeField] private GameObject _PlayerResources;
    [SerializeField] private GameObject _TeleportTo;
    [SerializeField] private GameObject _DebugMenuFirstOpt;
    [SerializeField] private TMP_InputField _EssenceInput;
    [SerializeField] private TMP_InputField _HealthInput;
    [SerializeField] private TMP_InputField _StaminaInput;

    private List<GameObject> _List = new List<GameObject>();
    private bool _DebugShowed = false;

    private void OnEnable()
    {
        // _EssenceInput.text = _Resources.CurrentEssence.ToString();
        // _HealthInput.text = _Resources.CurrentHealth.ToString();
        // _StaminaInput.text = _Resources.CurrentStamina.ToString();
    }

    private void Update()
    {
        // _EssenceInput.text = _Resources.CurrentEssence.ToString();
        // _HealthInput.text = _Resources.CurrentHealth.ToString();
        // _StaminaInput.text = _Resources.CurrentStamina.ToString();

        if (Input.GetButtonDown("Debug") && _GameState.DebugState)
        {
            if (_DebugShowed)
            {
                _DebugMenu.SetActive(false);
                Time.timeScale = 1;
                foreach (var item in _List)
                {
                    if (item.activeInHierarchy)
                    {
                        item.SetActive(false);
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                    }
                }
            }
            else
            {
                _DebugMenu.SetActive(true);
                _EventSytem.SetSelectedGameObject(_DebugMenuFirstOpt);
                Time.timeScale = 0;

            }
            _DebugShowed = !_DebugShowed;
        }
    }

    private void Start()
    {
        _List.Add(_SpawnEnemy);
        _List.Add(_SpawnDummy);
        _List.Add(_PlayerResources);
        //_List.Add(_FreeCamera);
        _List.Add(_TeleportTo);
    }

    public void SpawnEnemy()
    {
        _SpawnEnemy.SetActive(true);
        _EventSytem.SetSelectedGameObject(_EnemyFirstOpt);
        _DebugMenu.SetActive(false);
    }

    public void SpawnDummy()
    {
        _SpawnDummy.SetActive(true);
        _EventSytem.SetSelectedGameObject(_DummyFirstOpt);
        _DebugMenu.SetActive(false);
    }

    public void KillEnemies()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in go)
        {
            Destroy(item.gameObject);
        }
    }

    public void PlayerResources()
    {
        _PlayerResources.SetActive(true);
        _DebugMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void FreeCamera()
    {
        _DebugMenu.SetActive(false);
        //_FreeCamera.SetActive(true);
    }

    public void TeleportTo()
    {
        _TeleportTo.SetActive(true);
        _DebugMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Back()
    {
        foreach (var item in _List)
        {
            if (item.activeInHierarchy)
            {
                item.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        _DebugMenu.SetActive(true);
        _EventSytem.SetSelectedGameObject(_DebugMenuFirstOpt);
    }

    public void PlayerResourcesv(string type)
    {
        switch (type)
        {
            case "Essence":
                _Resources.CurrentEssence = int.Parse(_EssenceInput.text);
                break;

            case "Health":
              _Resources.CurrentHealth = int.Parse(_HealthInput.text);
                break;

            case "Stamina":
                _Resources.CurrentStamina = int.Parse(_StaminaInput.text);
                break;
        }
    }
}
