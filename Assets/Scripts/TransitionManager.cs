using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TransitionManager : MonoBehaviour
{
    [SerializeField,Tooltip("フェードに使用するパネル")]
    private Image fadePanel;
    [SerializeField,Tooltip("フェードの持続時間")]
    private float fadeDuration = 1f;

    void Start()
    {
        FadeIn();
    }

   public void FadeToScene(string sceneName)
    {
        // 指定されたシーンへのフェードアウトを開始
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        float timer = 0;

        // フェードアウト処理
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1,0,timer/fadeDuration);
            fadePanel.color = new Color(0,0,0,alpha);
            yield return null;
        }
    }

    public void FadeIn()
    {
        // フェードイン処理の開始
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        float timer = 0;

        // フェードイン処理
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0,1,timer/fadeDuration);
            fadePanel.color = new Color(0,0,0,alpha);
            yield return null;
        }
    }
}
