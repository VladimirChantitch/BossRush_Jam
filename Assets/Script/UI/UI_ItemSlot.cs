using Boss.inventory;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.Events;
using System;

public class UI_ItemSlot : Button
{
    public new class UxmlFactory : UxmlFactory<UI_ItemSlot>
    {

    }

    public UI_ItemSlot()
    {
        this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI/Boss.uss"));
        AddToClassList("Slot");
        Init();
    }

    public UnityEvent<AbstractItem> ItemSelected = new UnityEvent<AbstractItem>();
    public UnityEvent<AbstractItem> ItemDeselected = new UnityEvent<AbstractItem>();
    public bool isSelected;
    string overriderClass;
    AbstractItem Item;


    private void Init()
    {
        BindEvents();
    }

    public void Init(AbstractItem Item)
    {
        this.Item = Item;

        InitIcon();
    }

    public void SetOverriderClass(string name)
    {
        RemoveOverriderClass();
        RemoveFromClassList("SlotSelected");
        overriderClass = name;
        AddToClassList(overriderClass);
    }

    public void RemoveOverriderClass()
    {
        RemoveFromClassList(overriderClass);
    }

    private void BindEvents()
    {
        this.clicked += () =>
        {
            isSelected = !isSelected;
            if (isSelected == true)
            {
                ItemSelected?.Invoke(Item);
                ToggleInClassList("Slot");
                ToggleInClassList("SlotSelected");
            }
            else
            {
                ItemDeselected?.Invoke(Item);
                ToggleInClassList("Slot");
                ToggleInClassList("SlotSelected");
            }

        };
    }

    public void Clean()
    {
        RemoveFromClassList("SlotSelected");
        Item = null;
        style.backgroundImage = null;
        AddToClassList("Slot");
        isSelected = false;
    }

    private void InitIcon()
    {
        StyleBackground styleBackground = new StyleBackground();
        Background background = new Background();
        background.sprite = Item.Icon;
        styleBackground.value = background;
        style.backgroundImage = styleBackground;
    }
}
