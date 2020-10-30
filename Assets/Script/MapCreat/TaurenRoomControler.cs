using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TaurenRoomControler : MonoBehaviour
    {
        public TaurenBoss taurenBoss;
        public Transform CenterWall, TaurenInsPoses, Winds;
        List<Transform> winds = new List<Transform>();
        public GameObject tauren;
        HashSet<Transform> usedFort = new HashSet<Transform>();

        void Start()
        {

        }

        void Update()
        {
            //防止條動畫失誤陷入無限迴圈
            if(usedFort.Count> CenterWall.childCount - 4)
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
                r = Random.Range(0, CenterWall.childCount);
                selectedFort = CenterWall.GetChild(r).GetChild(2);
            } while (usedFort.Contains(selectedFort));

            usedFort.Add(selectedFort);
            selectedFort.GetComponent<Animator>().SetTrigger("Shoot");
        }

        public void clearUsedFort()
        {
            usedFort.Clear();
        }
        #endregion

        #region//小牛頭人生成
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
        #endregion

        #region//風
        Transform selectedWind;

        public void SelectWind()
        {
            int r = Random.Range(0, winds.Count), _r = Random.Range(0, 2);
            selectedWind = winds[r];
            selectedWind.rotation = Quaternion.Euler(0, _r * 180, selectedWind.rotation.eulerAngles.z);
            winds.RemoveAt(r);
        }

        public void ResetWindsList()
        {
            winds.Clear();
            for (int i = 0; i < Winds.childCount; i++)
            {
                winds.Add(Winds.GetChild(i));
            }
        }

        public void LetWindBlow()
        {
            selectedWind.GetComponent<Animator>().SetTrigger("Blow");
        }
        #endregion
    }
}