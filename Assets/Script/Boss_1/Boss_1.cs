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
            PhaseTransition,
        }

        [SerializeField] private Boss_1_Animator animator;

        [Header("Stats")]
        [SerializeField] private float vulnerabilityTimer = 5;
        [SerializeField] private float headVulnerabilityTimer = 8;
        [SerializeField] private float vulnerabilityFailedRadius = 1.5f;
        [SerializeField] private float headVulnerabilityFailesradius = 2.5f;
        [SerializeField] private float explosionDamage = 5;

        [Header("State")]
        [SerializeField] private bool isPhaseTwo;
        [SerializeField] private MaxiBestOfState state;
        public bool isAttacking;
        public bool isVulnerable;
        public bool isDamageable;
        public bool isArriving;
        public bool isAwaiting;
        public int attackPhaseCounter;

        [Header("Attack Logic")]
        [SerializeField] List<AttackData> attackDatas = new List<AttackData>(4);
        [SerializeField] AttackData current_attack;

        [Header("Collider reference")]
        [SerializeField] C_Boss_1_AttackCollider rightCollider;
        [SerializeField] C_Boss_1_AttackCollider leftCollider;
        [SerializeField] Boss_1_Vulnerability right_vulnerability;
        [SerializeField] Boss_1_Vulnerability left_vulnerability;
        [SerializeField] Boss_1_Vulnerability selectedHand;
        [SerializeField] Boss_1_HeadVUlnerability headVUlnerability;

        [Header("Animations")]
        [SerializeField] AnimationClip Idle;
        [SerializeField] AnimationClip Arriving;
        [SerializeField] AnimationClip RightVulnerability;
        [SerializeField] AnimationClip LeftVulnerability;


        private void Start()
        {
            animator = GetComponent<Boss_1_Animator>();
            state = MaxiBestOfState.Appearing;
             
            rightCollider.applyDamageToTarget.AddListener(target => target.AddDamage(current_attack.damageAmount));
            leftCollider.applyDamageToTarget.AddListener(target => target.AddDamage(current_attack.damageAmount));
        }

        private void Update()
        {
            Debug.Log($"<color=purple> the current is {state} </color>");
            HandleState();
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
            if (isArriving == false)
            {
                isArriving = true;
                animator.PlayTargetAnimation(true, Arriving.name, 0.15f);
            }
        }

        public void initBoss()
        {
            state = MaxiBestOfState.Appearing;
        }

        #endregion

        #region Awaiting
        public void EnterAwaitingState()
        {
            state = MaxiBestOfState.Awaiting;
            isAwaiting = false;
        }

        private void Awaiting()
        {
            if (isAwaiting == false)
            {
                isAwaiting = true;
                animator.PlayTargetAnimation(false, Idle.name, 0.75f);
            }
        }

        #endregion

        #region Attacking 
        public void EnterAttackState()
        {
            state = MaxiBestOfState.Attacking;
            isAttacking = false;
        }

        private void Attacking()
        {
            if (isAttacking == false)
            {
                isAttacking = true;
                attackPhaseCounter += 1;

                List<AttackData> attackData_phase = attackDatas.Where(a => a.isSecondPhase == isPhaseTwo).ToList();

                current_attack = attackData_phase[Random.Range(0, attackData_phase.Count)];
                animator.PlayTargetAnimation(false, current_attack.associatedAnimation.name, 0.25f);
            }
        }

        /// <summary>
        /// Method called ByAnimation Event
        /// </summary>
        public void AttackFinished()
        {
            if (attackPhaseCounter >= 2)
            {
                attackPhaseCounter = 0;

                bool isLeft = (int)Time.time % 2 == 0;

                EnterVulnerabilityState(isLeft);
                EnterVulnerabilityState(isLeft);
            }
            else
            {
                EnterAwaitingState();
            }
        }

        #endregion

        #region Vulnerability_Hand
        public void EnterVulnerabilityState(bool isLeft)
        {
            if (isLeft)
            {
                state = MaxiBestOfState.Vulnerability_leftHand;
            }
            else
            {
                state = MaxiBestOfState.Vulnerability_rightHand;
            }
            isVulnerable = false;
        }

        public void EnterLeftVulnerability()
        {
            EnterVulnerabilityState(true);
        }

        public void EnterRightVulnerability()
        {
            EnterVulnerabilityState(false);
        }

        public void EnterRandomVulnerability()
        {
            EnterVulnerabilityState((int)Time.time % 2 == 0);
        }

        private void Vulnerability(bool isLeft)
        {
            if (isVulnerable == false)
            {
                isVulnerable = true;
                if (left_vulnerability.isDestroyed)
                {
                    selectedHand = right_vulnerability;
                }
                else if (right_vulnerability.isDestroyed)
                {
                    selectedHand = left_vulnerability;
                }
                else if (isLeft)
                {
                    selectedHand = right_vulnerability;
                }
                else
                {
                    selectedHand = left_vulnerability;
                }

                selectedHand.vulnerabilirabilityDestroyed.AddListener(() => VulnerabilityFinished(true));

                if (isLeft)
                {
                    animator.PlayTargetAnimation(false, LeftVulnerability.name, 0.75f);
                }
                else
                {
                    animator.PlayTargetAnimation(false, RightVulnerability.name, 0.75f);
                }
            }
        }

        public void VulnerabilityFinished()
        {
            VulnerabilityFinished(false);
        }

        public void VulnerabilityFinished(bool isDestroyed)
        {
            if (isDestroyed)
            {
                //selectedHand.CloseCollider();
                selectedHand.parent.SetActive(false);
                if(left_vulnerability.parent.activeInHierarchy == false && right_vulnerability.parent.activeInHierarchy == false)
                {
                    
                    EnterAwaitingState(); // to change for the transition animation
                }
                else
                {
                    EnterAwaitingState();
                }
            }
            else
            {
                //selectedHand.CloseCollider();
                DealDamageToNearByTargets(selectedHand.transform, explosionDamage);
                EnterAwaitingState();
            }
        }
        #endregion

        #region Vulnerability_Head
        public void EnterHeadVulnerability()
        {
            state = MaxiBestOfState.Vulnerability_Head;
            isDamageable = false;
        }

        private void HeadVulnerability()
        {
            if (isDamageable == false)
            {
                isDamageable = true;
                headVUlnerability.vulnerabilirabilityDestroyed.AddListener(amount => AddDamage(amount));
            }
            
            // Plays an animation tu show that the head is vulnerable 
            // the player can hit and inflict damages to the boss
        }

        private void HeadVulnerabilityFinished()
        {
            state = MaxiBestOfState.Awaiting;
            if (Health <= 0.5 * MaxHealth)
            {
                isPhaseTwo = true;
                // Do something epic
            }
            else if (Health <= 0)
            {
                // the boss goes to the Dying Phase -- shouldnt be launched caudsed well hum its alreaduy handed by abstrct character 
            }
            else
            {
                //DealDamageToNearByTargets(head,explosionDamage)
            }
            isDamageable = false;
        }
        #endregion

        private void DealDamageToNearByTargets(Transform sourceTransform, float radius)
        {
            //Collider[] colliders = Physics.OverlapSphere(sourceTransform.position, radius);
            ////Play an epic explosion animation
            //for (int i = 0; i < radius; i++)
            //{
            //    if (colliders[i].gameObject.layer == 20)
            //    {
            //        colliders[i].GetComponent<AbstractCharacter>().AddDamage(current_attack.damageAmount);
            //        return;
            //    }
            //}
        }

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
}


