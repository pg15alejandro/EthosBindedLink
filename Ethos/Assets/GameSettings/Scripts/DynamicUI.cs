//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicUI : MonoBehaviour
{
    [SerializeField] private GameObject _PointsList;
    [SerializeField] private GameObject _DebugMenu;
    [SerializeField] private float _Left;
    [SerializeField] private float _Top;
    [SerializeField] private GameObject _Player;
    [SerializeField] private EventSystem _EventSystem;
    [SerializeField] private GameObject _DebugFirstOpt;
    private int _AmmountOfPoints;

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable()
    {
        Time.timeScale = 1f;
        _AmmountOfPoints = _PointsList.transform.childCount;
    }

    
    /// <summary>
    /// Called for rendering and handling events
    /// </summary>
    private void OnGUI()
    {
        GUILayout.Space(_Top);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(_Left);
            GUILayout.BeginVertical();
            {
                for (int i = 0; i < _AmmountOfPoints; i++)
                {
                    if (GUILayout.Button(_PointsList.transform.GetChild(i).name))
                    {
                        _Player.transform.position = GameObject.Find(_PointsList.transform.GetChild(i).name).transform.position;
                        Debug.Log($"Pressed button {i}");
                    }
                }

                if (GUILayout.Button("Back"))
                {
                    gameObject.SetActive(false);
                    _DebugMenu.SetActive(true);
                    _EventSystem.SetSelectedGameObject(_DebugFirstOpt);
                    Time.timeScale = 0f;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }
}
