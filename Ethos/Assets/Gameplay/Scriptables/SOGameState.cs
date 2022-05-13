//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Game")]
public class SOGameState : ScriptableObject
{
    public bool MainMenu;
    public bool DebugState;
    public bool GamePaused;
}
