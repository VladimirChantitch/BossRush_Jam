using Boss.inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


namespace Boss.Upgrades.UI
{
    public class UI_GuitareSlotUpgrades : UI_ItemSlot
    {
        public new class UxmlFactory : UxmlFactory<UI_GuitareSlotUpgrades, UxmlTraits> { }

        public UnityEvent<GuitareUpgrade> onDisupgraded = new UnityEvent<GuitareUpgrade>();

        public UI_GuitareSlotUpgrades() : base()
        {

        }

        public UpgradePartType type { get; private set; }

        /// <summary>
        /// Sets up the slot with the right item ref
        /// </summary>
        /// <param name="item"></param>
        public override void Init(AbstractItem item)
        {
            if (Item != null)
            {
                onDisupgraded?.Invoke(Item as GuitareUpgrade);
                Clean();
            }

            base.Init(item);
            this.type = type;
        }

        protected override void BindEvents()
        {
            this.clicked += () =>
            {
                onDisupgraded?.Invoke(Item as GuitareUpgrade);
                Clean();
            };
        }
    }
}

