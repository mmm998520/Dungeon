using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrackTransform : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 Offset;
    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.position = Camera.main.WorldToScreenPoint(target.position) + Offset;
    }
}
