using UnityEngine;

[CreateAssetMenu(fileName = "Transform", menuName = "Variables")]
public class SOTransform : ScriptableObject
{
    public Transform Hand;
    public Transform value;
    public Vector3 checkpoint;
    public int ZoneLoad;

    public bool UseCheckpoint;
}
