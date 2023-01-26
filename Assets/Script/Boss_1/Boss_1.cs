using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Boss.stats;

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

        [Header("Attack Logic")]
        [SerializeField] List<AttackData> attackDatas = new List<AttackData>(4);
        [SerializeField] AttackData current_attack;

        [Header("Attack Collider references")]
        [SerializeField] C_Boss_1_AttackCollider rightAttackCollider;
        public void OpenRightAttackCollider()
        {
            rightAttackCollider.OpenCollider();
        }
        public void CloseRightAttackCollider()
        {
            rightAttackCollider.CloseCollider();
        }
        [SerializeField] C_Boss_1_AttackCollider leftAttackCollider;
        public void OpenLeftAttackCollider()
        {
            leftAttackCollider.OpenCollider();
        }
        public void CloseLeftAttackCollider()
        {
            leftAttackCollider.CloseCollider();
        }
        [SerializeField] Boss_1_ShootLaser rightLaser;
        public void OpenRightLaser()
        {
            rightLaser.OpenLaser();
        }
        public void CloseLaser()
        {
            rightLaser.CloseLaser();
        }
        [SerializeField] Boss_1_ShootLaser leftLaser;
        public void OpenLeftLaser()
        {
            leftLaser.OpenLaser();
        }
        public void CloseLeftLaser()
        {
            leftLaser.CloseLaser();
        }

        [Header("Vulnerabilities Collider references")]
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
        [SerializeField] string Idle;
        [SerializeField] string Arriving;
        [SerializeField] string RightVulnerability;
        [SerializeField] string LeftVulnerability;
        [SerializeField] string HeadVulnerabilityAnim;
        [SerializeField] string Death;

        private void Start()
        {
            animator = GetComponent<Boss_1_Animator>();
            initBoss();

            rightAttackCollider.applyDamageToTarget.AddListener(target => target?.takeDamage?.Invoke(GetCurrentDamage()));
            leftAttackCollider.applyDamageToTarget.AddListener(target => target?.takeDamage?.Invoke(GetCurrentDamage()));
            rightLaser.laserLeftCollider.applyDamageToTarget.AddListener(target => target?.takeDamage?.Invoke(GetCurrentDamage()));
            rightLaser.laserRightCollider.applyDamageToTarget.AddListener(target => target?.takeDamage?.Invoke(GetCurrentDamage()));
            leftLaser.laserLeftCollider.applyDamageToTarget.AddListener(target => target?.takeDamage?.Invoke(GetCurrentDamage()));
            leftLaser.laserRightCollider.applyDamageToTarget.AddListener(target => target?.takeDamage?.Invoke(GetCurrentDamage()));

            right_vulnerability.vulnerabilirabilityDestroyed.AddListener(() => VulnerabilityFinished(true));
            left_vulnerability.vulnerabilirabilityDestroyed.AddListener(() => VulnerabilityFinished(true));

            headVulnerability.TakeDamageEvent.AddListener(amount => {
                amount *= 0.1f;
                if (!isAwaiting)
                {
                    if (isLeftDestroyed)
                    {
                        amount *= 2;
                    }
                    if (isRightDestroyed)
                    {
                        amount *= 2;
                    }
                    if (isAttacking)
                    {
                        amount *= 2;
                    }
                    if (state == MaxiBestOfState.Vulnerability_Head)
                    {
                        amount *= 4;
                    }
                }
                AddDamage(amount);
            });
            left_vulnerability.TakeDamageEvent.AddListener(amount => AddDamage(amount / 4));
            right_vulnerability.TakeDamageEvent.AddListener(amount => AddDamage(amount / 4));
        }

        public float GetCurrentDamage()
        {
            if (current_attack == null)
            {
                return 0;
            }
            return current_attack.damageAmount;
        }

        private void Update()
        {
            if (GetStat(StatsType.health).Value <= 0)
            {
                EnterDyingState();
            }
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
                    isAwaiting = false;
                    Attacking();
                    break;
                case MaxiBestOfState.Vulnerability_rightHand:
                    isAwaiting = false;
                    Vulnerability(false);
                    break;
                case MaxiBestOfState.Vulnerability_leftHand:
                    isAwaiting = false;
                    Vulnerability(true);
                    break;
                case MaxiBestOfState.Vulnerability_Head:
                    isAwaiting = false;
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
                animator.PlayTargetAnimation(true, Arriving, 1f);
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
                animator.PlayTargetAnimation(false, Idle, 1f);
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

                List<AttackData> attackData_phase = attackDatas.Where(a => a.isSecondPhase == isPhaseTwo).ToList();

                current_attack = attackData_phase[Random.Range(0, attackData_phase.Count)];
                animator.PlayTargetAnimation(false, current_attack.associatedAnimation.name, 1f);
            }
        }

        public void AttackFinished()
        {
            EnterVulnerabilityState();
        }
        #endregion

        #region Vulnerability_Hand
        public void EnterVulnerabilityState()
        {
            if (isLeftDestroyed && isRightDestroyed)
            {
                EnterHeadVulnerability();
            }

            bool isLeft = 0 == Random.Range(0, 2);
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

        private void Vulnerability(bool isLeft)
        {
            if (isLeftDestroyed && isRightDestroyed)
            {
                isVulnerable = true;
                EnterHeadVulnerability();
            }

            if (isVulnerable == false)
            {
                isVulnerable = true;

                if (isLeft)
                {
                    selectedHand = left_vulnerability;
                    animator.PlayTargetAnimation(false, LeftVulnerability, 1f);
                }
                else
                {
                    selectedHand = right_vulnerability;
                    animator.PlayTargetAnimation(false, RightVulnerability, 1f);
                }
            }
        }

        public void VulnerabilityFinished()
        {
            VulnerabilityFinished(false);
        }

        public void VulnerabilityFinished(bool isDestroyed)
        {
            if (left_vulnerability.isDestroyed)
            {
                isLeftDestroyed = true;
            }
            if (right_vulnerability.isDestroyed)
            {
                isRightDestroyed = true;
            }

            if(isLeftDestroyed && isRightDestroyed)
            {
                EnterHeadVulnerability(); 
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
                animator.PlayTargetAnimation(false, HeadVulnerabilityAnim, 1f);
            }
        }

        public void ExitHeadVulnerability()
        {
            if (GetStat(StatsType.health).Value <= 0.5 * GetStat(StatsType.health).MaxValue)
            {
                isPhaseTwo = true;
                right_vulnerability.Reactivate();
                left_vulnerability.Reactivate();
                isLeftDestroyed = false;
                isRightDestroyed = false;
                EnterAwaitingState();
            }
            else if (GetStat(StatsType.health).Value <= 0)
            {
                EnterDyingState();
            }
            else
            {
                right_vulnerability.Reactivate();
                left_vulnerability.Reactivate();
                isLeftDestroyed = false;
                isRightDestroyed = false;
                EnterAwaitingState();
            }
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
                animator.PlayTargetAnimation(false, Death, 0.25f);
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


