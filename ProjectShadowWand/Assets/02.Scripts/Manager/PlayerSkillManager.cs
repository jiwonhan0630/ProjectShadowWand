using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public bool isDrawGizmo = false;
    public Skill_WindGlide skillWindGilde;
    public Skill_LightningShock skillLightningShock;
    public Skill_WaterWave skillWaterWave;
    public Skill_Restore skillRestore;


    [Header("복원")]
    public bool TESTBOOL;

    [Header("바람")]
    public GameObject windEffect;


    [Header("물")]

    [Tooltip("물 스킬 시전 이펙트")]
    public GameObject waterEffect_Set;

    [Tooltip("물 스킬 발동 이펙트")]
    public GameObject waterEffect_Splash;

    [Header("번개")]

    [Tooltip("번개 스킬 발동 이펙트")]
    public GameObject lightningEffect_Shock;

    [Tooltip("번개 스킬 잔류 스파크")]
    public GameObject lightningEffect_Spark;

    public Transform lightningPosition;

    List<Skill> skillList = null;

    public bool unlockWind;
    public bool unlockLightning;
    public bool unlockWater;
    public bool unlockRestore;

    public void Init()
    {
        CheckSkills();
        //if (skillList != null)
        //{
        //    for (int i = 0; i < skillList.Count; i++)
        //    {
        //        skillList[i].Init();
        //    }
        //}

    }


    public void RestoreInit()
    {

    }
    public void WindInit()
    {
        skillWindGilde.windEffect = windEffect;
        skillWindGilde.windAnimator = windEffect.GetComponent<Animator>();
        skillWindGilde.windAnimatorTornadoBlend = Animator.StringToHash("TornadoBlend");
        //skillWindGilde.Init();
    }


    public void WaterInit()
    {
        skillWaterWave.waterEffect_Set = waterEffect_Set;
        skillWaterWave.waterEffect_Splash = waterEffect_Splash;
        skillWaterWave.Init();
    }

    public void LightningInit()
    {
        skillLightningShock.lightningEffect_Shock = lightningEffect_Shock;
        skillLightningShock.lightningEffect_Spark = lightningEffect_Spark;
        skillLightningShock.lightningPosition = lightningPosition;
        skillLightningShock.Init();
    }
    /// <summary>
    /// unlock 여부에 따라 리스트에 스킬을 추가합니다.
    /// </summary>
    public void CheckSkills()
    {
        if (skillList != null)
        {
            skillList.Clear();

        }
        skillList = new List<Skill>();

        if (unlockWind == true)
        {
            skillWindGilde = new Skill_WindGlide(PlayerController.Instance);

            WindInit();

            skillList.Add(skillWindGilde);
        }
        if (unlockWater == true)
        {
            skillWaterWave = new Skill_WaterWave(PlayerController.Instance);

            WaterInit();

            skillList.Add(skillWaterWave);
        }
        if (unlockLightning == true)
        {

            skillLightningShock = new Skill_LightningShock(PlayerController.Instance);
            LightningInit();
            skillList.Add(skillLightningShock);
        }
        if (unlockRestore == true)
        {
            skillRestore = new Skill_Restore(PlayerController.Instance);
            RestoreInit();
            skillList.Add(skillRestore);
        }
    }


    public void Execute()
    {
        if (skillList.Count != 0)
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                skillList[i].Execute();
            }
        }

    }

    public void PhysicsExcute()
    {
        if (skillList.Count != 0)
        {
            for (int i = 0; i < skillList.Count; i++)
            {
                skillList[i].PhysicsExecute();
            }
        }
    }
    public void UnlockWind()
    {
        UnlockSkill(eSkill.WINDGILDE);
        YeonchoolManager.Instance.isCutscenePlaying = true;
        StartCoroutine(YeonchoolManager.Instance.StartCutscene(eCutsceneType.UNLOCK_WIND));
    }
    public void UnlockLightning()
    {
        UnlockSkill(eSkill.LIGHTNINGSHOCK);
        YeonchoolManager.Instance.isCutscenePlaying = true;
        StartCoroutine(YeonchoolManager.Instance.StartCutscene(eCutsceneType.UNLOCK_LIGHTNING));
    }

    public void UnlockWater()
    {
        UnlockSkill(eSkill.WATERWAVE);
        YeonchoolManager.Instance.isCutscenePlaying = true;
        StartCoroutine(YeonchoolManager.Instance.StartCutscene(eCutsceneType.UNLOCK_WATER));
    }

    public void UnlockRestore()
    {
        UnlockSkill(eSkill.RESTORE);
    }
    /// <summary>
    /// 스킬을 해금하고 획득합니다.이미 있으면 아무것도 안하지롱
    /// </summary>
    private void UnlockSkill(eSkill _skill)
    {
        switch (_skill)
        {
            case eSkill.RESTORE:
                if (unlockWind == false) //해금하지 않은 상태라면
                {
                    unlockRestore = true;
                    skillRestore = new Skill_Restore(PlayerController.Instance);

                    RestoreInit();
                    skillList.Add(skillRestore);
                }


                break;

            case eSkill.WINDGILDE:
                if (unlockWind == false) //해금하지 않은 상태라면
                {
                    unlockWind = true;
                    skillWindGilde = new Skill_WindGlide(PlayerController.Instance);

                    WindInit();
                    skillList.Add(skillWindGilde);
                }
                break;

            case eSkill.LIGHTNINGSHOCK:
                if (unlockLightning == false) //해금하지 않은 상태라면
                {
                    unlockLightning = true;
                    skillLightningShock = new Skill_LightningShock(PlayerController.Instance);
                    LightningInit();
                    skillList.Add(skillLightningShock);
                }
                break;

            case eSkill.WATERWAVE:
                if (unlockWater == false) //해금하지 않은 상태라면
                {
                    unlockWater = true;
                    skillWaterWave = new Skill_WaterWave(PlayerController.Instance);
                    WaterInit();
                    skillList.Add(skillWaterWave);
                }
                break;


            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (isDrawGizmo)
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireCube(waterEffect_Splash.transform.position, PlayerController.Instance.waterSize);
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(waterEffect_Splash.transform.position, PlayerController.Instance.waterDirection * PlayerController.Instance.waterDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(waterEffect_Splash.transform.position + (Vector3)PlayerController.Instance.waterDirection * PlayerController.Instance.waterDistance, PlayerController.Instance.waterSize);

            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(lightningPosition.position, PlayerController.Instance.lightningRadius);
        }

    }

}
