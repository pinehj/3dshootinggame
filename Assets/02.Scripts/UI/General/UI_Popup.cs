using System;
using UnityEngine;

public class UI_Popup : MonoBehaviour
{
    private Action _closeCallback;
    public virtual void Open(Action closeCallback = null)
    {
        _closeCallback = closeCallback;
        gameObject.SetActive(true);
        Debug.Log("?ㅇ");
    }
    public virtual void Close()
    {
        _closeCallback?.Invoke();
        gameObject.SetActive(false);
    }
}
