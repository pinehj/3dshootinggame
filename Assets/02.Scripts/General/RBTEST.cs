using UnityEngine;

public class RBTEST : MonoBehaviour
{
    public GameObject zz;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {

        _rb.AddForce(Vector3.up * 10
                                //* (ThrowBombPower + BombChargePower * (BombChargeTime / PlayerFireDataSO.MaxBombChargeTime))
                                , ForceMode.Impulse);
    }
}
