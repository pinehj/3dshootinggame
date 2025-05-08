using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("게임매니저")]
    public TextMeshProUGUI GameManagerText;
    public UI_OptionPopup PopupOption;

    [Header("체력")]
    public Slider HealthSlider;
    public TextMeshProUGUI HealthText;
    [Header("스태미나")]
    public Slider StaminaSlider;
    public TextMeshProUGUI StaminaText;

    [Header("총알")]
    public TextMeshProUGUI BulletCountText;
    public Slider BulletReloadSlider;
    [Header("폭탄")]
    public TextMeshProUGUI BombCountText;
    public Slider BoombChargeSlider;

    [Header("피격 이펙트")]
    public Image HitEffectImage;
    public float effectTime;
    private Coroutine _hitEffectRoutine;

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(KeyCode.BackQuote))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (InputManager.Instance.GetKeyUp(KeyCode.BackQuote))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void InitializePlayerStaminaSlider(float value)
    {
        StaminaSlider.maxValue = value;
    }
    public void UpdateStaminaSlider(float value)
    {
        StaminaSlider.value = value;
        StaminaText.text = $"{value.ToString("0")} / {StaminaSlider.maxValue.ToString("0")}";
    }
    public void InitializePlayerHealthSlider(float value)
    {
        HealthSlider.maxValue = value;
    }
    public void UpdateHealthSlider(float value)
    {
        HealthSlider.value = value;
        HealthText.text = $"{value.ToString("0")} / {HealthSlider.maxValue.ToString("0")}";
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

    public void StartHitEffect()
    {
        if (_hitEffectRoutine != null)
        {
            StopCoroutine(_hitEffectRoutine);
        }
        _hitEffectRoutine = StartCoroutine(HitEffectRoutine());
    }

    private IEnumerator HitEffectRoutine()
    {
        HitEffectImage.gameObject.SetActive(true);
        Color newColor = HitEffectImage.color;
        newColor.a = 1;
        HitEffectImage.color = newColor;
        while (HitEffectImage.color.a >= 0)
        {
            newColor = HitEffectImage.color;
            newColor.a -= Time.deltaTime / effectTime;
            HitEffectImage.color = newColor;
            yield return null;
        }
        HitEffectImage.gameObject.SetActive(false);
    }

    public void OpenOptionPopup()
    {
        PopupOption.Open();
    }
    public void CloseOptionPopup()
    {
        PopupOption.Close();
    }
}
