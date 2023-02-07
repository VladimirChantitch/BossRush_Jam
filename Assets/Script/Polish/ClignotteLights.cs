using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ClignotteLights : MonoBehaviour
{
    public Light2D currentLight;
    [SerializeField] float speed;
    bool isDecreasing;

    void Start()
    {
        currentLight = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (isDecreasing)
        {
            currentLight.intensity -= Time.deltaTime * speed;
            if (currentLight.intensity <= 0)
            {
                isDecreasing = false;
            }
        }
        else
        {
            currentLight.intensity += Time.deltaTime * speed;
            if (currentLight.intensity >= 2)
            {
                currentLight.intensity = 2;
                isDecreasing = true;
            }
        }
    }
}
