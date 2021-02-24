using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class Tutorial : MonoBehaviour
    {
        Color textColor;
        public Text[] texts;
        public int[] ShowTextCenters;
        int usedTextCenters;
        float textStopTimer;
        public int ShowHPLine;
        public Image[] images;
        public GameObject[] doors;
        bool canMove = true;

        // Start is called before the first frame update
        void Start()
        {
            textColor = texts[0].color;
        }

        // Update is called once per frame
        void Update()
        {
            Color color;
            for(int i = 0; i < texts.Length; i++)
            {
                float showTextDis = CameraManager.center.x - ShowTextCenters[i];
                if (i == usedTextCenters && showTextDis > 0f)
                {
                    canMove = false;
                }
                color = new Color(textColor.r, textColor.g, textColor.b, 2 - Mathf.Abs(showTextDis) * 0.5f);
                texts[i].color = color;
                for(int j=0; j < texts[i].transform.childCount; j++)
                {
                    texts[i].transform.GetChild(j).GetComponent<Image>().color = color;
                }
            }
            if (!canMove)
            {
                textStopTimer += Time.deltaTime;
                if (textStopTimer >= 3)
                {
                    usedTextCenters++;
                    textStopTimer = 0;
                    canMove = true;
                }
            }
            for (int j = 0; j < GameManager.players.childCount; j++)
            {
                PlayerManager playerManager = GameManager.players.GetChild(j).GetComponent<PlayerManager>();
                playerManager.enabled = canMove;
                if (!canMove)
                {
                    PlayerManager.HP = PlayerManager.MaxHP;
                    playerManager.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
            }

            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, (ShowHPLine - CameraManager.center.x) / ShowHPLine);
            }
            if (GameManager.monsters.childCount <= 2)
            {
                bool openDoor = true;
                for(int i = 0;i < GameManager.monsters.childCount; i++)
                {
                    if(GameManager.monsters.GetChild(i).GetComponent<Collider2D>().enabled == true)
                    {
                        openDoor = false;
                    }
                }
                if (openDoor)
                {
                    for (int i = 0; i < doors.Length; i++)
                    {
                        Destroy(doors[i]);
                    }
                }
            }
            if(CameraManager.center.x > 112.7f)
            {
                ReGamer.ReAbility();
                SwitchScenePanel.NextScene = "Home";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }
    }
}