using UnityEngine;

public class Skybox : MonoBehaviour
{
    [SerializeField] float rotationSpeed;


    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", (rotationSpeed * Time.time) % 360);
    }
}
