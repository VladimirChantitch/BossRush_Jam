using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MHR_BiteDetector : MonoBehaviour
{
    [SerializeField] private MobileHomeRex mrh;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            mrh.EnterState(MobileHomeRex.MobileHomeState.AttackingBite);
            return;
        }
    }
}
