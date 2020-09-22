using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class GetHurt : MonoBehaviour
    {
        float timer = 0;
        void Update()
        {
            if((timer+= Time.deltaTime) > 0.2)
            {
                timer = 0;
                gameObject.SetActive(false);
            }
        }
    }
}