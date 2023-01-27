using Boss.inventory;
using UnityEngine.UIElements;
using UnityEditor;

public class UI_ItemSlot : VisualElement
{
    public new class UxmlFactory : UxmlFactory<UI_ItemSlot>
    {

    }

    public UI_ItemSlot()
    {
        this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI/Boss.uss"));
        AddToClassList("Slot");
    }

    public void Init(AbstractItem Item)
    {
        this.Item = Item;

    }

    AbstractItem Item;

}
