using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class BossUIManager : MonoBehaviour
{
    [SerializeField] RectTransform lifeBG;
    [SerializeField] RectTransform life;

    public void InitBossHealth(float currentHealth, float maxHealth)
    {
        life.sizeDelta = new Vector2(1500, life.sizeDelta.y);
        lifeBG.sizeDelta = new Vector2(1500, lifeBG.sizeDelta.y);
    }

    public void SetBossHealth(float currentHealth, float maxHealth)
    {
        float newHealth = math.remap(0, maxHealth, 0, 1500, currentHealth);
        life.sizeDelta = new Vector2(newHealth, life.sizeDelta.y);
    }
}
