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
    public PlusMusic_DJ plusMusic;
    private bool isLoaded;

    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioMixerSnapshot[] snapshots;
    [SerializeField] float[] weight_Menu;
    [SerializeField] float[] weight_Fight;

    [SerializeField] AudioSource[] sourcePlus;

    [SerializeField] AudioClip[] clips;

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

        //SetSoundLevel();
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
        TransitionInfo transitionInfo = new TransitionInfo(tags, 0.5f);
        if (plusMusic != null)
        {
            if (transitionInfo != null)
            {
                plusMusic.PlayArrangement(transitionInfo);
            } 
        }
        else
        {
            plusMusic = FindObjectOfType<PlusMusic_DJ>();
            plusMusic.PlayArrangement(transitionInfo);
        }

        if(sceneType == 0)
            mixer.TransitionToSnapshots(snapshots, weight_Menu, 0.5f);
        else if (sceneType == 1)
            mixer.TransitionToSnapshots(snapshots, weight_Fight, 0.5f);
    }

    public void SelectMusic()
    {
         StartMusic("1434", PlusMusic_DJ.PMTags.full_song);
    }

    private void SetSoundLevel()
    {
        foreach(AudioSource source in sourcePlus)
        {
            if(source.volume == 0)
                source.volume = 1;
        }
    }

}
