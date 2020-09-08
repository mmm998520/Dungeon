using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.BoardGameDungeon
{
    public class Exit : MonoBehaviour
    {
        public AudioSource ExitAudio;
        void Start()
        {
            ExitAudio.Play();
        }
    }
}