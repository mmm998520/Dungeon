using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class Exit : Trigger
    {
        public Transform monsters;

        void Update()
        {
            if(monsters.childCount == 0)
            {
                GetComponent<Collider2D>().enabled = true;
                GetComponent<MeshRenderer>().enabled = true;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                SceneManager.LoadScene("Game 2");
            }
        }
    }
}