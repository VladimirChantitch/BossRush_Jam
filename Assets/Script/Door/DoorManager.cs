using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    [SerializeField] GameObject Door;
    [SerializeField] GameObject DoorOpen;
    [SerializeField] float LightOpenSpeed;
    [SerializeField] float doorOpenSpeed;
    [SerializeField] float cameraZoomSpeed;

    bool alreadyLoading;

    public async void LaunchNewBossFight(string bossName)
    {
        if (!alreadyLoading)
        {
            alreadyLoading = true;
            StartCoroutine(Opendoor(bossName));
        }
    }

    IEnumerator Opendoor(string bossName)
    {
        DoorOpen.SetActive(true);
        Light2D light = DoorOpen.GetComponent<Light2D>();
        light.enabled = true;
        light.intensity = 0;

        while (Door.transform.position.y <= 5)
        {
            yield return new WaitForEndOfFrame();

            Vector2 pos = Door.transform.position;
            pos.y += Time.deltaTime * doorOpenSpeed;
            Door.transform.position = pos;

            light.intensity += Time.deltaTime * LightOpenSpeed;

            Camera.main.orthographicSize -= Time.deltaTime *cameraZoomSpeed;
        }
        AudioManager.Instance.TransitionMusic(PlusMusic_DJ.PMTags.high_template, 1);
        SceneManager.LoadScene(bossName);
    }
}
