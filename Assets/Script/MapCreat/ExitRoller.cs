using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class ExitRoller : MonoBehaviour
    {
        bool goNext = false;
        bool open = false;
        void Update()
        {
            transform.localPosition = Vector3.zero;
            if (!open)
            {
                if (transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z < 200)
                {
                    open = true;
                }
            }
            else
            {
                if (GameManager.layers == 1)
                {
                    AbilityShower abilityShower = GameObject.Find("AbilityShower").GetComponent<AbilityShower>();
                    if (!goNext && abilityShower.rotate == 0 && !ReLifeParticle.Tracking)
                    {
                        GameManager.passLayerOneTimes += 1;
                        GameManager.layerOneCntinuousDideTimes = 0;
                        goNext = true;
                        SwitchScenePanel.NextScene = "Game 2";
                        GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                    }
                }
                else
                {
                    AbilityShower abilityShower = GameObject.Find("AbilityShower").GetComponent<AbilityShower>();
                    if (!goNext && abilityShower.rotate == 0 && !ReLifeParticle.Tracking)
                    {
                        GameManager.passLayerThreeTimes += 1;
                        GameManager.layerThreeCntinuousDideTimes = 0;
                        goNext = true;
                        SwitchScenePanel.NextScene = "Game 4";
                        GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                    }
                }
            }
        }
    }
}