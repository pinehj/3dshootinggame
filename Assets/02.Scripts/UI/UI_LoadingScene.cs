using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LoadingScene : MonoBehaviour
{
    public int NextSceneIndex = 2;

    public Slider ProgressSlider;

    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(LoadNextSceneRoutine());
    }
    private IEnumerator LoadNextSceneRoutine()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false; //비동기로 로드되는 씬의 모습이 화면에 보이지 않게 한다.

        while(ao.isDone == false)
        {

            ProgressSlider.value = ao.progress;
            ProgressText.text = $"{ProgressSlider.value * 100}%";

            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            else if (ao.progress >= 0.75f)
            {
                ProgressText.text += "미소 짓는 중..";
            }
            if (ao.progress >= 0.5f)
            {
                ProgressText.text += "장비를 챙기는 중..";
            }
            if (ao.progress >= 0.25f)
            {
                ProgressText.text += "쥐 코스튬으로 환복 중..";
            }
            else
            {
                ProgressText.text += "옷을 벗는 중..";
            }
            yield return null;
        }
    }
}
