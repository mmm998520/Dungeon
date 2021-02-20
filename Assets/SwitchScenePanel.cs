using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class SwitchScenePanel : MonoBehaviour
    {
        void switchScenePanel()
        {
            GameManager.asyncOperation.allowSceneActivation = true;
        }
    }
}
