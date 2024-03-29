﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameData
{

}

[System.Serializable]
public class Data_Settings : InGameData
{
    public bool isFullScreenMode; //전체화면모드인가?
    public string masterVolume;
    public string sfxVolume; //
    public string bgmVolume;

    public bool muteMasterVolume;
    public bool muteBgmVolume;
    public bool muteSfxVolume;

    public string brightness;
    public eResolutionData resolution;

    /// <summary>
    /// 디폴트 생성자. 기본값이 들어갑니다~~
    /// </summary>
    public Data_Settings() //생성자?
    {
        isFullScreenMode = true;

        muteMasterVolume = false;
        muteBgmVolume = false;
        muteSfxVolume = false;

        masterVolume = "0.7";
        sfxVolume = "0.5";
        bgmVolume = "0.5";
        brightness = "0.5";
        resolution = eResolutionData.HD;
    }

    /// <summary>
    /// 데이터를 넣어서 생성합니다.
    /// </summary>
    /// <param name="data">이 데이터가 넣어집니다.</param>
    public Data_Settings(Data_Settings data)
    {
        CopyFrom(data);
    }

    /// <summary>
    /// 해당하는 데이터를 자신에게 복사해 넣습니다.
    /// </summary>
    /// <param name="data">복사되어서 넣어질 테이터</param>
    public void CopyFrom(Data_Settings data)
    {
        this.isFullScreenMode = data.isFullScreenMode;
        this.masterVolume = data.masterVolume;
        this.bgmVolume = data.bgmVolume;
        this.sfxVolume = data.sfxVolume;
        this.muteMasterVolume = data.muteMasterVolume;
        this.muteBgmVolume = data.muteBgmVolume;
        this.muteSfxVolume = data.muteSfxVolume;


        this.brightness = data.brightness;
        this.resolution = data.resolution;
    }

    /// <summary>
    /// 데이터가 같으면 true를 리턴합니다.
    /// </summary>
    public bool Equal(Data_Settings data)
    {

        if (this.isFullScreenMode == data.isFullScreenMode&&
        this.masterVolume == data.masterVolume &&
        this.bgmVolume == data.bgmVolume &&
        this.sfxVolume == data.sfxVolume &&
        this.muteMasterVolume == data.muteMasterVolume &&
        this.muteBgmVolume== data.muteBgmVolume &&
        this.muteSfxVolume == data.muteSfxVolume &&
        this.brightness == data.brightness &&
        this.resolution == data.resolution)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class Data_Stage : InGameData
{
    public string stageName;
    public Data_Stage()
    {
        stageName = "Stage_00";
    }
    public Data_Stage(string _stageName)
    {
        stageName = _stageName;
    }
    /// <summary>
    /// 스테이지 이름을 미리 가진 채로 생성합니다.
    /// </summary>
    /// <param name="_stageName"></param>
    public Data_Stage(Data_Stage _data)
    {

        stageName = _data.stageName;
    }


}

[System.Serializable]
public class Data_Player : InGameData
{
    public int currentStage;
    public Vector3 currentPosition;
    // public float currentHP;

}

[System.Serializable]
public class Data_Child : InGameData
{
    public Data_Child() { }

    /// <summary>
    /// _data를 깊은 복사 합니다.
    /// </summary>
    /// <param name="_data">복사할 원본 데이터</param>
    public Data_Child(Data_Child _data)
    {

        currentStage = _data.currentStage;
        currentPosition = _data.currentPosition;
        isDie = _data.isDie;
        isFriend = _data.isFriend;
        isBye = _data.isBye;
        name = _data.name;
        diaryData = _data.diaryData;


    }

    public int currentStage;
    public Vector3 currentPosition; // 반딧불이 상태가 아닐 때에 사용합니다.
    public bool isDie; //빛의 폭발로 죽어있는 상태냐?
    public bool isFriend; // 반딧불이 상태냐?
    public bool isBye; //플레이어가 떠나보낸 상태냐?

    public string name;
    public string age;
    public string diaryData;
    public eChildType childType;
}

public class Data_ChildList : InGameData
{
    public List<Data_Child> childDataList;
}

