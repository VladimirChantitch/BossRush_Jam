using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ClignotteLights : MonoBehaviour
{
    public Light2D currentLight;
    [SerializeField] float speed;
    bool isDecreasing;
    [SerializeField] float min = 0;
    [SerializeField] float max = 2;

    void Start()
    {
        currentLight = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (isDecreasing)
        {
            currentLight.intensity -= Time.deltaTime * speed;
            if (currentLight.intensity <= min)
            {
                isDecreasing = false;
            }
        }
        else
        {
            currentLight.intensity += Time.deltaTime * speed;
            if (currentLight.intensity >= max)
            {
                currentLight.intensity = max;
                isDecreasing = true;
            }
        }
    }
}
