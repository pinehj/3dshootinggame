using DG.Tweening;
using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



[Serializable]
public class UI_InputFields
{
    public TextMeshProUGUI ResultText;
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordConfirmInputField;
    public Button ConfirmButton;
}
public class UI_LoginScene : MonoBehaviour
{
    [Header("패널")]

    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    [Header("로그인")]
    public UI_InputFields LoginInputFields;
    [Header("회원가입")]
    public UI_InputFields RegisterInputFields;

    [Header("결과 알림 효과")]
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakeStrength;

    private const string PREFIX = "ID_";
    private const string SALT = "19521";
    private void Start()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);

        LoginInputFields.ResultText.text = string.Empty;
        RegisterInputFields.ResultText.text = string.Empty;

        LoginCheck();
    }

    public void OnClickGoToRegisterButton()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
        LoginInputFields.ResultText.text = string.Empty;
    }
    public void OnClickGoToLoginButton()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
        RegisterInputFields.ResultText.text = string.Empty;
    }

    public void Login()
    {
        string id = LoginInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            AlertResult(LoginInputFields,"아이디를 입력해주세요.");
            return;
        }

        string password = LoginInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            AlertResult(LoginInputFields, "비밀번호를 입력해주세요.");
            return;
        }

        if (!PlayerPrefs.HasKey(PREFIX + id))
        {
            AlertResult(LoginInputFields, "아이디 또는 비밀번호가 일치하지 않습니다.");
            return;
        }
        if(!string.Equals(PlayerPrefs.GetString(PREFIX + id), Encryption(password)))
        {
            AlertResult(LoginInputFields, "아이디 또는 비밀번호가 일치하지 않습니다.");
            return;
        }


        SceneManager.LoadScene(1);
    }
    public void Register()
    {
        string id = RegisterInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            AlertResult(RegisterInputFields, "아이디를 입력해주세요.");
            return;
        }

        string password = RegisterInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            AlertResult(RegisterInputFields, "비밀번호를 입력해주세요.");

            return;
        }

        string passwordConfirm = RegisterInputFields.PasswordConfirmInputField.text;
        if(!string.Equals(password, passwordConfirm))
        {
            AlertResult(RegisterInputFields, "비밀번호가 일치하지 않습니다.");

            return;
        }

        PlayerPrefs.SetString(PREFIX + id, Encryption(password));

        LoginInputFields.IDInputField.text = id;
        OnClickGoToLoginButton();
    }

    private void AlertResult(UI_InputFields inputFields, string message)
    {
        inputFields.ResultText.text = message;
        inputFields.ResultText.transform.DOShakePosition(_shakeDuration, _shakeStrength, randomness:0);
    }

    public string Encryption(string text)
    {
        SHA256 sha256 = SHA256.Create();

        byte[] bytes = Encoding.UTF8.GetBytes(text + SALT);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = string.Empty;
        foreach (byte b in hash)
        {
            resultText += b.ToString("X2");
        }

        return resultText;
    }

    public void LoginCheck()
    {
        string id = LoginInputFields.IDInputField.text;
        string password = LoginInputFields.PasswordInputField.text;

        LoginInputFields.ConfirmButton.enabled = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password);
    }
}
