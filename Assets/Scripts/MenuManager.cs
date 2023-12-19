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
        //ステージ選択のロジック
        //ステージがアンロックされているか確認する
        string stageName = "Stage" + stageNumber.ToString();
        GameManager.instance.StartStage(stageName);
    }
}
