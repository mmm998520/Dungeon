using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class SellerTalk : MonoBehaviour
    {
        RectTransform rectTransform;
        public Transform seller;
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            transform.SetParent(GameObject.Find("Canvas").transform);
        }

        void Update()
        {
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(seller.position);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}