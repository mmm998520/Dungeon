using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Spider : MonsterManager
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
            findRoadWait = new WaitForSeconds(Random.Range(0.3f, 0.5f));
        }

        void Update()
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

            attackCD();
            if (prepare != 0)
            {
                if (prepare == 1)
                {
                    Transform t = MinDisPlayer();
                    nextPos = new int[] { Mathf.RoundToInt(t.position.x), Mathf.RoundToInt(t.position.y) };
                }
                prepareAttack();
            }
            else
            {
                StartCoroutine("findRoad");
            }
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
            if ((timer += Time.deltaTime) > 3)
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

        protected override void afterAttack()
        {
            ArmorReCharge();
        }
    }
}