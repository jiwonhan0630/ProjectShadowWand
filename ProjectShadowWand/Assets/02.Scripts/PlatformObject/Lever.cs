using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject
{
    [Header("����� �÷���")]
    public MovePlatform[] movePlatforms;


    public LeverGroup leverGroup;
    public int leverIndex;
    public bool isOn;

    public void SetLeverGroupAndIndex(LeverGroup _g, int _i)
    {
        leverGroup = _g;
        leverIndex = _i;
    }
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        player = PlayerController.Instance;
    }


    public void SetIsOn(bool _b)
    {
        isOn = _b;
    }
    public override void DoInteract()
    {
        SetIsOn(!isOn);

        if (isOn)
        {
            leverGroup.UpdateLeverToggle(leverIndex);
        }
    }

    public override void SetTouchedObject(bool _b)
    {
        base.SetTouchedObject(_b);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagPlayer))
        {
            SetTouchedObject(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagPlayer))
        {
            SetTouchedObject(false);
        }

    }

}