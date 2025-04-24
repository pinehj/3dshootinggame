using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraManager : MonoBehaviour
{
    public enum ECameraMode
    {
        FPS,
        TPS,
        QV
    }

    private ECameraMode _currentMode;


    public float RotationSpeed = 15f;
    private float _rotationX = 0;
    private float _rotationY = 0;

    private Transform _target;
    public Transform FPSTarget;
    public Transform TPSTarget;
    public Transform QVTarget;

    public Transform TPSPivot;

    private void Start()
    {
        _currentMode = ECameraMode.FPS;
        _target = FPSTarget;
    }

    private void Update()
    {
        ChangeCameraMode();
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 마우스 입력으로부터 회전시킬 방향을 만든다
        //Vector3 dir = new Vector3(-mouseY, mouseX, 0);
        // 3. 카메라를 해당 방향으로 회전한다.
        // 새로운 위치 = 현재 위치 + 속도 * 시간
        _rotationX = mouseX * RotationSpeed * Time.deltaTime;
        _rotationY = mouseY * RotationSpeed * Time.deltaTime;
        //_rotationY = Mathf.Clamp(_rotationY, -90, 90);

        //transform.eulerAngles = new Vector3(-_rotationY, _rotationX);


    }
    private void LateUpdate()
    {


        // 1. 마우스 입력을 받는다.(마우스의 커서의 움직임 방향)


        //90 ~ 0   270 ~ 360
        Rotate(new Vector2(_rotationX, _rotationY));
        Follow();
        //transform.rotation = new Quaternion(Mathf.Clamp((transform.rotation.x - _rotationY), -90, 90), transform.rotation.y + _rotationX, 0, 0);
        // 회전의 상하 제한 필요(-90, 90)
        //Vector3 rotation = transform.eulerAngles;
        //rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        //transform.eulerAngles = rotation;
    }
    public void Follow()
    {
        transform.position = _target.position;
    }
    public void Rotate(Vector2 delta)
    {
        switch (_currentMode)
        {
            case ECameraMode.FPS:
            case ECameraMode.TPS:
            {
                TPSPivot.eulerAngles = new Vector3(Mathf.Clamp(((TPSPivot.eulerAngles.x >= 270) ? TPSPivot.eulerAngles.x - 360 : TPSPivot.eulerAngles.x) - delta.y, -90, 90)
                                    , TPSPivot.eulerAngles.y, 0);

                transform.eulerAngles = new Vector3(Mathf.Clamp(((transform.eulerAngles.x >= 270) ? transform.eulerAngles.x - 360 : transform.eulerAngles.x) - delta.y, -90, 90)
                                    , transform.eulerAngles.y + delta.x, 0);
                break;
            }
        }




    }

    public void ChangeCameraMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _currentMode = ECameraMode.FPS;
            _target = FPSTarget;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _currentMode = ECameraMode.TPS;
            _target = TPSTarget;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _currentMode = ECameraMode.QV;
            _target = QVTarget;
        }
    }
}
