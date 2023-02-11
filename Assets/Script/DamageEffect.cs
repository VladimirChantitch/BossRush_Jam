using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] Material mat;

    public float spriteBlinkingTimer = 0.0f;
    public float spriteBlinkingMiniDuration = 0.1f;
    public float spriteBlinkingTotalTimer = 0.0f;
    public float spriteBlinkingTotalDuration = 1.0f;
    public bool startBlinking = false;

    public Color color;
    public float expo;

    private void Start()
    {
        mat.SetColor("_Color", Color.white);
    }

    void Update()
    {
        if (startBlinking == true)
        {
            SpriteBlinkingEffect();
        }
    }

    public void Blinking()
    {
        startBlinking = true;
    }


    private void SpriteBlinkingEffect()
    {
        spriteBlinkingTotalTimer += Time.deltaTime;
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            startBlinking = false;
            spriteBlinkingTotalTimer = 0.0f;
            mat.SetColor("_Color", Color.white);
            return;
        }

        spriteBlinkingTimer += Time.deltaTime;
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            spriteBlinkingTimer = 0.0f;
            if (mat.GetColor("_Color") == Color.white)
            {
                mat.SetColor("_Color", color * math.pow(2f, expo));
            }
            else
            {
                mat.SetColor("_Color", Color.white);
            }

        }
    }
}
