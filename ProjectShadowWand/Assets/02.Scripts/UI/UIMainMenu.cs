using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : UIBase
{
    public CanvasGroup canvasGroup;

    public UIPopup uiPopup_NewGame;
    public UISettings uiSetting;
    public UIPopup uiPopup_ExitGame;

    public UICutScene cutscene;

    private void Start()
    {
        canvasGroup.interactable = true;
    }
    public void Button_GoNewGame(UIBase _ui)
    {
        if (SaveLoadManager.Instance.currentData_Stage.stageName == "Stage_00") //00스테이지라면
        {
            Button_StartNewGame();
        }
        else
        {
            _ui.Open();
        }

    }

    public void Button_StartNewGame()
    {
        canvasGroup.interactable = false;
        StartCoroutine(NewGameIntro());
    }

    private IEnumerator NewGameIntro()
    {
        cutscene = UIManager.Instance.uiDicitonary[eUItype.CUTSCENE] as UICutScene;

        cutscene.StartPlayCutScene();

        while (cutscene.isEnd == false)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        while (cutscene.isNext == false)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        SceneChanger.Instance.LoadThisSceneName("Stage_00", true);
       // AudioManager.Instance.Stop_Bgm();
    }


    //게임을 이어서 플레이합니다.
    public void Button_GoContinue()
    {
        //_ui.Open();
        if (SaveLoadManager.Instance.currentData_Stage.stageName == "Stage_00")
        {
            canvasGroup.interactable = false;
            SceneChanger.Instance.LoadThisSceneName(SaveLoadManager.Instance.currentData_Stage.stageName, false);
           // AudioManager.Instance.Stop_Bgm();
        }
        else
        {
            canvasGroup.interactable = false;
        //    SceneChanger.Instance.LoadThisSceneName(SaveLoadManager.Instance.currentData_Stage.stageName, false);
        }

    }

    public void Button_OpenSetting(UIBase _ui)
    {
        _ui.Open();
    }
    public void Button_OpenExit(UIBase _ui)
    {
        _ui.Open();
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

}
