using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ShowAbilityDetail : MonoBehaviour
    {
        Image text;
        [SerializeField] Sprite Null;
        float timer = 10;
        [SerializeField] GameObject[] Xs;

        void Start()
        {
            text = GetComponent<Image>();
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer > 10)
            {
                text.sprite = Null;
            }
            for (int i = 0; i < 2; i++)
            {
                Xs[i].SetActive(text.sprite.name == "傳送Lv" + (i + 1));
            }
        }

        public void showDetail(string ability)
        {
            timer = 0;
            text.sprite = Resources.Load<Sprite>("UI/Ability/AbilityText/" + ability);
        }
    }
}