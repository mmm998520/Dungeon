using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TaurenRoomControler : MonoBehaviour
    {
        public TaurenBoss taurenBoss;
        public Transform centerWall;
        HashSet<Transform> usedFort = new HashSet<Transform>();

        void Start()
        {

        }

        void Update()
        {
            if(usedFort.Count> centerWall.childCount - 4)
            {
                usedFort.Clear();
            }
        }

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

        public void selectOneFort()
        {
            int r;
            Transform selectedFort;
            do
            {
                r = Random.Range(0, centerWall.childCount);
                selectedFort = centerWall.GetChild(r).GetChild(2);
            } while (usedFort.Contains(selectedFort));

            Debug.LogError(selectedFort.name + "" + r);
            usedFort.Add(selectedFort);
            selectedFort.GetComponent<Animator>().SetTrigger("Shoot");
        }

        public void clearUsedFort()
        {
            usedFort.Clear();
        }
    }
}