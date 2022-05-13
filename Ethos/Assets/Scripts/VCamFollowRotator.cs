//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS
using UnityEngine;

public class VCamFollowRotator : MonoBehaviour
{

    [SerializeField]
    private float _Delay = 1f;

    [SerializeField]
    private float _Duration = 3f;

    private float _LastMoved = 0f;
    private float _Time = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _LastMoved = _Delay;
            _Time = 0f;
        }

        if (_LastMoved >= 0)
        {
            _LastMoved -= Time.deltaTime;
        }
        else
        {
            _Time += Time.deltaTime / _Duration;
        }

        if(_Time <= 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, _Time);
        }
    }

}
