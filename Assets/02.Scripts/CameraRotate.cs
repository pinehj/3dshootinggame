using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // 카메라 회전
    // 목표: 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    // 구현 순서
    public float RoatationSpeed = 15f;

    private void Update()
    {
        // 1. 마우스 입력을 받는다.(마우스의 커서의 움직임 방향)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Debug.Log($"MouseX: {mouseX}, MouseY: {mouseY}");

        // 2. 마우스 입력으로부터 회전시킬 방향을 만든다
        Vector3 dir = new Vector3(-mouseY, mouseX, 0);
        // 3. 카메라를 해당 방향으로 회전한다.
        // 새로운 위치 = 현재 위치 + 속도 * 시간

        transform.eulerAngles = transform.eulerAngles + dir * RoatationSpeed * Time.deltaTime;
    }
}
