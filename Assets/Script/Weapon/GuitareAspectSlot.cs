using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Upgrades
{
    public class GuitareAspectSlot : MonoBehaviour
    {
        public UpgradePartType type;
        [SerializeField] SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        internal void LoadNewUpgrade(Sprite sprite)
        {
            spriteRenderer.sprite = sprite; 
        }

        //TODO --- adapt acollier depending on the sprite
    }
}

