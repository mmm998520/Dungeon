using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace com.DungeonPad
{
    public class GlobalLightSetting : MonoBehaviour
    {
        void Start()
        {
            settingLight();
        }

        public static void settingLight()
        {
            try
            {
                GameObject.Find("Directional Light 2D").GetComponent<Light2D>().intensity = SettingPanal.Lightness / 10f;
            }
            catch
            {

            }
        }
    }
}