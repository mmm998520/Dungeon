using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TaurenRoomControler : MonoBehaviour
    {
        public TaurenBoss taurenBoss;
        public Transform centerWall, TaurenInsPoses;
        public GameObject tauren;
        HashSet<Transform> usedFort = new HashSet<Transform>();

        void Start()
        {

        }

        void Update()
        {
            //防止條動畫失誤陷入無限迴圈
            if(usedFort.Count> centerWall.childCount - 4)
            {
                usedFort.Clear();
            }
        }

        #region//Boss
        public void setTaurenBossCanWalk(int _canWalk)
        {
            taurenBoss.canWalk = (_canWalk > 0);
            if (_canWalk > 0)
            {
                taurenBoss.animator.SetTrigger("Walk");
            }
        }

        public void setTaurenBossCanPunch(int _canPunch)
        {
            taurenBoss.canPunch = (_canPunch > 0);
        }

        public void setTaurenBossFatalBlow()
        {
            taurenBoss.animator.SetTrigger("FatalBlow");
        }

        public void TaurenBossReCharge()
        {
            taurenBoss.Armor = taurenBoss.MaxArmor;
        }
        #endregion

        #region//砲台
        public void selectOneFort()
        {
            int r;
            Transform selectedFort;
            do
            {
                r = Random.Range(0, centerWall.childCount);
                selectedFort = centerWall.GetChild(r).GetChild(2);
            } while (usedFort.Contains(selectedFort));

            usedFort.Add(selectedFort);
            selectedFort.GetComponent<Animator>().SetTrigger("Shoot");
        }

        public void clearUsedFort()
        {
            usedFort.Clear();
        }
        #endregion

        public void setTaurenInsPosesRotate()
        {
            float angle = Vector3.SignedAngle(Vector3.right, CameraManager.center * Vector2.one, Vector3.forward);
            if (angle > -135 && angle <= -45)
            {
                TaurenInsPoses.rotation = Quaternion.Euler(0, 0, 270);
            }
            else if (angle > -45 && angle <= 45)
            {
                TaurenInsPoses.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (angle > 45 && angle <= 135)
            {
                TaurenInsPoses.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                TaurenInsPoses.rotation = Quaternion.Euler(0, 0, 180);
            }
        }

        public void insTaurenByOrder(int Order)
        {
            if (Order != 4)
            {
                for(int i=0;i< TaurenInsPoses.GetChild(Order).childCount; i++)
                {
                    Instantiate(tauren, TaurenInsPoses.GetChild(Order).GetChild(i).position, Quaternion.identity);
                }
            }
            else
            {
                Instantiate(tauren, TaurenInsPoses.GetChild(Order).GetChild(Random.Range(0, TaurenInsPoses.GetChild(Order).childCount)).position, Quaternion.identity);
            }
        }
    }
}