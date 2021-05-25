using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace com.DungeonPad
{
    public class DemoEndingTimer : MonoBehaviour
    {
        [SerializeField] MusicManager music;
        [SerializeField] float startVolume;
        [SerializeField] VideoPlayer video;
        [SerializeField] Image[] Panels;
        int PanelsNum = 1;
        [SerializeField] float timer, timerStoper = 6;

        enum Stat
        {
            文字,
            預告片
        }
        Stat stat = Stat.文字;

        void Update()
        {
            if (InputManager.anyEnter() || InputManager.anyExit())
            {
                timer = timerStoper;
            }
            timer += Time.deltaTime;
            if (stat == Stat.文字)
            {
                try
                {
                    Panels[PanelsNum].color = new Color(1, 1, 1, (timer - 0.5f) / (timerStoper - 0.25f));
                }
                catch
                {
                    Debug.Log("沒了");
                }
                if (timer > timerStoper)
                {
                    timer = 0;
                    try
                    {
                        Panels[++PanelsNum].enabled = true;
                    }
                    catch
                    {
                        for (int i = 0; i < Panels.Length; i++)
                        {
                            Panels[i].enabled = false;
                        }
                        stat = Stat.預告片;
                        timerStoper = 51;
                        startVolume = music.thisVolume;
                        /*video.audioOutputMode = VideoAudioOutputMode.AudioSource;//设置音频输出模式
                        video.SetTargetAudioSource(0, music.GetComponent<AudioSource>());//设置音频声道，绑定AudioSource组件
                        video.playOnAwake = false;//取消默认播放
                        video.IsAudioTrackEnabled(0);//开启音频声道*/
                    }
                }
            }
            else
            {
                if (timer > timerStoper)
                {
                    video.gameObject.SetActive(false);
                    SwitchScenePanel.NextScene = "DEMO Thanks";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
                else if (timer > 2)
                {
                    video.SetDirectAudioVolume(0, startVolume * MusicManager.gameVolume * MusicManager.gradientVolume);
                    video.gameObject.SetActive(true);
                }
                else
                {
                    music.thisVolume = Mathf.Lerp(startVolume, 0, timer / 1.5f);
                    music.setVolume();
                }
            }
        }
    }
}