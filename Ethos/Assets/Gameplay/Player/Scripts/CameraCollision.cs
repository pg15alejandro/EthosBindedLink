using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------

    [SerializeField] float _MinDistance = 1.0f;
    [SerializeField] float _MaxDistance = 4.0f;
    [SerializeField] float _Smooth = 10.0f;
    [SerializeField] private LayerMask _ObstacleMask;       //Layer of the objects to ignore

    [Range(0, 1)]
    [SerializeField] float wallDistance = .85f;

    Vector3 _DollyDir, _DollyDirAdjusted;
    float _Distance;


    /// <summary>
    /// Called when the script instance is being loaded
    /// </summary>
    private void Awake()
    {
        _DollyDir = transform.localPosition.normalized;     //Normalized position of the camera (based on the parent)
        _Distance = transform.localPosition.magnitude;      //Magnitued of the vector position of the camera (based on the parent)
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    void Update()
    {
        //Get the position where we want the camera to be
        Vector3 desiredCameraPos = transform.parent.TransformPoint(_DollyDir * _MaxDistance);
        RaycastHit hit;

        //Check if there is any object between the camera and the player
        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit, _ObstacleMask))
        {
            //Set the camera's distance between the max and min value
            _Distance = Mathf.Clamp((hit.distance - wallDistance), _MinDistance, _MaxDistance);
        }
        else
        {
            //If there's nothing between just leave the distance to the max distance
            _Distance = _MaxDistance;
        }

        //Set the position to the actual position to the distance obtained
        transform.localPosition = Vector3.Lerp(transform.localPosition, _DollyDir * _Distance, Time.deltaTime * _Smooth);
    }
}
