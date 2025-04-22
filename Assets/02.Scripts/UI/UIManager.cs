using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
       
    }

    public Slider StaminaSlider;
    public TextMeshProUGUI StaminaText;
    public void InitializePlayerStaminaSlider(float value)
    {
        StaminaSlider.maxValue = value;
    }
    public void UpdateStaminaSlider(float value)
    {
        StaminaSlider.value = value;
        StaminaText.text = $"{value.ToString("0")} / {StaminaSlider.maxValue.ToString("0")}";
    }
}
