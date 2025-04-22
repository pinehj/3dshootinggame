using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 2. 발사 위치에 수류탄 생성
    // 3. 생성된 수류탄을 카메라 방향으로 물리적인 힘 가하기

    [Header("설정값")]
    public PlayerFireDataSO PlayerFireDataSO;
    private Camera _mainCamera;

    public GameObject FireBombPosition;
    public GameObject BombPrefab;

    public GameObject FireBulletPosition;
    public GameObject BulletEffectPrefab;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        FireBullet();
        FireBomb();
    }
    private void FireBullet()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = new Ray(FireBulletPosition.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                GameObject BulletEffect = Instantiate(BulletEffectPrefab);
                
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward = hitInfo.normal;
                

                // 게임 수학: 선형대수학(스칼라, 벡터
            }
        }
    }
    private void FireBomb()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(BombPrefab);
            bomb.transform.position = FireBombPosition.transform.position;

            Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
            bombRigidbody.AddForce((Camera.main.transform.forward + Camera.main.transform.up / 2) * PlayerFireDataSO.ThrowBombPower, ForceMode.Impulse);
            bombRigidbody.AddTorque(Vector3.one);
        }
    }
}
