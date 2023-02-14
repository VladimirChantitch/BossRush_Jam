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
    [SerializeField] GameObject particule_Atk2_2;

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
            light.pointLightInnerRadius = Mathf.Clamp(math.remap(rythmDZ, rythmDZ + 0.25f, 0.0f, 2.0f, plusMusic.TimeNextBeat()), 0.0f, 5.0f);
            light.pointLightOuterRadius = math.remap(0.0f, 1.0f, 0.0f, 20.0f, plusMusic.TimeNextBeat());
            light.intensity = Mathf.Clamp(math.remap(0.0f, rythmDZ + 0.25f, 5.0f, 0.0f, plusMusic.TimeNextBeat()), 0.0f, 5.0f);

            if (!onTime)
            {
                light.color = Color.Lerp(Color.red, Color.white, plusMusic.TimeNextBeat());
            }
    }

    public void BaseAttack(int type)
    {
        if (plusMusic?.TimeNextBeat() < rythmDZ)
        {
            Debug.Log(playerManager.GetStat(Boss.stats.StatsType.combo).Value);
            StartCoroutine(OnTime());
        }
        else
        {
            light.color = Color.white;
            playerManager.SetStat(false, 0, Boss.stats.StatsType.combo);
            playerManager.playerUIManager.SetPlayerCombo(playerManager.GetStat(Boss.stats.StatsType.combo).Value, playerManager.GetStat(Boss.stats.StatsType.combo).MaxValue);
        }

        if (type == 0)
            ComboEffect_Atk1();
        else
            ComboEffect_Atk2();
    }

    IEnumerator OnTime()
    {
        onTime = true;
        playerManager.SetStat(false, playerManager.GetStat(Boss.stats.StatsType.combo).Value + 1, Boss.stats.StatsType.combo);
        playerManager.playerUIManager.SetPlayerCombo(playerManager.GetStat(Boss.stats.StatsType.combo).Value, playerManager.GetStat(Boss.stats.StatsType.combo).MaxValue);
        light.color = color;
        yield return new WaitForSeconds(0.15f);
        onTime = false;
    }

    private void ComboEffect_Atk1()
    {
        StopParticule(particule_Atk2.GetComponent<ParticleSystem>());
        ParticleCombo(particule.GetComponent<ParticleSystem>());
        LightCombo();
        TrailCombo();
    }

    private void ComboEffect_Atk2()
    {
        StopParticule(particule.GetComponent<ParticleSystem>());
        ParticleCombo(particule_Atk2_2.GetComponent<ParticleSystem>());
        WaveCombo();
    }

    private void ParticleCombo(ParticleSystem ps)
    {
        var main = ps.main;
        main.startSpeedMultiplier = playerManager.GetStat(Boss.stats.StatsType.combo).Value;

        var em = ps.emission;
        ParticleSystem.Burst burst = em.GetBurst(0);
        burst.count = math.remap(0.0f, 20f, 0.0f, 1000f, playerManager.GetStat(Boss.stats.StatsType.combo).Value);
        em.SetBurst(0, burst);

        ps.Play();
    }

    private void LightCombo()
    {
        guitareLight.intensity = math.remap(0.0f, 20f, 0.0f, 200f, playerManager.GetStat(Boss.stats.StatsType.combo).Value);
    }

    private void TrailCombo()
    {
        trail.time = math.remap(0.0f, 20f, 0.01f, 0.2f, playerManager.GetStat(Boss.stats.StatsType.combo).Value);
        trailMaterial.SetFloat("_Combo", playerManager.GetStat(Boss.stats.StatsType.combo).Value);
    }

    private void WaveCombo()
    {
        var ps = particule_Atk2.GetComponent<ParticleSystem>();

        var main = ps.main;

        var solt = ps.sizeOverLifetime;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.0f);
        curve.AddKey(1.0f, playerManager.GetStat(Boss.stats.StatsType.combo).Value);

        solt.size = new ParticleSystem.MinMaxCurve(1f, curve);

        ps.Play();
    }

    private void StopParticule(ParticleSystem ps)
    {
        ps.Stop();
    }
}
