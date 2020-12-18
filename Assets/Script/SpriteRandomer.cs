using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomer : MonoBehaviour
{
    public Sprite[] sprites;
    public bool wall;
    public bool randomRotate;
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        if (!wall)
        {
            transform.rotation = Quaternion.Euler(0, 180 * Random.Range(0, 2), 0);
        }
        else
        {
            int r = Random.Range(0, 2);
            transform.parent.GetChild(0).rotation = Quaternion.Euler(0, 180 * r, 0);
            transform.parent.GetChild(1).rotation = Quaternion.Euler(0, 180 * r, 0);
        }
        if (randomRotate)
        {
            transform.Rotate(0, 0, 90 * Random.Range(0, 4));
        }
    }

    void Update()
    {
        
    }
}
