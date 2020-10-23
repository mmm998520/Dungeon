using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Slime : MonsterManager
    {
        float timer;
        void Start()
        {
            RepTime = new WaitForSeconds(repTimer);
            startRoomRow = Mathf.RoundToInt(transform.position.x) / GameManager.mazeCreater.objectCountRowNum;
            startRoomCol = Mathf.RoundToInt(transform.position.y) / GameManager.mazeCreater.objectCountColNum;
            arriveNewRoom(startRoomRow, startRoomCol);
            randomTarget();
            ArmorBar = transform.GetChild(1);
            findRoadWait = new WaitForSeconds(Random.Range(0.7f, 1f));
        }

        void Update()
        {
            if ((energyDeficiencyTimer += Time.deltaTime) > 1f)
            {
                Armor += Time.deltaTime;
                Armor = Mathf.Clamp(Armor, 0, MaxArmor);
            }

            if (TauntTarge == null)
            {
                randomMove();
            }
            else
            {
                endRow = new int[] { Mathf.RoundToInt(TauntTarge.position.x) };
                endCol = new int[] { Mathf.RoundToInt(TauntTarge.position.y) };
            }


            StartCoroutine("findRoad");
            if (nextPos == null)
            {
                if (TauntTarge == null)
                {
                    randomMove();
                }
                else
                {
                    endRow = new int[] { Mathf.RoundToInt(TauntTarge.position.x) };
                    endCol = new int[] { Mathf.RoundToInt(TauntTarge.position.y) };
                }
                StartCoroutine("findRoad");
            }
            moveToTarget();
            changeDirection();

            ArmorBar.gameObject.SetActive(Armor > 0);
            ArmorBar.localScale = Vector3.one * ((Armor / MaxArmor) * 0.6f + 0.4f);
        }

        void randomTarget()
        {
            int r = Random.Range(0, canGo.Count);
            endRow = new int[] { canGo[r] / MazeCreater.totalCol };
            endCol = new int[] { canGo[r] % MazeCreater.totalCol };
        }

        void randomMove()
        {
            if ((timer += Time.deltaTime) > 5)
            {
                timer = 0;
                randomTarget();
            }
            if (nextPos != null)
            {
                if (nextPos.Length > 1)
                {
                    if (Vector3.Distance(new Vector3(endRow[0], endCol[0], 0), transform.position) < 0.5f)
                    {
                        timer = 0;
                        randomTarget();
                    }
                }
            }
            else
            {
                randomTarget();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<PlayerManager>())
            {
                collision.collider.GetComponent<PlayerManager>().StickTimer = 0;
                Destroy(gameObject);
            }
        }
    }
}