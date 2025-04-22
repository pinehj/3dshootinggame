using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    [Header("스태미나")]
    public Slider StaminaSlider;
    public TextMeshProUGUI StaminaText;

    [Header("총알")]
    public TextMeshProUGUI BulletCountText;
    public Slider BulletReloadSlider;
    [Header("폭탄")]
    public TextMeshProUGUI BombCountText;
    public Slider BoombChargeSlider;

    public void InitializePlayerStaminaSlider(float value)
    {
        StaminaSlider.maxValue = value;
    }
    public void UpdateStaminaSlider(float value)
    {
        StaminaSlider.value = value;
        StaminaText.text = $"{value.ToString("0")} / {StaminaSlider.maxValue.ToString("0")}";
    }

    public void UpdateBulletCount(int bulletCount, int maxBulletCount)
    {
        BulletCountText.text = $"{bulletCount} / {maxBulletCount}";
    }
    public void UpdateBombCount(int boomCount, int maxBoomCount)
    {
        BombCountText.text = $"{boomCount} / {maxBoomCount}";
    }

    public void InitializeBulletReloadSlider(float value)
    {
        BulletReloadSlider.maxValue = value;
    }
    public void UpdateBulletReloadSlider(float value)
    {
        BulletReloadSlider.value = value;

        if (BulletReloadSlider.value > 0)
        {
            if (!BulletReloadSlider.IsActive())
            {
                BulletReloadSlider.gameObject.SetActive(true);
            }
        }
        else
        {
            BulletReloadSlider.gameObject.SetActive(false);
        }
    }
    public void InitializeBombChargeSlider(float value)
    {
        BoombChargeSlider.maxValue = value;
    }
    public void UpdateBombChargeSlider(float value)
    {
        BoombChargeSlider.value = value;

        if (BoombChargeSlider.value > 0)
        {
            if (!BoombChargeSlider.IsActive())
            {
                BoombChargeSlider.gameObject.SetActive(true);
            }
        }
        else
        {
            BoombChargeSlider.gameObject.SetActive(false);
        }
    }
}
