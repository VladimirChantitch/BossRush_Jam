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

namespace player
{
    public class PlayerManager : AbstractCharacter, ISavable
    {
        public UnityEvent onJustRevived = new UnityEvent();

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

        [Header("Colliders refs")]
        [SerializeField] PlayerAttackCollider attackCollider;

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
        }

        private void Start()
        {
            base.Init();
            if (status == Status.BossMode)
            {
                if (playerMovement == null)
                {
                    playerMovement = FindObjectOfType<PlayerMovement>();
                }
                attackCollider.applyDamageToTarget.AddListener(target => target?.TakeDamageEvent?.Invoke(GetAttackAmount(currentAttackType)));
                playerTakeDamageCollider.takeDamage.AddListener(amount => AddDamage(amount));
                playerMovement.attackPerformed.AddListener(type => currentAttackType = type);
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
            }

            guitareUpgradeSystem.onUpgradesUpdated.AddListener(Upgrades =>
            {
                LoadUpgrades(Upgrades);
            });

            LoadUpgrades(guitareUpgradeSystem.GetUpgrades());

            if (GetStat(StatsType.health).Value <= 0)
            {
                onJustRevived?.Invoke();
                SetStat(false, 0, StatsType.Blood);
                SetStat(false, GetStat(StatsType.health).MaxValue, StatsType.health);
            }
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
            return attack * multiplier;
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

        private void LoadUpgrades(List<GuitareUpgrade> upgrades)
        {
            Debug.Log("<color=red> Not implemented </color>");
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

        private float GetMultipler()
        {
            throw new NotImplementedException();
        }

        #region data
        public DTO GetData()
        {
            List<Stat_DTO> stats_dto = new List<Stat_DTO>();
            stats.ForEach(stat => stats_dto.Add(new Stat_DTO(stat.Value, stat.MaxValue, stat.StatType)));

            Inventory_DTO inventory_DTO = inventory.Save();
            GuitareUpgrade_DTO guitareUpgrade_DTO = guitareUpgradeSystem.Save();

            return new Player_DTO(stats_dto, inventory_DTO, guitareUpgrade_DTO);
        }

        public void LoadData(DTO dTO)
        {
            if (dTO is Player_DTO player_dto)
            {
                player_dto.Stats.ForEach(stat =>
                {
                    SetStat(false, stat.value, stat.statType);
                    SetStat(false, stat.maxValue, stat.statType);
                });

                inventory.Load(player_dto.Inventory);
                guitareUpgradeSystem.Load(player_dto.Upgrades);
            }
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
        public Player_DTO(List<Stat_DTO> stats, Inventory_DTO inventory, GuitareUpgrade_DTO upgrades)
        {
            Stats = stats;
            Inventory = inventory;
            Upgrades = upgrades;
        }

        public List<Stat_DTO> Stats { get; private set; }
        public Inventory_DTO Inventory { get; private set; }
        public GuitareUpgrade_DTO Upgrades { get; private set; }
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

