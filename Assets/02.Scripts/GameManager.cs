using System.Collections;
using UnityEngine;

public enum EGameState
{
    Ready,
    Run,
    Over
}
public class GameManager : Singleton<GameManager>
{
    private EGameState _currentGameState;
    public EGameState CurrentGameState => _currentGameState;
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
        Time.timeScale = 1f;
        UIManager.Instance.GameManagerText.gameObject.SetActive(false);

    }
}
