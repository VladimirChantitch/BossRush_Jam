using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlusMusic;
public class PlisMusic : MonoBehaviour
{
    public PlusMusic_DJ plusMusic;
    public Transform square;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(plusMusic.TimeNextBeat());
        square.localScale = new Vector3(1, 1+ plusMusic.TimeNextBeat(),1);
    }
}
