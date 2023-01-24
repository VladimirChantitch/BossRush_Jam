using Boss.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HubManager : MonoBehaviour
{
    public UnityEvent GoblinInteract = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        //If the left mouse button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<HubInteractor>() is HubInteractor hubInteractor)
                {
                    HandleInteractable(hubInteractor);
                }
            }
        }
    }

    private void HandleInteractable(HubInteractor interactable)
    {
        interactable.interacts?.Invoke();
        switch (interactable.interactables)
        {
            case HubInteractor.Interactables.Door:
                break;
            case HubInteractor.Interactables.Wheel:
                break;
            case HubInteractor.Interactables.Goblin:
                GoblinInteract?.Invoke();
                break;
            case HubInteractor.Interactables.Crafter:
                break;
            case HubInteractor.Interactables.Gauge:
                break;
            case HubInteractor.Interactables.Weapon:
                break;
            case HubInteractor.Interactables.Map:
                break;
            case HubInteractor.Interactables.Pot_1:
                break;
            case HubInteractor.Interactables.Pot_2:
                break;
            case HubInteractor.Interactables.Pot_3:
                break;
        }
    }
}
