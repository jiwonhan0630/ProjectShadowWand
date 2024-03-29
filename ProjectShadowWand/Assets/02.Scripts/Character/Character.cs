﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MovableObject
{
    [Header("캐릭터 관련")]
    [HideInInspector] public Animator animator = null;
    [HideInInspector] public Transform puppet = null;
    [HideInInspector] public SpriteRenderer spriteRenderer= null;


    [HideInInspector] public Vector2 movementInput;
    [Header("이동속도"), Tooltip("이동속도입니다.")] 
    public float movementSpeed = 0.0f;
    //[Tooltip("플레이어의 최대 이동속도. \n velocity에 관여합니다.")]
    //[HideInInspector]public float maxMovementSpeed = 0.0f;

    [Header("점프력"), Tooltip("점프력입니다.")]
    public float jumpForce = 0.0f;

    [HideInInspector] public float minFlipSpeed = 0.1f;
    [HideInInspector] public readonly Vector3 flippedScale = new Vector3(-1, 1, 1);
    [HideInInspector] public bool isFlipped;

}
