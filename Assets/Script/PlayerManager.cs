using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace player
{
    public class PlayerManager : AbstractCharacter
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
        }

        public float GetAttackAmount(AttackType type)
        {
            float attack = attackDatas.Where(a => a.GetAttackType() == type).First().GetAttackDamage();
            return attack * multiplier;
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

    public enum AttackType
    {
        normal, 
        big
    }
}

