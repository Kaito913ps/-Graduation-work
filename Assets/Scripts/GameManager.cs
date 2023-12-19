using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartStage(string stageName)
    {
        //�����Ńt�F�[�h�A�E�g,�t�F�[�h�C��������
        SceneManager.LoadScene(stageName);
    }

    public void CopleteStage()
    {
        //�X�e�[�W�N���A�̏���
        //���̃X�e�[�W�̃A�����b�N
    }
}
