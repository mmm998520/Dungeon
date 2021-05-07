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
        }

        public void showDetail(string ability)
        {
            timer = 0;
            text.sprite = Resources.Load<Sprite>("UI/Ability/AbilityText/" + ability);
            try
            {
                transform.GetChild(0).gameObject.SetActive(ability.Contains("傳送"));
            }
            catch
            {
                Debug.LogError("這場景忘了放");
            }
        }
    }
}