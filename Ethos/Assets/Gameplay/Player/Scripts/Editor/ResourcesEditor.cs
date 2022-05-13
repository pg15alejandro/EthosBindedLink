using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOResources))]
public class ResourcesEditor : Editor
{
    /// <summary>
    /// Unity's OnInspectorGUI used to make a custom inspector
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Make sure that the target is a SOResources
        if (!(target is SOResources resources))
            return;

        //Add a button which will decrease the health of the player
        if (GUILayout.Button("Deal Damage"))
        {
            resources.CurrentHealth -= 10;
        }
    }
}