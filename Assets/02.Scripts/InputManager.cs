using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class InputManager : Singleton<InputManager>
{
    public float GetAxis(string axisName)
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetAxis(axisName);
        }
        else
        {
            return 0;
        }
    }

    public float GetAxisRaw(string axisName)
    {
        if(GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetAxisRaw(axisName);
        }
        else
        {
            return 0;
        }
    }

    public bool GetButton(string buttonName)
    {
        if(GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetButton(buttonName);
        }
        else
        {
            return false;
        }
    }
    public bool GetButtonDown(string buttonName)
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetButtonDown(buttonName);
        }
        else
        {
            return false;
        }
    }
    public bool GetButtonUp(string buttonName)
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetButtonUp(buttonName);
        }
        else
        {
            return false;
        }
    }

    public bool GetMouseButton(int button)
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetMouseButton(button);
        }
        else
        {
            return false;
        }
    }
    public bool GetMouseButtonDown(int button)
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetMouseButtonDown(button);
        }
        else
        {
            return false;
        }
    }
    public bool GetMouseButtonUp(int button)
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetMouseButtonUp(button);
        }
        else
        {
            return false;
        }
    }
    public bool GetKey(KeyCode key)
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetKey(key);
        }
        else
        {
            return false;
        }
    }
    public bool GetKeyDown(KeyCode key)
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetKeyDown(key);
        }
        else
        {
            return false;
        }
    }
    public bool GetKeyUp(KeyCode key)
    {
        if (GameManager.Instance.CurrentGameState == EGameState.Run)
        {
            return Input.GetKeyUp(key);
        }
        else
        {
            return false;
        }
    }
}
