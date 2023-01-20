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
        public bool isDying;

        public bool isLeftDestroyed;
        public bool isRightDestroyed;
        public int attackPhaseCounter;

        [Header("Attack Logic")]
        [SerializeField] List<AttackData> attackDatas = new List<AttackData>(4);
        [SerializeField] AttackData current_attack;

        [Header("Collider reference")]
        [SerializeField] C_Boss_1_AttackCollider rightCollider;
        public void OpenRightAttackCollider()
        {
            rightCollider.OpenCollider();
        }
        public void CloseRightAttackCollider()
        {
            rightCollider.CloseCollider();
        }
        [SerializeField] C_Boss_1_AttackCollider leftCollider;
        public void OpenLeftAttackCollider()
        {
            leftCollider.OpenCollider();
        }
        public void CloseLeftAttackCollider()
        {
            leftCollider.CloseCollider();
        }
        [SerializeField] Boss_1_Vulnerability right_vulnerability;
        [SerializeField] Boss_1_Vulnerability left_vulnerability;
        [SerializeField] Boss_1_Vulnerability selectedHand;
        public void OpenSelectedVulnerabilitCollider()
        {
            selectedHand.OpenCollider();
        }
        public void CloseSlectedVulnerabilityCollider()
        {
            selectedHand.CloseCollider();
        }
        [SerializeField] Boss_1_HeadVUlnerability headVulnerability;
        public void OpenHeadVulnerabilitCollider()
        {
            headVulnerability.OpenCollider();
        }
        public void CloseHeadVulnerabilityCollider()
        {
            headVulnerability.CloseCollider();
        }

        [Header("Animations")]
        [SerializeField] AnimationClip Idle;
        [SerializeField] AnimationClip Arriving;
        [SerializeField] AnimationClip RightVulnerability;
        [SerializeField] AnimationClip LeftVulnerability;
        [SerializeField] AnimationClip HeadVulnerabilityAnim;
        [SerializeField] AnimationClip Death;


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
                if (attackPhaseCounter >= 2)
                {
                    EnterRandomVulnerability();
                    attackPhaseCounter = 0;
                }
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
                if (isLeftDestroyed)
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
                if (isRightDestroyed)
                {
                    state = MaxiBestOfState.Vulnerability_leftHand;
                }
                else
                {
                    state = MaxiBestOfState.Vulnerability_rightHand;
                }
            }
            isVulnerable = false;
        }

        public void EnterLeftVulnerability()
        {
            EnterVulnerabilityState(true);
            isVulnerable = false;
        }

        public void EnterRightVulnerability()
        {
            EnterVulnerabilityState(false);
            isVulnerable = false;
        }

        public void EnterRandomVulnerability()
        {
            EnterVulnerabilityState((int)Time.time % 2 == 0);
            isVulnerable = false;
        }

        private void Vulnerability(bool isLeft)
        {
            if (isVulnerable == false)
            {
                isVulnerable = true;
                if (isLeftDestroyed)
                {
                    selectedHand = right_vulnerability;
                }
                else if (isRightDestroyed)
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
                selectedHand.parent.SetActive(false);
                if (left_vulnerability.isDestroyed || isLeftDestroyed)
                {
                    isLeftDestroyed = true;
                }
                if (right_vulnerability.isDestroyed || isRightDestroyed)
                {
                    isRightDestroyed = true;
                }

                if(isLeftDestroyed && isRightDestroyed)
                {
                    EnterHeadVulnerability(); 
                }
                else
                {
                    EnterAwaitingState();
                }
            }
            else
            {
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
                headVulnerability.vulnerabilirabilityDestroyed.AddListener(amount => AddDamage(amount));
            }
            
            // Plays an animation tu show that the head is vulnerable 
            // the player can hit and inflict damages to the boss
        }

        public void HeadVulnerabilityFinished()
        {
            state = MaxiBestOfState.Awaiting;
            if (Health <= 0.5 * MaxHealth)
            {
                isPhaseTwo = true;
                right_vulnerability.Reactivate();
                left_vulnerability.Reactivate();
                // Do something epic
            }
            else if (Health <= 0)
            {
                EnterDyingState();
                // the boss goes to the Dying Phase -- shouldnt be launched caudsed well hum its alreaduy handed by abstrct character 
            }
            else
            {
                right_vulnerability.Reactivate();
                left_vulnerability.Reactivate();
                isLeftDestroyed = false;
                isRightDestroyed = false;
                EnterAwaitingState();
                //Play hand Recovery animation;
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
        public void EnterDyingState()
        {
            state = MaxiBestOfState.Dying;
            isDying = false;

        }

        private void Dying()
        {
            if (isDying == false)
            {
                isDying = true;
                animator.PlayTargetAnimation(false, Death.name, 1);
                // Stops the fight, the player gets to a lock position 
                // Explosion 
                // Three item are displayed 
                // The player is unlocked and goes on them to collect
                // Once collect is done the player goes back to the Hub
            }
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


