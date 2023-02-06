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

    public async void LaunchNewBossFight(string bossName)
    {
        Door.SetActive(false);

        await Task.Delay(250);

        DoorOpen.SetActive(true);

        await Task.Delay(250);

        DoorOpen.GetComponent<Light2D>().enabled = true;

        await Task.Delay(250);

        SceneManager.LoadScene(bossName);
    }
}
