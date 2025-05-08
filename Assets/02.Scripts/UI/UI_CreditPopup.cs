using UnityEngine;

public class UI_CreditPopup : UI_Popup
{
    public void OnClickExitButton()
    {
        PopupManager.Instance.Close(EPopup.UI_CreditPopup);
    }

}
