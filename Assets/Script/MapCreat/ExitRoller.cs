using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class ExitRoller : MonoBehaviour
    {
        float timer = 0;
        bool finalStoreOpend = false;
        void Update()
        {
            transform.localPosition = Vector3.zero;
            if (transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z < 200)
            {
                if (!finalStoreOpend)
                {
                    finalStoreOpend = true;
                    GameManager.abilityStore.showStore();
                    GameManager.abilityStore.appearRoomNum = 9999;
                }
            }
            if (finalStoreOpend)
            {
                if ((timer += Time.deltaTime) > 0.5f)
                {
                    SceneManager.LoadScene("Game 2");
                }
            }
        }
    }
}