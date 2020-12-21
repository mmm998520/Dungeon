using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeRotater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 360 * 3 * Time.deltaTime);
    }
}
