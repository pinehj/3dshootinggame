using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    // Update is called once per frame
    void Update()
    {
        transform.position = Target.position;
    }
}
