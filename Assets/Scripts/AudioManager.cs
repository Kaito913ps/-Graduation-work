using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // BGM���Đ����邽�߂̃I�[�f�B�I�\�[�X
    public AudioSource bgmSource;
    // ���ʉ����Đ����邽�߂̃I�[�f�B�I�\�[�X
    public AudioSource sfxSource;

    //BGM�p�̃N���b�v�z��
    public AudioClip[] bgmClips;
    //SE�p�̃N���b�v�z��
    public AudioClip[] sfxClips;

    private Dictionary<string,AudioClip> bgmDict = new Dictionary<string, AudioClip> ();
    private Dictionary<string,AudioClip> sfxDict = new Dictionary<string,AudioClip> ();

    private void Awake()
    {
        // �V���O���g���p�^�[��
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
        // BGM�ƌ��ʉ��̃N���b�v�������Ɋi�[
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
        // �w�肳�ꂽ���O�̌��ʉ����Đ�
        if (sfxDict.TryGetValue(name, out var clip))
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayBGM(string name)
    {
        // �w�肳�ꂽ���O��BGM���Đ�
        if (bgmDict.TryGetValue(name,out var clip))
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }
}
