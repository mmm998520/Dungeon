using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TaurenRoomControler : MonoBehaviour
    {
        public TaurenBoss taurenBoss;
        void Start()
        {

        }

        void Update()
        {

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
            taurenBoss.canWalk = (_canPunch > 0);
        }

        public void setTaurenBossFatalBlow()
        {
            taurenBoss.animator.SetTrigger("FatalBlow");
        }

        public void TaurenBossReCharge()
        {
            taurenBoss.Armor = taurenBoss.MaxArmor;
        }
    }
}