using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPosToUIPos : MonoBehaviour
{
    RectTransform rectTransform;
    public Transform BluePlayer, RedPlayer;
    public bool blue;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        setPos();
    }

    public void setPos()
    {
        if (blue)
        {
            rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(BluePlayer.position);
            //rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(BluePlayer.position) + new Vector3(rectTransform.sizeDelta.x / 2, rectTransform.sizeDelta.y / 2);
        }
        else
        {
            rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(RedPlayer.position);
            //rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(RedPlayer.position) + new Vector3(rectTransform.sizeDelta.x/2, rectTransform.sizeDelta.y/2);
        }
    }
}
