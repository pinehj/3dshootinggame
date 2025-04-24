using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YOffset = 10f;

    public Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        Vector3 newPosition = Target.position;
        newPosition.y += YOffset;

        transform.position = newPosition;

        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;
        transform.eulerAngles = newEulerAngles;
    }

    public void ZoomIn()
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - 1, 5, 30);
    }
    public void ZoomOut()
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + 1, 5, 30);
    }
}
