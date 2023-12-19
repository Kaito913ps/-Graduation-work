using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
   public void StartGame()
    {
        GameManager.instance.StartStage("Stage1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SelectStage(int stageNumber)
    {
        //�X�e�[�W�I���̃��W�b�N
        //�X�e�[�W���A�����b�N����Ă��邩�m�F����
        string stageName = "Stage" + stageNumber.ToString();
        GameManager.instance.StartStage(stageName);
    }
}
