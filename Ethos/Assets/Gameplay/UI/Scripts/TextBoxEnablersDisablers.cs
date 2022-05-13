//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;

public class TextBoxEnablersDisablers : MonoBehaviour
{
    [SerializeField] private float _MaxTimerFirstBox;
    [SerializeField] private GameObject _FirstCheckBox;
    [SerializeField] private GameObject _PossesionBox;
    [SerializeField] private GameObject _CombatSystemBox;
    [SerializeField] private GameObject _AttackingBox;
    [SerializeField] private GameObject _BlockBox;
    [SerializeField] private GameplayLogic _GameLogic;
    [Header("Debug")]
    [SerializeField] private float _TimerFirstBox;
    private bool _AlreadyActive = false;
    private bool _AlreadyActive2 = false;
    private bool _UIDisplayed = false;
    [SerializeField] private SOGameState _GameState;

    // Update is called once per frame

    private void Start()
    {
        _TimerFirstBox = _MaxTimerFirstBox;
    }

    void Update()
    {
        if(_GameLogic.ZoneIndex != 0) return;

        if (_TimerFirstBox <= 0 && !_AlreadyActive)
        {
            _FirstCheckBox.SetActive(true);
            _AlreadyActive = true;
            _AlreadyActive2 = true;
            Time.timeScale = 0;
            _GameState.GamePaused = true;
        }
        else
        {
            _TimerFirstBox -= Time.deltaTime;
        }

        ReadInput();
    }

    private void ReadInput()
    {
        if (Input.GetButtonDown("AButton") && _AlreadyActive2)
        {
            _AlreadyActive2 = false;
            Time.timeScale = 1f;
            _GameState.GamePaused = false;
            _FirstCheckBox.SetActive(false);
        }

        if (Input.GetButtonDown("AButton") && _UIDisplayed)
        {
            Time.timeScale = 1f;
            _PossesionBox.SetActive(false);
            _GameState.GamePaused = false;
            _UIDisplayed = false;

            _AttackingBox.SetActive(false);
            if (_CombatSystemBox.activeInHierarchy)
            {
                _CombatSystemBox.SetActive(false);
                AfterPickUp();
            }
            _BlockBox.SetActive(false);

        }
    }

    public void AfterPossesionBox()
    {
        if(_GameLogic.ZoneIndex != 0) return;

        _PossesionBox.SetActive(true);
        _UIDisplayed = true;
        Time.timeScale = 0;
        _GameState.GamePaused = true;
    }


    public void PickUpSwordBox()
    {
        if(_GameLogic.ZoneIndex != 0) return;

        _CombatSystemBox.SetActive(true);
        _UIDisplayed = true;
        Time.timeScale = 0;
        _GameState.GamePaused = true;
    }


    public void AfterPickUp()
    {
        if(_GameLogic.ZoneIndex != 0) return;

        _AttackingBox.SetActive(true);
        _UIDisplayed = true;
        Time.timeScale = 0;
        _GameState.GamePaused = true;
    }


    public void Block()
    {
        if(_GameLogic.ZoneIndex != 0) return;

        _BlockBox.SetActive(true);
        _UIDisplayed = true;
        Time.timeScale = 0;
        _GameState.GamePaused = true;
    }
}
