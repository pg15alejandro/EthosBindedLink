//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using UnityEngine;

public class InputSwitchCombo : MonoBehaviour
{
    public bool IsInputReading;
    public bool CanReadInput;
    public bool CanRotate;
    public bool DetectingCollisions;
    
    [SerializeField] private MeshRenderer _Sword;
    [SerializeField] private GameObject _StealthSword;

    public void Disable()
    {
        IsInputReading = false;
        CanReadInput = true;
    }

    public void Enable()
    {
        IsInputReading = true;
        CanReadInput = true;
    }

    public void EnableRotation()
    {
        print("??Enabling rotation");
        CanRotate = true;
    }

    public void DisableRotation()
    {
        print("??Disabling rotation");
        CanRotate = false;
    }

    private void EnableCollision()
    {
        DetectingCollisions = true;
    }

    private void DisableCollision()
    {
        DetectingCollisions = false;
    }

    private void EnableHiddenSword()
    {
        _Sword.enabled = false;
        _StealthSword.SetActive(true);
    }

    private void DisableHiddenSword()
    {
        _Sword.enabled = true;
        _StealthSword.SetActive(false);
    }
}