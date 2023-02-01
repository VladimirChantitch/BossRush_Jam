using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using player;
using Unity.Mathematics;

public class RythmBonus : MonoBehaviour
{
    private PlusMusic_DJ plusMusic;

    private PlayerManager playerManager;
    private float rythmDZ;

    public Light2D light;
    public Color color;
    private bool onTime;

    // Start is called before the first frame update
    void Start()
    {
        plusMusic = PlusMusic_DJ.Instance;
        playerManager = GetComponent<PlayerManager>();
        rythmDZ = playerManager.GetStat(Boss.stats.StatsType.rythme_deadZone).Value;

    }

    // Update is called once per frame
    void Update()
    {
        if (plusMusic.AllFilesLoaded())
        {
            if(!onTime)
            {
                light.pointLightInnerRadius = Mathf.Clamp(math.remap(rythmDZ, rythmDZ + 0.25f, 0.0f, 2.0f, plusMusic.TimeNextBeat()), 0.0f, 5.0f);
                light.pointLightOuterRadius = math.remap(0.0f, 1.0f, 0.0f, 20.0f, plusMusic.TimeNextBeat());
                light.intensity = Mathf.Clamp(math.remap(0.0f, rythmDZ + 0.25f, 5.0f, 0.0f, plusMusic.TimeNextBeat()), 0.0f, 5.0f);
                Debug.Log($"<color=green> {plusMusic.TimeNextBeat()} remaped {light.pointLightInnerRadius} </color>");
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (plusMusic.TimeNextBeat() < rythmDZ)
                {
                    StartCoroutine(OnTime());
                }
                else
                {
                    light.color = Color.white;
                }
            }
        }
    }

    IEnumerator OnTime()
    {
        onTime = true;
        light.color = color;
        light.pointLightInnerRadius = 0.25f;
        light.pointLightOuterRadius = 20.0f;
        light.intensity = 1.0f;
        yield return new WaitForSeconds(0.1f);
        onTime = false;
        light.color = Color.white;
    }
}
