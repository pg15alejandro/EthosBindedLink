//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DebugSwitch : MonoBehaviour
{
    [SerializeField] private EventSystem _EventSystem;
    [SerializeField] private GameObject _OnButton;
    [SerializeField] private GameObject _OffButton;
    [SerializeField] private SOGameState _GameState;


    public void OnBtn()
    {
        _EventSystem.SetSelectedGameObject(_OffButton);
        _GameState.DebugState = true;
        _OnButton.GetComponent<Button>().interactable = false;
        _OffButton.GetComponent<Button>().interactable = true;
    }

    public void OffBtn()
    {
        _EventSystem.SetSelectedGameObject(_OnButton);
        _GameState.DebugState = false;
        _OnButton.GetComponent<Button>().interactable = true;
        _OffButton.GetComponent<Button>().interactable = false;
    }
}
