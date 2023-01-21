using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1_Animator : MonoBehaviour
{
    [SerializeField] Animator animator;
    public void PlayTargetAnimation(bool looping, string animationName, float fadeTime)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animator.SetBool("Loop", looping);
        animator.CrossFade(animationName, fadeTime);
        animator.Play(animationName);
    }
}
