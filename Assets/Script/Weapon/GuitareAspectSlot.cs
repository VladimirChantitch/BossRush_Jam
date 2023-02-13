using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss.Upgrades
{
    public class GuitareAspectSlot : MonoBehaviour
    {
        public UpgradeGraphicsRefType type;
        [SerializeField] SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        internal void EnableUpgrade()
        {
            spriteRenderer.enabled = true;
        }

        public void DisableUpgrade()
        {
            spriteRenderer.enabled = false;
        }

        //TODO --- adapt acollier depending on the sprite
    }
}

