using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MagneticFields : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            int i, j;
            for(i = 0; i < transform.childCount; i++)
            {
                /*
                MagneticField magneticField = transform.GetChild(i).GetComponent<MagneticField>();
                for(j=0;j< magneticField.monsters.Count; j++)
                {
                    attack(magneticField.monsters[j], 0.1f * Time.deltaTime);
                }*/
                if (i >= 3)
                {
                    transform.GetChild(transform.childCount -1 - i).GetComponent<Animator>().SetTrigger("end");
                }
            }
            //monsterManagers.Clear();
        }
    }
}