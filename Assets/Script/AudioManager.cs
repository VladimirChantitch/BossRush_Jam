using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    List<AudioSource> sources = new List<AudioSource>();
    bool _1;
    bool _2;
    bool _3;

    private void Awake()
    {
        sources = GetComponents<AudioSource>().ToList();
        for
    }

    public void PlayClip(string clipName)
    {
        AudioClip clip = DADDY.Instance.GetClipByName(clipName);
        if (!_1)
        {
            _1 = true;
        }
        if (!_2)
        {
            _1 = true;
        }
        if (!_3)
        {
            _1 = true;
        }
    }

    public class AudioSlot{
        public AudioSlot(AudioClip clip)
        {
            this.clip = clip;
            playing = false;

            //// binder la fin du clip pour libéré la source 
        }

        public AudioClip clip;
        public bool playing;
    }
}
