using UnityEngine;

public class GunRotate : MonoBehaviour
{
    Vector3 baseEulerOffset;
    public float RotationSpeed = 15f;
    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Start()
    {
        baseEulerOffset = transform.eulerAngles;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX = mouseX * RotationSpeed * Time.deltaTime;
        _rotationY = mouseY * RotationSpeed * Time.deltaTime;
    }
}
