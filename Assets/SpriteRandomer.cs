using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomer : MonoBehaviour
{
    public Sprite[] sprites;
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        transform.rotation = Quaternion.Euler(0, 180 * Random.Range(0, 2), 0);
    }

    void Update()
    {
        
    }
}
