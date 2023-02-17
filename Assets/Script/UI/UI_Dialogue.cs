using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UI_Dialogue : VisualElement
{
    public new class UxmlFactory : UxmlFactory<UI_Dialogue, UxmlTraits> { }

    public UnityEvent onFinished = new UnityEvent();

    Label label;

    public bool isOverrided = false;

    public void Init()
    {
        label = this.Q<Label>("Dialogue");
    }

    public IEnumerator SetNewDialogue(string txt, bool overrider = false)
    {
        isOverrided = overrider;
        if (txt == null) txt = "Well Lorem Ipsum I guess";
        this.label.text = "";
        foreach (var t in txt)
        {
            label.text += t;
            if (t.ToString() == " ")
            {
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(4f);
        onFinished?.Invoke();
        isOverrided = false;
    } 
}
