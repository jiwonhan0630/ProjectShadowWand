using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public bool canLadder = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canLadder)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //if (PlayerController.Instance.isCatching == false)
                //{
                //    PlayerController.Instance.SetIsLadder(true, gameObject.transform.position);
                //}
                PlayerController.Instance.SetIsLadder(true, gameObject.transform.position);
                //Debug.Log("SetIsLadder!!");
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (canLadder)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController.Instance.SetIsLadder(false, gameObject.transform.position);
            }
        }

    }
}
