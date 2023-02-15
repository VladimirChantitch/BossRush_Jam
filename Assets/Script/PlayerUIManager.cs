using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] RectTransform lifeBG;
    [SerializeField] RectTransform life;

    [SerializeField] RectTransform comboBG;
    [SerializeField] RectTransform combo;
    private Material combo_mat;

    [SerializeField] GameObject[] dashes;


    // Start is called before the first frame update
    void Start()
    {
        combo_mat = combo.GetComponent<Image>().material;
    }

    public void SetDash(int max)
    {
        for (int i = 0; i < dashes.Length; i++)
        {
            if(i < max)
            {
                dashes[i].SetActive(true); 
            }
            else
                dashes[i].SetActive(false);
        }
    }

    public void InitPlayerHealth(float currentHealth, float maxHealth)
    {
        life.sizeDelta = new Vector2 (currentHealth,life.sizeDelta.y);
        lifeBG.sizeDelta = new Vector2 (maxHealth,lifeBG.sizeDelta.y);
    }

    public void SetPlayerHealth(float amount)
    {
        life.sizeDelta = new Vector2(amount, life.sizeDelta.y);
    }

    public void InitPlayerCombo(float maxCombo)
    {
        combo.sizeDelta = new Vector2(0, combo.sizeDelta.y);
        comboBG.sizeDelta = new Vector2(maxCombo * 10, combo.sizeDelta.y);
    }

    public void SetPlayerCombo(float amount, float maxCombo)
    {
        combo.sizeDelta = new Vector2(amount * 10, combo.sizeDelta.y);
        float expo = math.remap(0, maxCombo, -2, 3, amount);
        combo_mat.SetColor("_Color", Color.white * math.pow(2f, expo));
    }
}
