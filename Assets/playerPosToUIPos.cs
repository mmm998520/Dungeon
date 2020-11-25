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
        if (blue)
        {
            rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(BluePlayer.position);
        }
        else
        {
            rectTransform.anchoredPosition = Camera.main.WorldToScreenPoint(RedPlayer.position);
        }
    }
}
