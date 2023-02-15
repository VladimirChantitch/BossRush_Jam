using Boss.save;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Boss.stats;
using Boss.inventory;
using UnityEngine.Events;
using Boss.Upgrades;
using Unity.Mathematics;
using Boss.UI;

namespace player
{
    public class PlayerManager : AbstractCharacter, ISavable
    {
        /// <summary>
        /// The bool stands for if the player died in the boss fight or not 
        /// </summary>
        public UnityEvent<BossRelatedDialogues, bool> onJustCameBack = new UnityEvent<BossRelatedDialogues, bool>();

        [Header("Behaviour Mode")]
        [SerializeField] Status status = Status.HubMode;    
        public void SetStatus(bool isReadyToFight)
        {
            if (isReadyToFight)
            {
                status = Status.BossMode;
            }
            else
            {
                status = Status.HubMode;
            }
        }

        [Header("services ref")]
        [SerializeField] PlayerMovement playerMovement;
        public PlayerUIManager playerUIManager;
        [SerializeField] GuitareUpgradeSystem guitareUpgradeSystem;

        [Header("attack data")]
        [SerializeField] List<AttackData> attackDatas = new List<AttackData>();
        [SerializeField] float multiplier = 1;
        [SerializeField] BossRelatedDialogues currentBossRelatedDialogues;

        [Header("Colliders refs")]
        [SerializeField] PlayerAttackCollider attackCollider;

        private DamageEffect playerDamageEffect;

        public void OpenAttackCollider()
        {
            attackCollider.OpenCollider();
        }
        public void CloseAttackCollider()
        {
            attackCollider.CloseCollider();
        }

        [SerializeField] PlayerAttackCollider_Big attackCollider_Big;
        public bool isAtk2 = false;

        public void OpenAttackCollider_Big()
        {
            attackCollider_Big.OpenCollider();
            isAtk2 = true;  
        }
        public void CloseAttackCollider_Big()
        {
            attackCollider_Big.CloseCollider();
            isAtk2 = false;
        }

        [SerializeField] PlayerTakeDamageCollider playerTakeDamageCollider;
        public void CloseTakeDamageCollide()
        {
            playerTakeDamageCollider.CloseCollider();
        }

        public void OpenTakeDamageCollider()
        {
            playerTakeDamageCollider.OpenCollider();
        }

        [Header("state")]
        [SerializeField] AttackType currentAttackType;

        [HideInInspector] public UnityEvent onPlayerDead = new UnityEvent();

        private void Awake()
        {
            if (guitareUpgradeSystem == null)
            {
                guitareUpgradeSystem = new GuitareUpgradeSystem();
            }
            base.Init();
        }

        [SerializeField] AudioSource audioSource;
        [SerializeField] List<AudioClip> clips = new List<AudioClip>();

        IEnumerator BigSound()
        {
            audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Count)]);
            yield return new WaitForSeconds(0.15f);
        }

        private void Start()
        {
            if (status == Status.BossMode)
            {
                playerDamageEffect = GetComponent<DamageEffect>();

                if (playerMovement == null)
                {
                    playerMovement = FindObjectOfType<PlayerMovement>();
                }
                attackCollider.applyDamageToTarget.AddListener(target => target?.TakeDamageEvent?.Invoke(GetAttackAmount(currentAttackType)));
                playerTakeDamageCollider.takeDamage.AddListener(amount => {
                    AddDamage(amount);
                    playerDamageEffect.Blinking();
                    });
                playerMovement.attackPerformed.AddListener(type => {
                    if (status == Status.BossMode)
                    {
                        currentAttackType = type;
                        if (type == AttackType.normal)
                        {
                            audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Count)]);
                        }
                        else
                        {
                            StartCoroutine(BigSound());
                        }
                    }
                });
                playerMovement.onAttack2Stopped.AddListener(() => {
                    if(status == Status.BossMode)
                    {
                        StopCoroutine(BigSound());
                    }
                });
                playerMovement.StartDash.AddListener(() => CloseTakeDamageCollide());
                playerMovement.EndDash.AddListener(() => OpenTakeDamageCollider());

                playerUIManager.InitPlayerHealth(GetStat(StatsType.health).Value, GetStat(StatsType.health).MaxValue);
                playerUIManager.InitPlayerCombo(GetStat(StatsType.combo).MaxValue);
            }
            else if (status == Status.HubMode)
            {
                if (playerMovement != null)
                {
                    playerMovement.GetComponent<Rigidbody2D>().simulated = false;
                    playerMovement.defaultCollider2D.enabled = false;
                    playerMovement.defaultCollider3D.enabled = false;
                    playerMovement.groundCheckCollider.enabled = false;
                    playerMovement.jumpCollider2D.enabled = false;
                    playerMovement.jumpCollider3D.enabled = false;
                    playerMovement.enabled = false;
                }

                GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(sr =>
                {
                    sr.enabled = false;
                });

                if (GetStat(StatsType.health).Value <= 0)
                {
                    onJustCameBack?.Invoke(currentBossRelatedDialogues, true);
                    SetStat(false, 0, StatsType.Blood);
                    SetStat(false, GetStat(StatsType.health).MaxValue, StatsType.health);
                    FindObjectOfType<HubBloodGauge>()?.Init();
                }
                else
                {
                    onJustCameBack?.Invoke(currentBossRelatedDialogues, false);
                    FindObjectOfType<HubBloodGauge>()?.Init();
                }
            }

            guitareUpgradeSystem.onUpgradesUpdated.AddListener(Upgrades =>
            {
                Debug.Log("add");
                ModifyUpgrades(Upgrades);
            });

            guitareUpgradeSystem.onUpgradeRemoved.AddListener(Upgrade =>
            {
                Debug.Log("remove");
                ClearUpgrade(Upgrade);
            });

            ModifyUpgrades(GetGuitareUpgrades());
        }

        public override void AddDamage(float amount)
        {
            base.AddDamage(amount);
            if (status == Status.BossMode)
            {
                playerUIManager.SetPlayerHealth(GetStat(StatsType.health).Value);

                if (GetStat(StatsType.health).Value <= 0)
                    onPlayerDead?.Invoke();
            }
        }

        public float GetAttackAmount(AttackType type)
        {
            float attack = attackDatas.Where(a => a.GetAttackType() == type).First().GetAttackDamage();
            return attack * GetMultipler(type);
        }

        #region Upgrades
        public List<GuitareUpgrade> GetGuitareUpgrades()
        {
            return guitareUpgradeSystem.GetUpgrades();
        }

        public void RemoveUpgrade(GuitareUpgrade guitareUpgrade)
        {
            guitareUpgradeSystem.RemoveUpgrade(guitareUpgrade);
        }

        public void AddOrModifyUpgrade(GuitareUpgrade guitareUpgrade)
        {
            guitareUpgradeSystem.AddOrModdifyUpgrade(guitareUpgrade);
        }

        private void ModifyUpgrades(List<GuitareUpgrade> upgrades)
        {
            if (status == Status.BossMode)
            {
                upgrades.ForEach(u =>
                {
                    u.upgrades.ForEach(up =>
                    {
                        SetStat(false, up.Value + GetStat(up.StatType).Value, up.StatType);
                        SetStat(true, up.MaxValue + GetStat(up.StatType).MaxValue, up.StatType);
                    });
                });
            }
        }

        public void ClearUpgrades()
        {
            if(status == Status.BossMode)
            {
                GetGuitareUpgrades().ForEach(u =>
                {
                    u.upgrades.ForEach(up =>
                    {
                        SetStat(false, -up.Value + GetStat(up.StatType).Value, up.StatType);
                        SetStat(true, -up.MaxValue + GetStat(up.StatType).MaxValue, up.StatType);
                    });
                });
            }
        }

        private void ClearUpgrade(GuitareUpgrade upgrade)
        {
            if(status == Status.BossMode)
            {
                upgrade.upgrades.ForEach(u =>
                {
                    SetStat(false, GetStat(u.StatType).Value - u.Value, u.StatType);
                    SetStat(true, GetStat(u.StatType).MaxValue - u.MaxValue, u.StatType);
                });
            }
        }

        #endregion

        internal void UseBlood(Action<float> action)
        {
            (float,float) amount = (GetStat(StatsType.Blood).Value, GetStat(StatsType.Blood).MaxValue);
            float rate = amount.Item1 / amount.Item2;
            if (rate > 0.1)
            {
                AddDamage(GetStat(StatsType.health).MaxValue * 0.1f);
                SetStat(false, amount.Item1 - amount.Item2 * 0.1f, StatsType.Blood);
                action.Invoke(rate - 0.1f);
            }
            else
            {
                Debug.Log("Not enought blood to heal player");
            }
        }

        private float GetMultipler(AttackType type)
        {
           if(type == AttackType.normal)
           {
                float multiplier = math.remap(0, GetStat(StatsType.combo).MaxValue, 1, 100, GetStat(StatsType.combo).Value);
                return multiplier;
            }
           else
           {
                float multiplier = math.remap(0, GetStat(StatsType.combo).MaxValue, 1, 666, GetStat(StatsType.combo).Value);
                return multiplier;
            }
 
        }

        #region data
        public DTO GetData()
        {
            ClearUpgrades();   
            List<Stat_DTO> stats_dto = new List<Stat_DTO>();
            stats.ForEach(stat => stats_dto.Add(new Stat_DTO(stat.Value, stat.MaxValue, stat.StatType)));

            Inventory_DTO inventory_DTO = inventory.Save();
            GuitareUpgrade_DTO guitareUpgrade_DTO = guitareUpgradeSystem.Save();
            BossRelated_Dto bossRelated_Dto = currentBossRelatedDialogues.Save();

            return new Player_DTO(stats_dto, inventory_DTO, guitareUpgrade_DTO, bossRelated_Dto);
        }

        public void LoadData(DTO dTO)
        {
            if (dTO is Player_DTO player_dto)
            {
                player_dto.Stats.ForEach(stat =>
                {
                    SetStat(false, stat.value, stat.statType);
                    SetStat(true, stat.maxValue, stat.statType);
                });

                inventory.Load(player_dto.Inventory);
                guitareUpgradeSystem.Load(player_dto.Upgrades);
                currentBossRelatedDialogues = new BossRelatedDialogues(player_dto.BossRelated_Dto);
            }
        }

        internal void ReceiveDialogueData(BossRelatedDialogues bossRelatedDialogues)
        {
            this.currentBossRelatedDialogues = bossRelatedDialogues;
        }
        #endregion
    }

    [Serializable]
    public class AttackData
    {
        public AttackData(AttackType attackType, float damageAmount)
        {
            this.DamageAmount = damageAmount;
            this.AttackType = attackType;
        }

        [SerializeField] AttackType AttackType;
        [SerializeField] float DamageAmount;

        public AttackType GetAttackType()
        {
            return AttackType;
        }

        public float GetAttackDamage()
        {
            return DamageAmount * -1;
        }
    }

    public class Player_DTO : DTO
    {
        public Player_DTO(List<Stat_DTO> stats, Inventory_DTO inventory, GuitareUpgrade_DTO upgrades, BossRelated_Dto bossRelated_Dto)
        {
            Stats = stats;
            Inventory = inventory;
            Upgrades = upgrades;
            BossRelated_Dto = bossRelated_Dto;
        }

        public List<Stat_DTO> Stats { get; private set; }
        public Inventory_DTO Inventory { get; private set; }
        public GuitareUpgrade_DTO Upgrades { get; private set; }
        public BossRelated_Dto BossRelated_Dto { get; private set; }
    }

    public enum AttackType
    {
        normal, 
        big
    }

    public enum Status
    {
        HubMode, 
        BossMode
    }
}

