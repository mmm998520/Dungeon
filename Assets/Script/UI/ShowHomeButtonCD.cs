using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ShowHomeButtonCD : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            int i = (int)Mathf.Floor(Mathf.Floor(PlayerManager.homeButtonTimer) / 2);
            if (i > 0)
            {
                i = 0;
            }
            GetComponent<Text>().text = "" + i;
        }
    }
}