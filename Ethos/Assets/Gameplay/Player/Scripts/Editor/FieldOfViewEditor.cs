using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    /// <summary>
    /// Unity's OnSceneGUI. Enables the Editor to handle an event in the Scene View
    /// </summary>
    private void OnSceneGUI()
    {
        //Make sure that the target is a FieldOfView
        if (!(target is FieldOfView fov))
            return;

        var transform = fov.transform;                                  //Transform of the FOV
        var position = transform.position;                              //Position of the gameobject
        (float viewRadius, float viewAngle) = fov.GetRadiusAngle();     //Angle and Radius of the FOV
        var vAngle = viewAngle / 2f;                                    //Half angle of the FOV

        //Get the two corners of the FOV view        
        Vector3 vieAngleA = transform.DirFromAngle(-vAngle, false);
        Vector3 vieAngleB = transform.DirFromAngle(vAngle, false);

        //Set to white the color of the Handles drawer and draw the cone
        Handles.color = Color.white;
        Handles.DrawWireArc(position, Vector3.up, vieAngleA, viewAngle, viewRadius);
        Handles.DrawLine(position, position + vieAngleA * viewRadius);
        Handles.DrawLine(position, position + vieAngleB * viewRadius);

        //Set to red the color of the Handles drawer
        Handles.color = Color.red;

        //Draw a line to each target inside the FOV
        foreach (var item in fov.VisibleTargets)
        {
            Handles.DrawLine(position, item.position);
        }
    }
}

