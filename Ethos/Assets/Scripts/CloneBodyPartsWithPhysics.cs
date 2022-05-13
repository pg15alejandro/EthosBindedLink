using UnityEngine;

public class CloneBodyPartsWithPhysics : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _BodyPartsToClone;

    [SerializeField]
    private GameObject _ObjectToDisable;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            CloneBody();
            _ObjectToDisable.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.P))
            Time.timeScale = 1f;
    }

    [ExecuteInEditMode, ContextMenu("Do It")]
    public void CloneBody()
    {
        var dad = new GameObject($"[CLONE] {_ObjectToDisable.name}").transform;
        foreach (var item in _BodyPartsToClone)
        {
            var clone = Instantiate(item, item.transform.position, item.transform.rotation, dad);
            clone.GetComponent<SkinnedMeshRenderer>().rootBone = null;
            clone.AddComponent<Rigidbody>();
        }
    }
}
