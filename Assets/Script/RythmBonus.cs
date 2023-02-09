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

    [SerializeField] GameObject particule;
    [SerializeField] Light2D guitareLight;
    [SerializeField] TrailRenderer trail;
    Material trailMaterial;

    [SerializeField] GameObject particule_Atk2;

    private int combo;

    // Start is called before the first frame update
    void Start()
    {
        plusMusic = PlusMusic_DJ.Instance;
        playerManager = GetComponent<PlayerManager>();
        rythmDZ = playerManager.GetStat(Boss.stats.StatsType.rythme_deadZone).Value;

        trailMaterial = trail.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (plusMusic.AllFilesLoaded())
        {
            light.pointLightInnerRadius = Mathf.Clamp(math.remap(rythmDZ, rythmDZ + 0.25f, 0.0f, 2.0f, plusMusic.TimeNextBeat()), 0.0f, 5.0f);
            light.pointLightOuterRadius = math.remap(0.0f, 1.0f, 0.0f, 20.0f, plusMusic.TimeNextBeat());
            light.intensity = Mathf.Clamp(math.remap(0.0f, rythmDZ + 0.25f, 5.0f, 0.0f, plusMusic.TimeNextBeat()), 0.0f, 5.0f);

            if (!onTime)
            {
                light.color = Color.Lerp(Color.red, Color.white, plusMusic.TimeNextBeat());
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
                    combo = 0;
                    playerManager.playerUIManager.SetPlayerCombo(combo, 50);
                }

                ComboEffect_Atk1();
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (plusMusic.TimeNextBeat() < rythmDZ)
                {
                    StartCoroutine(OnTime());
                }
                else
                {
                    light.color = Color.white;
                    combo = 0;
                    playerManager.playerUIManager.SetPlayerCombo(combo, 50);
                }

                ComboEffect_Atk2();
            }
        }
    }

    IEnumerator OnTime()
    {
        onTime = true;
        combo++;
        playerManager.playerUIManager.SetPlayerCombo(combo, 50);
        light.color = color;
        //light.pointLightInnerRadius = 0.25f;
        //light.pointLightOuterRadius = 10.0f;
        //light.intensity = 1.5f;
        yield return new WaitForSeconds(0.15f);
        onTime = false;
        //light.color = Color.white;
    }

    private void ComboEffect_Atk1()
    {
        ParticleCombo();
        LightCombo();
        TrailCombo();
    }

    private void ComboEffect_Atk2()
    {
        WaveCombo();
        LightCombo();
        TrailCombo();
    }

    private void ParticleCombo()
    {
        var ps = particule.GetComponent<ParticleSystem>();

        var main = ps.main;
        main.startSpeedMultiplier = combo;

        var em = ps.emission;
        ParticleSystem.Burst burst = em.GetBurst(0);
        burst.count = math.remap(0.0f, 20f, 0.0f, 1000f, (float)combo);
        em.SetBurst(0, burst);

        ps.Play();
    }

    private void LightCombo()
    {
        guitareLight.intensity = math.remap(0.0f, 20f, 0.0f, 200f, (float)combo);
    }

    private void TrailCombo()
    {
        trail.time = math.remap(0.0f, 20f, 0.01f, 0.2f, (float)combo);
        trailMaterial.SetFloat("_Combo", combo);
    }

    private void WaveCombo()
    {
        var ps = particule_Atk2.GetComponent<ParticleSystem>();

        var main = ps.main;

        var solt = ps.sizeOverLifetime;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.0f);
        curve.AddKey(1.0f, combo);

        solt.size = new ParticleSystem.MinMaxCurve(1f, curve);

        ps.Play();
    }
}
