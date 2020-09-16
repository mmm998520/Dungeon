using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public int dir;
    private void OnMouseDrag()
    {
        if (dir == 0)
        {
            Camera.main.transform.Translate(Vector3.up * (10 * Time.deltaTime));
        }
        if (dir == 1)
        {
            Camera.main.transform.Translate(Vector3.right * (10 * Time.deltaTime));
        }
        if (dir == 2)
        {
            Camera.main.transform.Translate(Vector3.down * (10 * Time.deltaTime));
        }
        if (dir == 3)
        {
            Camera.main.transform.Translate(Vector3.left * (10 * Time.deltaTime));
        }
    }
}
