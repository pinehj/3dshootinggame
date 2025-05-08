public class UI_OptionPopup : UI_Popup
{
    public void OnClickContinueButton()
    {
        PopupManager.Instance.Close(EPopup.UI_OptionPopup);
    }
    public void OnClickRetryButton()
    {
        GameManager.Instance.Restart();
    }
    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
# else
Application.Quit();
# endif
    }
    public void OnClickCreditButton()
    {
        PopupManager.Instance.Open(EPopup.UI_CreditPopup);
    }
}
