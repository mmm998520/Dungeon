using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.DungeonPad
{
    public class StopPanal : MonoBehaviour
    {
        void OnEnable()
        {
            StartCoroutine("SelectButtonLater");
        }

        private IEnumerator SelectButtonLater()
        {
            Image image = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            Sprite sp = image.sprite;
            image.sprite = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Slider>().spriteState.selectedSprite;
            yield return null;
            EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(1).GetChild(0).gameObject);
            image.sprite = sp;
        }
    }
}