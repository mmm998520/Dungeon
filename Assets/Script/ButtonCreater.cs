using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ButtonCreater : MonoBehaviour
    {
        public GameObject buttonPrefab;
        public GameObject[] button = new GameObject[2];
        public GameObject slime, cage, tauren, exit;
        public static List<Tauren> Taurens = new List<Tauren>();
        public static Exit Exit;
        void Update()
        {
            if (gameObject.GetComponent<ButtonManager>().pushButton && button[0].GetComponent<ButtonManager>().pushButton && button[1].GetComponent<ButtonManager>().pushButton)
            {
                List<GameObject> temp = new List<GameObject>();
                temp.Add(gameObject);
                temp.Add(button[0]);
                temp.Add(button[1]);
                int r, i;
                for(i = 3; i > 0; i--)
                {
                    r = Random.Range(0, i);
                    if (i == 3)
                    {
                        Vector3 vector = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.up;
                        Instantiate(slime, temp[r].transform.position + vector, Quaternion.Euler(Vector3.forward* Random.Range(0, 360)), GameManager.monsters);
                    }
                    else
                    {
                        Instantiate(cage, temp[r].transform.position, Quaternion.identity, GameManager.monsters);
                    }
                    temp.RemoveAt(r);
                }
                Taurens.Add(Instantiate(tauren, new Vector3(Random.Range(5, 7), Random.Range(3, 6)), Quaternion.Euler(Vector3.forward * Random.Range(0, 360)), GameManager.monsters).GetComponent<Tauren>());
                Taurens.Add(Instantiate(tauren, new Vector3(Random.Range(5, 7), Random.Range(3, 6)), Quaternion.Euler(Vector3.forward * Random.Range(0, 360)), GameManager.monsters).GetComponent<Tauren>());
                Exit = Instantiate(exit, new Vector3(Random.Range(5, 7), Random.Range(3, 6)), Quaternion.identity).GetComponent<Exit>();
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