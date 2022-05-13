using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EthosSense : MonoBehaviour
{

    [SerializeField] private KeyCode _key;

    [SerializeField] private Material _material;
    [SerializeField] private Shader _shad1;

    private GameObject _cam;
    [SerializeField] private Shader _shad2;
    private bool high = false;
    [SerializeField] private List<GameObject> _path;


    /// <summary>
    /// Unity's Start method. Called before the first frame
    /// </summary>
    void Start()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera");
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            high = !high;
        }

        if (high)
        {
            foreach (var item in _path)
            {
                item.GetComponent<MeshRenderer>().enabled = true;
            }
            _cam.GetComponent<Volume>().enabled = true;
            //_material.color = Color.black;
            _material.shader = _shad1;
        }
        else
        {
            foreach (var item in _path)
            {
                item.GetComponent<MeshRenderer>().enabled = false;
            }
            //_material.color = Color.magenta;
            _cam.GetComponent<Volume>().enabled = false;
            //_material.color = Color.black;
            _material.shader = _shad2;
        }
    }


}
