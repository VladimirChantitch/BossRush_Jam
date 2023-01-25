using Boss.save;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace player
{
    public class PlayerManager : AbstractCharacter, ISavable
    {
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

        public float GetAttackAmount(AttackType type)
        {
            float attack = attackDatas.Where(a => a.GetAttackType() == type).First().GetAttackDamage();
            return attack * multiplier;
        }

        public DTO GetData()
        {
            return new Player_DTO(Health, MaxHealth);
        }

        public void LoadData(DTO dTO)
        {
            if (dTO is Player_DTO player_dto)
            {
                SetHealth(player_dto.CurrentHealth);
                SetMaxHealth(player_dto.MaxHealth);
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
        public Player_DTO(float CurrentHealth, float MaxHealth)
        {
            this.CurrentHealth = CurrentHealth;
            this.MaxHealth = MaxHealth;
        }

        public float CurrentHealth;
        public float MaxHealth;
    }

    public enum AttackType
    {
        normal, 
        big
    }
}

