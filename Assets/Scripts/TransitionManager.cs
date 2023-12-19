using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TransitionManager : MonoBehaviour
{
    [SerializeField,Tooltip("�t�F�[�h�Ɏg�p����p�l��")]
    private Image fadePanel;
    [SerializeField,Tooltip("�t�F�[�h�̎�������")]
    private float fadeDuration = 1f;

    void Start()
    {
        FadeIn();
    }

   public void FadeToScene(string sceneName)
    {
        // �w�肳�ꂽ�V�[���ւ̃t�F�[�h�A�E�g���J�n
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        float timer = 0;

        // �t�F�[�h�A�E�g����
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
        // �t�F�[�h�C�������̊J�n
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        float timer = 0;

        // �t�F�[�h�C������
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0,1,timer/fadeDuration);
            fadePanel.color = new Color(0,0,0,alpha);
            yield return null;
        }
    }
}
