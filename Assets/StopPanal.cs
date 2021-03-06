﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.DungeonPad
{
    public class StopPanal : MonoBehaviour
    {
        EventSystem eventSystem;
        static int MusicSound, FXSound, Lightness, PlayerNum;
        [SerializeField] Sprite MusicSoundSpriteSelect, MusicSoundSpriteUnSelect,
                                                FXSoundSpriteSelect, FXSoundSpriteUnSelect,
                                                LightnessSpriteSelect, LightnessSpriteUnSelect,
                                                PlayerNumSpriteSelect, PlayerNumSpriteUnSelect;
        [SerializeField] Image MusicSoundText, FXSoundText, LightnessText, PlayerNumText;
        [SerializeField] Slider MusicSoundSlider, FXSoundSlider, LightnessSlider, PlayerNumSlider;

        void Awake()
        {
            eventSystem = EventSystem.current;
            if (PlayerPrefs.HasKey("MusicSound"))
            {
                MusicSoundSlider.value = PlayerPrefs.GetInt("MusicSound");
            }
            else
            {
                MusicSoundSlider.value = 5;
            }
            setMusicSound();
            if (PlayerPrefs.HasKey("FXSound"))
            {
                FXSoundSlider.value = PlayerPrefs.GetInt("FXSound");
            }
            else
            {
                FXSoundSlider.value = 5;
            }
            if (PlayerPrefs.HasKey("Lightness"))
            {
                LightnessSlider.value = PlayerPrefs.GetInt("Lightness");
            }
            else
            {
                LightnessSlider.value = 5;
            }
            if (PlayerPrefs.HasKey("PlayerNum"))
            {
                PlayerNumSlider.value = PlayerPrefs.GetInt("PlayerNum");
            }
            else
            {
                PlayerNumSlider.value = 1;
            }
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            StartCoroutine("SelectButtonLater");
        }

        private void Update()
        {
            Slider slider = eventSystem.currentSelectedGameObject.GetComponent<Slider>();
            MusicSoundText.sprite = MusicSoundSpriteUnSelect;
            FXSoundText.sprite = FXSoundSpriteUnSelect;
            LightnessText.sprite = LightnessSpriteUnSelect;
            PlayerNumText.sprite = PlayerNumSpriteUnSelect;
            if (slider == MusicSoundSlider)
            {
                MusicSoundText.sprite = MusicSoundSpriteSelect;
            }
            else if (slider == FXSoundSlider)
            {
                FXSoundText.sprite = FXSoundSpriteSelect;
            }
            else if (slider == LightnessSlider)
            {
                LightnessText.sprite = LightnessSpriteSelect;
            }
            else if (slider == PlayerNumSlider)
            {
                PlayerNumText.sprite = PlayerNumSpriteSelect;
            }
        }

        public void setMusicSound()
        {
            MusicSound = (int)MusicSoundSlider.value;
            PlayerPrefs.SetInt("MusicSound", MusicSound);
        }

        public void setFXSound()
        {
            FXSound = (int)FXSoundSlider.value;
            PlayerPrefs.SetInt("FXSound", FXSound);
        }

        public void setLightness()
        {
            Lightness = (int)LightnessSlider.value;
            PlayerPrefs.SetInt("Lightness", Lightness);
        }

        public void setPlayerNum()
        {
            PlayerNum = (int)PlayerNumSlider.value;
            PlayerPrefs.SetInt("PlayerNum", PlayerNum);
        }

        private IEnumerator SelectButtonLater()
        {
            Image image = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            Sprite sp = image.sprite;
            image.sprite = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Slider>().spriteState.selectedSprite;
            yield return null;
            eventSystem.SetSelectedGameObject(transform.GetChild(0).GetChild(1).GetChild(0).gameObject);
            image.sprite = sp;
        }
    }
}