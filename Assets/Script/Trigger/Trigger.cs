using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Trigger : MonoBehaviour
    {
        public int GenNum;
        protected TriggerGen triggerGen;

        public void start()
        {
            triggerGen = GameObject.Find("TriggerGen").GetComponent<TriggerGen>();
            triggerGen.rePos(transform);
        }
    }
}