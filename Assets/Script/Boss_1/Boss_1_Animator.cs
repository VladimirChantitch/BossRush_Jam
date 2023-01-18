using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Animator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayTargetAnimation(bool looping, float fadeTime)
    {

    }
}
