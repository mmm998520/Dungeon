using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ButtonCreater : MonoBehaviour
    {
        public GameObject buttonPrefab;
        GameObject[] button = new GameObject[2];

        void Update()
        {
            if (gameObject.GetComponent<ButtonManager>().pushButton && button[0].GetComponent<ButtonManager>().pushButton && button[1].GetComponent<ButtonManager>().pushButton)
            {
                Destroy(gameObject);
                Destroy(button[0]);
                Destroy(button[1]);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8)
            {
                button[0] = Instantiate(buttonPrefab, new Vector3(Random.Range(1, 11), Random.Range(1, 8)), Quaternion.identity);
                button[1] = Instantiate(buttonPrefab, new Vector3(Random.Range(1, 11), Random.Range(1, 8)), Quaternion.identity);
            }
        }
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8)
            {
                Destroy(button[0]);
                Destroy(button[1]);
            }
        }
    }
}