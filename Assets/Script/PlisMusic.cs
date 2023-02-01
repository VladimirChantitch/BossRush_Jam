using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlusMusic;
public class PlisMusic : MonoBehaviour
{
    private PlusMusic_DJ plusMusic;
    public Transform square_1;
    public Transform square_2;

    public TransitionInfo transitionInfo;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        plusMusic = PlusMusic_DJ.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(plusMusic.AllFilesLoaded())
        {
            Debug.Log(plusMusic.TimeNextBeat());
            square_1.localScale = new Vector3(1 + plusMusic.TimeNextBeat(), 1 + plusMusic.TimeNextBeat(), 1);
            square_2.localScale = new Vector3(1 + plusMusic.TimeNextBar(), 1 + plusMusic.TimeNextBar(), 1);

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                plusMusic.PlayArrangement(transitionInfo);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                index++;
                if (index >= plusMusic.theSoundtrackOptions.Length)
                    index = 0;

                plusMusic.ChangeSoundtrack(plusMusic.theSoundtrackOptions[index].id);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (plusMusic.TimeNextBeat() < 0.2f)
                {
                    square_1.transform.eulerAngles += Vector3.forward * 10f;
                }
                else
                {
                    square_1.transform.eulerAngles = Vector3.zero;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {

            }
        }
        

    }
}
