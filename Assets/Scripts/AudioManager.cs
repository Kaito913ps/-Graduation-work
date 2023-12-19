using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // BGMを再生するためのオーディオソース
    public AudioSource bgmSource;
    // 効果音を再生するためのオーディオソース
    public AudioSource sfxSource;

    //BGM用のクリップ配列
    public AudioClip[] bgmClips;
    //SE用のクリップ配列
    public AudioClip[] sfxClips;

    private Dictionary<string,AudioClip> bgmDict = new Dictionary<string, AudioClip> ();
    private Dictionary<string,AudioClip> sfxDict = new Dictionary<string,AudioClip> ();

    private void Awake()
    {
        // シングルトンパターン
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioClips();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioClips()
    {
        // BGMと効果音のクリップを辞書に格納
        foreach (var clip in bgmClips)
        {
            bgmDict[clip.name] = clip;
        }

        foreach(var clip in sfxClips)
        {
            sfxDict[clip.name] = clip;
        }
    }

    public void PlaySFX(string name)
    {
        // 指定された名前の効果音を再生
        if (sfxDict.TryGetValue(name, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayBGM(string name)
    {
        // 指定された名前のBGMを再生
        if (bgmDict.TryGetValue(name,out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }
}
