using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    /// ------------------------------------------------------------------------
    /// Class variables --------------------------------------------------------
    /// ------------------------------------------------------------------------
    [Range(0f, 360f)] [SerializeField] private float _ViewAngle = 90f;        //Angle of the FOV
    [SerializeField] private float _ViewRadius = 3f;        //Radius of the FOV
    [SerializeField] private LayerMask _TargetMask;         //Layer of the objects to include
    [SerializeField] private LayerMask _ObstacleMask;       //Layer of the objects to ignore

    public List<Transform> VisibleTargets { get; private set; } = new List<Transform>();        //List of all objects detected


    /// <summary>
    /// Returns the radius and angle of the FOV
    /// </summary>
    public (float viewRadius, float viewAngle) GetRadiusAngle()
    {
        return (_ViewRadius, _ViewAngle);
    }


    /// <summary>
    /// Unity's Update method. Called once per frame
    /// </summary>
    private void Update()
    {
        //Updates the number of targets inside the FOV
        VisibleTargets = transform.GetTargetsInFoV(_ViewRadius, _ViewAngle, _TargetMask, _ObstacleMask);
    }


    /// <summary>
    /// Updates the visible targets inside the FOV
    /// </summary>
    public void UpdateList()
    {
        //Updates the number of targets inside the FOV
        VisibleTargets = transform.GetTargetsInFoV(_ViewRadius, _ViewAngle, _TargetMask, _ObstacleMask);
    }


    /// <summary>
    /// Unity's OnDrawGizmos used for visual debugging in Scene view
    /// </summary>
    private void OnDrawGizmos()
    {
        //Updates the number of targets inside the FOV
        VisibleTargets = transform.GetTargetsInFoV(_ViewRadius, _ViewAngle, _TargetMask, _ObstacleMask);
    }

}
