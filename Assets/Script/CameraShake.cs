using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin channelPerlin;
    private float startingAmplitude;


    private float shakeTimer;
    private float shakeTimerTotal;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float amplitude, float time, float frequency)
    {
        channelPerlin.m_AmplitudeGain = amplitude;
        channelPerlin.m_FrequencyGain = frequency;

        startingAmplitude = amplitude;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            channelPerlin.m_AmplitudeGain = Mathf.Lerp(startingAmplitude, 0f, 1- shakeTimer / shakeTimerTotal);
        }
    }
}
