using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace com.DungeonPad
{
    public class PlayerHPVolumeSet : MonoBehaviour
    {
        Vignette vignette;
        float speed = 5;
        bool lerpToMax = true;
        float lerpMax = 0.4f, lerpMin = 0.15f;
        void Start()
        {
            gameObject.GetComponent<Volume>().profile.TryGet(out vignette);
        }

        void Update()
        {
            if (PlayerManager.HP < 30)
            {
                if (lerpToMax)
                {
                    vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, lerpMax, Time.deltaTime * speed);
                    if(lerpMax - vignette.intensity.value < 0.01f)
                    {
                        lerpToMax = false;
                    }
                }
                else
                {
                    vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, lerpMin, Time.deltaTime * speed);
                    if (vignette.intensity.value - lerpMin < 0.01f)
                    {
                        lerpToMax = true;
                    }
                }
            }
            else
            {
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, Time.deltaTime * speed);
            }
        }
    }
}