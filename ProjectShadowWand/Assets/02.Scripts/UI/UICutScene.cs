﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UICut을 재생시켜주는 클래스.
/// </summary>
public class UICutScene : UIBase
{

    //TODO : 켜고 싶을 때 gameObject.SetActive(true)를 해버리면 처음부터 하이어라키에서 끄고 시작할 때 오류가 생김.
    //즉, 얘는 캔버스 그 자체면 조금 애매애매쓰 하다는 소리....
    //그러니까, 빈 게임 오브젝트에 해당 컴포넌트를 붙이자.
    
    public CanvasGroup canvasGroup;

    [Tooltip("컷 목록")]
    public UICut[] cutList;

    [SerializeField]
    private UICut currentCut;

    private int cutCount;
    private int currentCutNumber;

    [Range(0f, 5f)]
    public float fadeTime = 2f;

    [Tooltip("마지막 씬의 isOn까지 true가 되었다면, true가 됩니다.")]
    public bool isEnd;


    private bool isNext;
    //private void Start()
    //{
    //    Init();

    //}

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        cutCount = cutList.Length;
        isEnd = false;
        currentCut = null;
        currentCutNumber = 0;

        for (int i = 0; i < cutCount; i++)
        {
            cutList[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public override bool Open()
    {

        StartPlayCutScene();
        return true;
    }
    public override bool Close()
    {
        StartCloseCutScene();
        return true;
    }

    //이미지컷씬을 재생시작합니다.
    public void StartPlayCutScene()
    {
        gameObject.SetActive(true);
        StartCoroutine(ProcessCutScene());
    }

    private void Update()
    {
        if (InputManager.Instance.buttonTalkNext.wasPressedThisFrame)
        {
            isNext = true;
        }

    }
    public void StartCloseCutScene()
    {
        StartCoroutine(ProcessClose_Fade());
    }

    private IEnumerator ProcessOpen_Fade()
    {

        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;

        float timer = 0f;
        float progress = 0f;
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeTime;
            canvasGroup.alpha = Mathf.Clamp(progress, 0f, 1f);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        StartCoroutine(ProcessCutScene());
    }

    private IEnumerator ProcessCutScene()
    {
        if (cutCount == 0) //갯수가 0이면
        {
            Debug.LogWarning("UICutScene : 이상하다...재생시킬 컷이 없어! 추가를 깜빡한거 아니야?");
            yield break;
        }
        isNext = false;
        isEnd = false;

        currentCut = null;
        currentCutNumber = -1;

        while (currentCutNumber < cutCount - 1) //작을 때 까지 반복
        {
            isNext = false;
            currentCutNumber += 1;
            Debug.Log("현재 컷 넘버 : " + currentCutNumber);

            currentCut = cutList[currentCutNumber];

            currentCut.SetActive(true);


            currentCut.Open();

            while (!currentCut.isOn)
            {
                if (isNext)
                {
                    currentCut.isSkip = true;
                    break;
                }
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }

            float nextTimer = 0f;

            if (isNext == false)
            {
                while (nextTimer < currentCut.waitTime)
                {
                    if (isNext)
                    {
                        isNext = false;
                        break;
                    }
                    nextTimer += Time.unscaledDeltaTime;

                    yield return YieldInstructionCache.WaitForEndOfFrame;
                }

            }

        }

        yield return null;

        isEnd = true;
        StartCloseCutScene();
    }

    private IEnumerator ProcessClose_Fade()
    {

        float timer = 0f;
        float progress = 0f;
        float alphaVal = 1f;
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeTime;
            alphaVal = 1f - progress;
            canvasGroup.alpha = Mathf.Clamp(alphaVal, 0f, 1f);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        gameObject.SetActive(false);
    }

}
