using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlusMusic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private Animator animator;
    [SerializeField] GameObject uI;
    private PlusMusic_DJ plusMusic;
    private bool isLoaded;

    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerSnapshot[] snapshots;
    [SerializeField] float[] weight_Menu;
    [SerializeField] float[] weight_Fight;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {
        plusMusic = PlusMusic_DJ.Instance;
        animator = GetComponent<Animator>();
        ActivateUI(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (plusMusic.AllFilesLoaded() && !isLoaded)
        {
            isLoaded = true;
            SelectMusic();
            animator.Play("FadeOut");
        }
    }
    public void ActivateUI(bool flag)
    {
        uI.SetActive(flag);
    }

    public void ActivateUI_Int(int i)
    {
        if(i == 0)
            uI.SetActive(false);
        else
            uI.SetActive(true);
    }

    private void StartMusic(string id, PlusMusic_DJ.PMTags tags)
    {
        plusMusic.ChangeSoundtrack(id);
        TransitionMusic(tags, 0);
    }

    public void TransitionMusic(PlusMusic_DJ.PMTags tags, int sceneType)
    {
        TransitionInfo transitionInfo = new TransitionInfo(tags, 0.25f);
        plusMusic.PlayArrangement(transitionInfo);
        if(sceneType == 0)
            mixer.TransitionToSnapshots(snapshots, weight_Menu, 0.5f);
        else if (sceneType == 1)
            mixer.TransitionToSnapshots(snapshots, weight_Fight, 0.5f);
    }

    public void SelectMusic()
    {
        int rnd = Random.Range(0, 6);
        switch (rnd)
        {
            case 0: //Start
                StartMusic("1683", PlusMusic_DJ.PMTags.full_song);
                break;
            case 1: //Hub
                StartMusic("1684", PlusMusic_DJ.PMTags.full_song);
                break;
            case 2: //Rex
                StartMusic("1681", PlusMusic_DJ.PMTags.full_song);
                break;
            case 3: //Ubuntu
                StartMusic("1434", PlusMusic_DJ.PMTags.full_song);
                break;
            case 4: //BubbleBoss
                StartMusic("1595", PlusMusic_DJ.PMTags.full_song);
                break;
            case 5: //Tuto
                StartMusic("1682", PlusMusic_DJ.PMTags.full_song);
                break;
            default:
                break;
        }
    }

}
