using Boss.Upgrades.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UI_Dialogue : VisualElement
{
    public new class UxmlFactory : UxmlFactory<UI_Dialogue, UxmlTraits> { }

    public UnityEvent onFinished = new UnityEvent();

    Label label;

    public void Init()
    {
        label = this.Q<Label>("Label");
    }

    public async void SetNewDialogue(string txt)
    {
        this.label.text = " ";
        this.label.text.ToList().ForEach(async t =>
        {
            label.text += t;
            await Task.Delay(200);
        });
        await Task.Delay(1500);
        onFinished.Invoke();
    }
}
