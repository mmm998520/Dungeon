using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class BubbleGen : MonoBehaviour
    {
        public float timer, timerStoper, timerStoperMin, timerStoperMax;
        public GameObject Bubble;
        public AudioSource BubbleSource;

        private void Start()
        {
            if(GameManager.layers == 2 && GameManager.CurrentSceneName =="Game 1")
            {
                transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.2941177f, 0.2352941f, 0.227451f);
            }
        }

        void Update()
        {
            if ((timer += Time.deltaTime) >= timerStoper)
            {
                timer = 0;
                timerStoper = Random.Range(timerStoperMin, timerStoperMax);
                Destroy(Instantiate(Bubble, transform.position+Vector3.up * 0.2f, Quaternion.identity, transform), 6);
                BubbleSource.pitch += Random.Range(-0.2f, 0.2f);
                BubbleSource.pitch = Mathf.Clamp(BubbleSource.pitch, 0.8f, 1.3f);
                BubbleSource.Play();
            }
        }
    }
}