using Boss.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace Boss.Map
{
    public class MapInterractor : HubInteractor
    {
        public UnityEvent<MapInterractor> onInteract = new UnityEvent<MapInterractor>();
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Collider2D activator;
        [SerializeField] Light2D light2D;
        [SerializeField] float speed = 5;
        public BossLocalization location;
        bool isIncreasing;
        bool isUnlocked = false;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            activator = GetComponent<Collider2D>();
            light2D = GetComponent<Light2D>();
            activator.enabled = false;
            light2D.enabled = false;
            spriteRenderer.enabled = true;
        }

        public void Unlock()
        {
            if (!isUnlocked)
            {
                isUnlocked = true;
                spriteRenderer.enabled = false;
                activator.enabled = true;
                light2D.enabled = true;
                StartCoroutine(IntensityVariation());
            }
        }

        public void Interact()
        {
            onInteract?.Invoke(this);
        }

        IEnumerator IntensityVariation()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                float value = 0;

                if (isIncreasing)
                {
                    value = light2D.intensity + Time.deltaTime * speed;
                    if (value > 10)
                    {
                        isIncreasing = false;
                    }
                }
                else
                {
                    value = light2D.intensity - Time.deltaTime * speed;
                    if (value <= 0)
                    {
                        isIncreasing = true;
                    }
                }

                light2D.intensity = value;
            }
        }
    }

}
