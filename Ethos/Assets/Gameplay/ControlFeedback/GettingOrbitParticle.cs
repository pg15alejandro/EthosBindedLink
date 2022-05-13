//Copyright (c) 2019, Alejandro Silva and Diego Montoya in Collaboration with VFS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingOrbitParticle : MonoBehaviour
{
    [SerializeField] private SOTransform _Player;
    public Vector3 OffsetDestination;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<ParticleSystem>().isPlaying)
        {
            gameObject.transform.position = _Player.value.position + OffsetDestination;
            gameObject.transform.rotation = new Quaternion(0, 0, _Player.value.rotation.z, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
