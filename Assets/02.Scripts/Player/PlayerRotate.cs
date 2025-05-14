using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f;

    private float _rotationX = 0;

    private void Update()
    {
        float mouseX = InputManager.Instance.GetAxis("Mouse X");

        if (CameraManager.Instance.CurrentMode == ECameraMode.QV)
        {
            Vector2 targetPos = InputManager.Instance.GetMousePositionFromCenter();

            if(targetPos == Vector2.zero)
            {
                return;
            }
            transform.forward = new Vector3(targetPos.x, 0, targetPos.y);
        }
        else
        {
            _rotationX = mouseX * RotationSpeed * Time.deltaTime;
            //transform.eulerAngles = new Vector3(0, _rotationX, 0);

            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + _rotationX, 0);
        }
    }
}
