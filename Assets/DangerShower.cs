using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class DangerShower : MonoBehaviour
    {
        Image image;
        bool goMax = true;
        float minA = 0.3f, maxA = 0.5f; 
        void Start()
        {
            image = GetComponent<Image>();
        }

        void Update()
        {
            if (PlayerManager.HP / PlayerManager.MaxHP > 0.5f)
            {
                image.color = new Color(1, 1, 1, Mathf.Lerp(image.color.a, 0, Time.deltaTime * 6));
                goMax = true;
            }
            else
            {
                if (goMax)
                {
                    image.color = new Color(1, 1, 1, Mathf.Lerp(image.color.a, maxA, Time.deltaTime * 3));
                    if (maxA - image.color.a < 0.01f)
                    {
                        goMax = false;
                    }
                }
                else if(image.color.a > 0.01f)
                {
                    image.color = new Color(1, 1, 1, Mathf.Lerp(image.color.a, minA, Time.deltaTime * 3));
                    if (image.color.a - minA < 0.01f)
                    {
                        goMax = true;
                    }
                }
            }
        }
    }
}
