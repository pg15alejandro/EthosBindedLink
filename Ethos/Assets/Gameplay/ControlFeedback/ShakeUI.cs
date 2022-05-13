//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] _ArrayOfTransform;
    private Quaternion[] _ArrayOfRot;

    public IEnumerator FShakeUI(float rotationRange, float timer)
    {
        float elapsed = 0.0f;
        _ArrayOfRot = new Quaternion[_ArrayOfTransform.Length];
        var lenght = _ArrayOfTransform.Length;
        print($"Lenght {lenght}");
        for (int i = 0; i < _ArrayOfTransform.Length; i++)
        {
            _ArrayOfRot[i] = _ArrayOfTransform[i].transform.rotation;
        }

        while (elapsed < timer)
        {
            for (int i = 0; i < _ArrayOfTransform.Length; i++)
            {

                elapsed += Time.deltaTime;
                float z = Random.value * rotationRange - (rotationRange / 2);
                _ArrayOfTransform[i].transform.eulerAngles = new Vector3(0, 0, _ArrayOfRot[i].z + z);
                yield return null;
                _ArrayOfTransform[i].transform.rotation = _ArrayOfRot[i];
            }

        }
    }
}
