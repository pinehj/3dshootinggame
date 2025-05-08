using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameState
{
    Ready,
    Run,
    Pause,
    Over
}
public class GameManager : Singleton<GameManager>
{
    private EGameState _currentGameState;
    public EGameState CurrentGameState => _currentGameState;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        _currentGameState = EGameState.Ready;
        Time.timeScale = 0f;
        UIManager.Instance.GameManagerText.gameObject.SetActive(true);
        UIManager.Instance.GameManagerText.text = "Ready...";
        yield return new WaitForSecondsRealtime(2f);
        UIManager.Instance.GameManagerText.text = "Go!";
        yield return new WaitForSecondsRealtime(.5f);
        _currentGameState = EGameState.Run;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        UIManager.Instance.GameManagerText.gameObject.SetActive(false);

    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
        PopupManager.Instance.Open(EPopup.UI_OptionPopup, closeCallback:Continue);
        _currentGameState = EGameState.Pause;
    }

    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
        Debug.Log("컨티뉴");
        _currentGameState = EGameState.Run;
    }

    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
        _currentGameState = EGameState.Run;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
