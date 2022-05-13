//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowLogoVideo : MonoBehaviour
{
    [SerializeField] float _SecondsToWait;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(LoadSceneInSeconds());   
    }

    IEnumerator LoadSceneInSeconds()
    {
        yield return new WaitForSeconds(_SecondsToWait);

        SceneManager.LoadScene(1);
    }
}
