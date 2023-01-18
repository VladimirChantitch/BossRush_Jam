using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boss
{
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

        [SerializeField] private Boss_1_Animator animator;

        [Header("State")]
        [SerializeField] private bool isPhaseTwo;
        [SerializeField] private MaxiBestOfState state;
        public bool isAttacking;
        public bool isVulnerable;
        public int attackPhaseCounter;

        [Header("Attack Logic")]
        [SerializeField] List<AttackData> attackDatas = new List<AttackData>(4);
        [SerializeField] AttackData current_attack;

        [Header("Collider reference")]
        [SerializeField] C_BossAttackCollider rightCollider;
        [SerializeField] C_BossAttackCollider leftCollider;
        [SerializeField] VulnerabilityData selectedHand;

        private void Start()
        {
            animator = GetComponent<Boss_1_Animator>();
            state = MaxiBestOfState.Idle;

            rightCollider.applyDamageToTarget.AddListener(target => target.AddDamage(current_attack.damageAmount));
            leftCollider.applyDamageToTarget.AddListener(target => target.AddDamage(current_attack.damageAmount));
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
            animator.PlayTargetAnimation(true, "Appearing", 0.15f);

            // This animation is coming with cam shake and epic music
            // At the end of the animation there is a Ui elements that shows that the fight starts
            // The Ui changes the state to awaiting which starts the fight for real
        }

        #endregion

        #region Awaiting
        private void Awaiting()
        {

            // Awaits rdn time with an animation max 3
            // Goes to attacking at the end of the animation
        }

        #endregion

        #region Attacking 
        private void Attacking()
        {
            if (isAttacking == false)
            {
                isAttacking = true;
                attackPhaseCounter += 1;
            }
            // Picks an attack that makes sens
            // plays the attack 
        }

        /// <summary>
        /// Method called ByAnimation Event
        /// </summary>
        public void AttackFinished()
        {
            if (attackPhaseCounter >= 2)
            {
                attackPhaseCounter = 0;

                if ((int)Time.time % 2 == 0)
                {
                    state = MaxiBestOfState.Vulnerability_rightHand;
                }
                else
                {
                    state = MaxiBestOfState.Vulnerability_leftHand;
                }
            }
            else
            {
                state = MaxiBestOfState.Awaiting;
            }
            isAttacking = false;
        }

        #endregion

        #region Vulnerability_Hand
        private void Vulnerability(bool isLeft)
        {
            if (isVulnerable == false)
            {
                isVulnerable = true;
                if ((int)Time.time % 2 == 0)
                {
                    selectedHand = new VulnerabilityData();
                }
                else
                {
                    selectedHand = new VulnerabilityData();
                }
            }

                // Picks a hand
                // Plays ans animation to show vulnerability
                // Gives player 10 to 15 sec to hit the vulnerability while playing a recovering animation 


        }

        public void VulnerabilityFinished(bool isDestroyed)
        {
            if (isDestroyed)
            {
                // If hit the boss goes back in Awaiting but has only one hand left
                // If it was the second hand, its goes to head vulnerablity
            }
            else
            {
                // If player is too slow the hand inflicts damages
            }
        }
        #endregion

        #region Vulnerability_Head
        private void HeadVulnerability()
        {
            // Plays an animation tu show that the head is vulnerable 
            // the player can hit and inflict damages to the boss
            // after a short 10 to 15 sec the boss goes back to awaiting phase
            // If under 50% HPs the boss goes to second phase
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

    [System.Serializable]
    public class AttackData
    {
        [Header("Values")]
        public float damageAmount;
        /// <summary>
        /// 0 to 100
        /// </summary>
        public int proba;
        public bool isSecondPhase;
        [Header("data")]
        public AnimationClip associatedAnimation;
    }

    [System.Serializable]
    public class VulnerabilityData
    {


    }
}


