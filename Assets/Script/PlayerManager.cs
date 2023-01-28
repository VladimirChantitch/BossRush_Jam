using Boss.save;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Boss.stats;
using Boss.inventory;

namespace player
{
    public class PlayerManager : AbstractCharacter, ISavable
    {
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

        private void Start()
        {
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
            }
            else if (status == Status.HubMode)
            {
                playerMovement.GetComponent<Rigidbody2D>().simulated = false;
                playerMovement.defaultCollider2D.enabled = false;
                playerMovement.defaultCollider3D.enabled = false;
                playerMovement.groundCheckCollider.enabled = false;
                playerMovement.jumpCollider2D.enabled = false;
                playerMovement.jumpCollider3D.enabled = false;
                playerMovement.enabled = false;
                GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(sr =>
                {
                    sr.enabled = false;
                });
            }
        }

        public float GetAttackAmount(AttackType type)
        {
            float attack = attackDatas.Where(a => a.GetAttackType() == type).First().GetAttackDamage();
            return attack * multiplier;
        }

        public DTO GetData()
        {
            List<Stat_DTO> stats_dto = new List<Stat_DTO>();
            Inventory_DTO inventory_DTO = inventory.Save();
            stats.ForEach(stat => stats_dto.Add(new Stat_DTO(stat.Value, stat.MaxValue, stat.StatType)));

            return new Player_DTO(stats_dto, inventory_DTO);
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
            }
        }
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
            return DamageAmount;
        }
    }

    public class Player_DTO : DTO
    {
        public Player_DTO(List<Stat_DTO> stats, Inventory_DTO inventory)
        {
            Stats = stats;
            Inventory = inventory;
        }

        public List<Stat_DTO> Stats { get; private set; }
        public Inventory_DTO Inventory { get; private set; }
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

