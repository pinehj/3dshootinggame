using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Rendering;
using UnityEngine;

public enum EPopup
{
    UI_OptionPopup,
    UI_CreditPopup
}
public class PopupManager : Singleton<PopupManager>
{
    public List<UI_Popup> Popups;
    [SerializeField] private LinkedList<UI_Popup> _openedPopups = new LinkedList<UI_Popup>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_openedPopups.Count >0)
            {
                _openedPopups.Last.Value.Close();
                _openedPopups.RemoveLast();
            }
            else
            {
                GameManager.Instance.Pause();
            }
        }
    }
    public void Open(EPopup popupType, Action closeCallback = null)
    {
        foreach(UI_Popup popup in Popups)
        {
            if(popup.gameObject.name == popupType.ToString())
            {
                popup.Open(closeCallback);
                _openedPopups.AddLast(popup);
                break;
            }
            else
            {
                UnityEngine.Debug.Log("팝업찾기 실패");
            }
        }
    }
    public void Close(EPopup popupType)
    {
        foreach (UI_Popup popup in Popups)
        {
            if (popup.gameObject.name == popupType.ToString())
            {
                popup.Close();
                _openedPopups.Remove(popup);
                break;
            }
            else
            {
                UnityEngine.Debug.Log("팝업찾기 실패");
            }
        }
    }
}
