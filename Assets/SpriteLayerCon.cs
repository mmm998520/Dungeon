using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerCon : MonoBehaviour
{
    [SerializeField] SpriteRenderer back, top;
    static HashSet<int> layers = new HashSet<int>();
    int r;

    void Start()
    {
        if (!back)
        {
            back = transform.parent.GetComponent<SpriteRenderer>();
        }
        if (!top)
        {
            top = GetComponent<SpriteRenderer>();
        }
        
        do
        {
            r = Random.Range(10, 450) * 2;
        } while (layers.Contains(r));
        layers.Add(r);
        back.sortingOrder = r;
        top.sortingOrder = r + 1;
    }

    private void OnDestroy()
    {
        layers.Remove(r);
    }
}
