using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1 : AbstractCharacter
{
    public enum MaxiBestOfState
    {
        Idle,
        Appearing,
        Awaiting,
        Attacking,
        Vulnerability_rightHand,
        Vulnerability_leftHand,
        Vulnerability_Head,
        Dying,
    }

    [SerializeField] private Animator animator;

    [Header("State")]
    [SerializeField] private bool isPhaseTwo;
    [SerializeField] private MaxiBestOfState state;

    private void Start()
    {
        animator = GetComponent<Animator>();
        state = MaxiBestOfState.Idle;
    }

    private void Update()
    {
        HandleState();
    }

    public void StartFight()
    {
        state = MaxiBestOfState.Appearing;
    }

    private void HandleState()
    {
        switch (state)
        {
            case MaxiBestOfState.Idle:
                break;
            case MaxiBestOfState.Appearing:
                Appearing();
                break;
            case MaxiBestOfState.Awaiting:
                Awaiting();
                break;
            case MaxiBestOfState.Attacking:
                Attacking();
                break;
            case MaxiBestOfState.Vulnerability_rightHand:
                Vulnerability(false);
                break;
            case MaxiBestOfState.Vulnerability_leftHand:
                Vulnerability(true);
                break;
            case MaxiBestOfState.Vulnerability_Head:
                HeadVulnerability();
                break;
            case MaxiBestOfState.Dying:
                Dying();    
                break;
        }
    }

    #region Appearing
    private void Appearing()
    {
        // Launch an animation
        // This animation is coming with cam shake and epic music
        // At the end of the animation there is a Ui elements that shows that the fight starts
        // The Ui changes the state to awaiting which starts the fight for real
    }

    #endregion

    #region Awaiting
    private void Awaiting()
    {
        // Awaits rdn time with an animation
        // Goes to attacking at the end of the animation
    }

    #endregion

    #region Attacking 
    private void Attacking()
    {
        // Picks an attack that makes sens
        // plays the attack 
        // gets back to awaiting
        // after 2 phases gets to a hand vulnerability.
    }

    #endregion

    #region Vulnerability_Hand
    private void Vulnerability(bool isLeft)
    { 
        // Picks a hand
        // Plays ans animation to show vulnerability
        // Gives player 10 to 15 sec to hit the vulnerability while playing a recovering animation 
            // If player is too slow the hand inflicts damages
            // If hit the boss goes back in Awaiting but has only one hand left
                // If it was the second hand, its goes to head vulnerablity
    }
    #endregion

    #region Vulnerability_Head
    private void HeadVulnerability()
    {
        // Plays an animation tu show that the head is vulnerable 
        // the player can hit and inflict damages to the boss
        // after a short 10 to 15 sec the boss goes back to awaiting phase
            // If his Hps get to 0 at any point the boss goes to the Dying Phase
    }
    #endregion

    #region Dying
    private void Dying()
    {
        // Stops the fight, the player gets to a lock position 
        // The  boss plays a small animation 
        // Explosion 
        // Three item are displayed 
        // The player is unlocked and goes on them to collect
        // Once collect is done the player goes back to the Hub
    }
    #endregion
}
