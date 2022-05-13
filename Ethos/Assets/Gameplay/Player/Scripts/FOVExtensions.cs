using System;
using System.Collections.Generic;
using UnityEngine;

//Put into Transform extension methods
public static class FOVExtensions
{
    /// <summary>
    /// Returns a list of all the targets inside the FOV
    /// </summary>
    /// <param name="transform">The transform where the FOV is going to start</param>    
    /// <param name="viewRadius">The distance of the FOV</param>
    /// <param name="viewAngle">The angle of the FOV</param>
    /// <param name="targetMask">The target layer you want to detect</param>
    /// <param name="obstacleMask">The obstacle layer you want to ignore</param>
    public static List<Transform> GetTargetsInFoV(this Transform transform, float viewRadius, float viewAngle, LayerMask targetMask, LayerMask obstacleMask)
    {
        //Create the empty list to be returned
        var visibleTargets = new List<Transform>();

        //Get all the enemies inside a sphere with the given radius
        var targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        //Test if every item inside the viewRadius are inside the specified angle
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            var target = targetsInViewRadius[i].transform;
            var dirToTarget = (target.position - transform.position).normalized;

            //If the target is outisde the specified angle, go to the next object
            if (Vector3.Angle(transform.forward, dirToTarget) > viewAngle / 2f)
                continue;

            //Get the distance between the object and the target
            float dist = Vector3.Distance(transform.position, target.position);

            //If there's a raycast between the two objects, add the target to the returning list
            if (Physics.Raycast(transform.position, dirToTarget, dist, obstacleMask) == false)
                visibleTargets.Add(target);
        }

        //Return the populated list
        return visibleTargets;
    }


    /// <summary>
    /// Returns true if the given transform is inside the field of view
    /// </summary>
    /// <param name="transform">The transform where the FOV is going to start</param>    
    /// <param name="pointToCheck">The transform we want to check if is inside the FOV</param>
    /// <param name="viewDistance">The distance of the FOV</param>
    /// <param name="viewAngle">The angle of the FOV</param>
    public static bool TargetInFoV(this Transform transform,
                                        Transform pointToCheck,
                                        float viewDistance,
                                        float viewAngle)
    {
        //Get the distnce between the beggining of the fov and the position to check
        var dist = Vector3.Distance(pointToCheck.position, transform.position);

        //If the distance is bigger than the viewDistance return false
        if (dist > viewDistance) return false;

        //Get the vector from the FOV transform to the point to check
        var dirToTarget = (pointToCheck.position - transform.position).normalized;

        //Check if the angle formed by that vector is lower than the viewAngle
        return !(Vector3.Angle(transform.forward, dirToTarget) > viewAngle / 2f);
    }


    /// <summary>
    /// Returns a Vector3 with the transform given after applying the given angle
    /// </summary>
    /// <param name="transform">The transform where the FOV is going to start</param>
    /// <param name="angleInDegrees">The angle applied to the transform</param>
    /// <param name="globalAngle">Specifies if the angle is global or not</param>
    public static Vector3 DirFromAngle(this Transform transform, float angleInDegrees, bool globalAngle)
    {
        if (globalAngle == false)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
