using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    //BGM用のクリップ配列
    public AudioClip[] bgmClips;
    //SE用のクリップ配列
    public AudioClip[] sfxClips;

    private Dictionary<string,AudioClip> bgmDict = new Dictionary<string, AudioClip> ();
    private Dictionary<string,AudioClip> sfxDict = new Dictionary<string,AudioClip> ();

    private void Awake()
    {
        if(instance == null)
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
        foreach(var clip in bgmClips)
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
        if(sfxDict.TryGetValue(name, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayBGM(string name)
    {
        if(bgmDict.TryGetValue(name,out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }
}
