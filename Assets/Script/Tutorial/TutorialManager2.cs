using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class TutorialManager2 : MonoBehaviour
    {
        public enum TutorialStat
        {
            story,
            p1Move,
            p1MoveCanPress,
            p2Move,
            p2MoveCanPress,
            dumbMonster,
            dumbMonsterCanPress,
            fight,
            slimeAndBubble,
            presseB,
            hole,
            holeCanMove,
            map
        }
        public TutorialStat stat;
        public enum PressedA
        {
            Non,
            P1,
            P2,
            P1P2
        }
        public PressedA pressedA;
        float statTimer,flashTimer;
        PlayerManager p1, p2;
        public Text TutorialText, P1Talk, P2Talk;
        public Animator monsterAnimator;

        public GameObject GoNextPos;

        public GameObject P1TalkBox, P2TalkBox, B, A, Dir, Map,BPicture,APicture,redcircle;
        void Start()
        {
            if (transform.GetChild(0).GetComponent<PlayerManager>().p1)
            {
                p1 = transform.GetChild(0).GetComponent<PlayerManager>();
                p2 = transform.GetChild(1).GetComponent<PlayerManager>();
            }
            else
            {
                p1 = transform.GetChild(1).GetComponent<PlayerManager>();
                p2 = transform.GetChild(0).GetComponent<PlayerManager>();
            }
            if(SelectMouse.playerColor == SelectMouse.PlayerColor.P1Blue_P2Red)
            {
                P1TalkBox.GetComponent<playerPosToUIPos>().blue = true;
                P2TalkBox.GetComponent<playerPosToUIPos>().blue = false;
            }
            else
            {
                P1TalkBox.GetComponent<playerPosToUIPos>().blue = false;
                P2TalkBox.GetComponent<playerPosToUIPos>().blue = true;
            }
            TutorialText.text = "僅存的光之元素啊，\n\r同心協力前往地城拯救被巨龍抓走的同胞吧";
            P1Talk.text = "是";
            P2Talk.text = "是";
            P1TalkBox.SetActive(true);
            P2TalkBox.SetActive(true);
            reset();
        }

        void Update()
        {
            switch (stat)
            {
                case TutorialStat.story:
                    p1.playerStat = PlayerManager.PlayerStat.CantMove;
                    p2.playerStat = PlayerManager.PlayerStat.CantMove;
                    p1PressA();
                    p2PressA();
                    if (pressedA == PressedA.P1P2)
                    {
                        stat = TutorialStat.p1Move;
                        p1.playerStat = PlayerManager.PlayerStat.Move;
                        p2.playerStat = PlayerManager.PlayerStat.CantMove;
                        TutorialText.text = "光代表你們共同的生命\n\r請P1移動看看\n\r觀察距離與光的關係";
                        P1Talk.text = "";
                        P2Talk.text = "";
                        P1TalkBox.SetActive(false);
                        P2TalkBox.SetActive(false);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        reset();
                    }
                    break;
                case TutorialStat.p1Move:
                    if ((statTimer += Time.deltaTime) > 10)
                    {
                        stat = TutorialStat.p1MoveCanPress;
                        statTimer = 0;
                        TutorialText.text = "光代表你們共同的生命\n\r請P1移動看看\n\r觀察距離與光的關係";
                        P1Talk.text = "距離近能回復光\n\r距離遠會耗損光";
                        P2Talk.text = "";
                        P1TalkBox.SetActive(true);
                        P2TalkBox.SetActive(false);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                    }
                    break;
                case TutorialStat.p1MoveCanPress:
                    p1PressA();
                    if (pressedA == PressedA.P1)
                    {
                        stat = TutorialStat.p2Move;
                        p1.playerStat = PlayerManager.PlayerStat.CantMove;
                        p2.playerStat = PlayerManager.PlayerStat.Move;
                        TutorialText.text = "光代表你們共同的生命\n\r請P2移動看看\n\r觀察距離與光的關係";
                        P1Talk.text = "";
                        P2Talk.text = "";
                        P1TalkBox.SetActive(false);
                        P2TalkBox.SetActive(false);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        reset();
                    }
                    break;
                case TutorialStat.p2Move:
                    if ((statTimer += Time.deltaTime) > 10)
                    {
                        stat = TutorialStat.p2MoveCanPress;
                        statTimer = 0;
                        TutorialText.text = "光代表你們共同的生命\n\r請P2移動看看\n\r觀察距離與光的關係";
                        P1Talk.text = "";
                        P2Talk.text = "要是光完全消失\n\r我們將一同死亡";
                        P1TalkBox.SetActive(false);
                        P2TalkBox.SetActive(true);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                    }
                    break;
                case TutorialStat.p2MoveCanPress:
                    p2PressA();
                    if (pressedA == PressedA.P2)
                    {
                        stat = TutorialStat.dumbMonster;
                        p1.playerStat = PlayerManager.PlayerStat.CantMove;
                        p2.playerStat = PlayerManager.PlayerStat.CantMove;
                        TutorialText.text = "地城裡有許多怪物\n\r但你們之間的能量線能消滅牠們\n\r消滅怪物甚至能快速回復光";
                        P1Talk.text = "";
                        P2Talk.text = "";
                        P1TalkBox.SetActive(false);
                        P2TalkBox.SetActive(false);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        reset();
                        monsterAnimator.SetBool("DumbSpider", true);
                    }
                    break;
                case TutorialStat.dumbMonster:
                    if ((statTimer += Time.deltaTime) > 11)
                    {
                        stat = TutorialStat.dumbMonsterCanPress;
                        statTimer = 0;
                        TutorialText.text = "地城裡有許多怪物\n\r但你們之間的能量線能消滅牠們\n\r消滅怪物甚至能快速回復光";
                        P1Talk.text = "好喔";
                        P2Talk.text = "好喔";
                        P1TalkBox.SetActive(true);
                        P2TalkBox.SetActive(true);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                    }
                    break;
                case TutorialStat.dumbMonsterCanPress:
                    p1PressA();
                    p2PressA();
                    if (pressedA == PressedA.P1P2)
                    {
                        stat = TutorialStat.fight;
                        p1.playerStat = PlayerManager.PlayerStat.Move;
                        p2.playerStat = PlayerManager.PlayerStat.Move;
                        for(int i=0;i< GameManager.monsters.childCount; i++)
                        {
                            GameManager.monsters.GetChild(i).gameObject.SetActive(true);
                        }
                        TutorialText.text = "小心，直接碰撞蜘蛛與蛛絲將耗損你們的光";
                        P1Talk.text = "";
                        P2Talk.text = "";
                        P1TalkBox.SetActive(false);
                        P2TalkBox.SetActive(false);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        reset();
                        monsterAnimator.SetBool("Fight", true);
                    }
                    break;
                case TutorialStat.fight:
                    if (GameManager.monsters.childCount == 0)
                    {
                        if((statTimer += Time.deltaTime) > 1)
                        {
                            stat = TutorialStat.slimeAndBubble;
                            statTimer = 0;
                            p1.playerStat = PlayerManager.PlayerStat.CantMove;
                            p2.playerStat = PlayerManager.PlayerStat.CantMove;
                            B.SetActive(true);
                            BPicture.SetActive(true);
                            if (BPicture.activeSelf == true)
                            {
                                redcircle.SetActive(true);
                                flashTimer += Time.deltaTime;
                                if(flashTimer >= 1 && flashTimer < 2)
                                {
                                    redcircle.SetActive(false);
                                }
                                if(flashTimer >= 2)
                                {
                                    flashTimer = 0;
                                }
                            }
                            
                            TutorialText.text = "史萊姆、毒泡泡會影響你們的行動\n\r快速按      可掙扎擺脫";
                            P1Talk.text = "";
                            P2Talk.text = "";
                            P1TalkBox.SetActive(false);
                            P2TalkBox.SetActive(false);
                            P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                            P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                            reset();
                            monsterAnimator.SetBool("Bubble", true);
                        }
                    }
                    break;
                case TutorialStat.slimeAndBubble:
                    if ((statTimer += Time.deltaTime) > 10)
                    {
                        if((statTimer+=Time.deltaTime) >= 1)
                        stat = TutorialStat.presseB;
                        statTimer = 0;
                        p1.playerStat = PlayerManager.PlayerStat.Move;
                        p2.playerStat = PlayerManager.PlayerStat.Move;
                        TutorialText.text = "史萊姆、毒泡泡會影響你們的行動\n\r快速按      可掙扎擺脫";
                        P1Talk.text = "";
                        P2Talk.text = "";
                        BPicture.SetActive(false);
                        P1TalkBox.SetActive(false);
                        P2TalkBox.SetActive(false);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                    }
                    break;
                case TutorialStat.presseB:
                    if (p1.ConfusionTimer > 10 && p2.ConfusionTimer > 10 && p1.StickTimer > 10 && p2.StickTimer > 10)
                    {
                        stat = TutorialStat.hole;
                        statTimer = 0;
                        pressedA = PressedA.Non;
                        p1.playerStat = PlayerManager.PlayerStat.CantMove;
                        p2.playerStat = PlayerManager.PlayerStat.CantMove;
                        B.SetActive(false);
                        A.SetActive(true);
                        APicture.SetActive(true);
                        Dir.SetActive(true);
                        TutorialText.text = "最後一步，      +      衝刺\n\r這能幫助你們度過深淵";
                        P1Talk.text = "好喔";
                        P2Talk.text = "好喔";
                        P1TalkBox.SetActive(true);
                        P2TalkBox.SetActive(true);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        reset();
                        monsterAnimator.SetBool("Hole", true);
                        GameObject.Find("TutorialManager").transform.GetChild(0).gameObject.SetActive(false);
                        GameObject.Find("TutorialManager").transform.GetChild(1).gameObject.SetActive(true);
                        GoNextPos.SetActive(true);
                    }
                    break;
                case TutorialStat.hole:
                    p1PressA();
                    p2PressA();
                    if (pressedA == PressedA.P1P2)
                    {
                        stat = TutorialStat.holeCanMove;
                        p1.playerStat = PlayerManager.PlayerStat.Move;
                        p2.playerStat = PlayerManager.PlayerStat.Move;
                        APicture.SetActive(false);
                    }
                    break;
                case TutorialStat.holeCanMove:
                    if (GoNextPos.activeSelf == false)
                    {
                        stat = TutorialStat.map;
                        statTimer = 0;
                        p1.playerStat = PlayerManager.PlayerStat.CantMove;
                        p2.playerStat = PlayerManager.PlayerStat.CantMove;
                        A.SetActive(false);
                        Dir.SetActive(false);
                        Map.SetActive(true);
                        TutorialText.text = "最後，相信地圖\n\r它將你們指引找到通往下一層的機關";
                        P1Talk.text = "好的";
                        P2Talk.text = "好的";
                        P1TalkBox.SetActive(true);
                        P2TalkBox.SetActive(true);
                        P1TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        P2TalkBox.GetComponent<playerPosToUIPos>().setPos();
                        reset();
                    }
                    break;
                case TutorialStat.map:
                    p1PressA();
                    p2PressA();
                    if (pressedA == PressedA.P1P2)
                    {
                        SceneManager.LoadScene("Game 1");
                    }
                    break;
            }
        }

        void p1PressA()
        {
            switch (SelectMouse.p1Joy)
            {
                case "WASD":
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        if (pressedA == PressedA.Non)
                        {
                            pressedA = PressedA.P1;
                        }
                        if (pressedA == PressedA.P2)
                        {
                            pressedA = PressedA.P1P2;
                        }
                        P1TalkBox.SetActive(false);
                    }
                    break;
                case "ArrowKey":
                    if (Input.GetKeyDown(KeyCode.Keypad1))
                    {
                        if (pressedA == PressedA.Non)
                        {
                            pressedA = PressedA.P1;
                        }
                        if (pressedA == PressedA.P2)
                        {
                            pressedA = PressedA.P1P2;
                        }
                        P1TalkBox.SetActive(false);
                    }
                    break;
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                    if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p1Joy))))
                    {
                        if (pressedA == PressedA.Non)
                        {
                            pressedA = PressedA.P1;
                        }
                        if (pressedA == PressedA.P2)
                        {
                            pressedA = PressedA.P1P2;
                        }
                        P1TalkBox.SetActive(false);
                    }
                    break;
            }
        }

        void p2PressA()
        {
            switch (SelectMouse.p2Joy)
            {
                case "WASD":
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        if (pressedA == PressedA.Non)
                        {
                            pressedA = PressedA.P2;
                        }
                        if (pressedA == PressedA.P1)
                        {
                            pressedA = PressedA.P1P2;
                        }
                        P2TalkBox.SetActive(false);
                    }
                    break;
                case "ArrowKey":
                    if (Input.GetKeyDown(KeyCode.Keypad1))
                    {
                        if (pressedA == PressedA.Non)
                        {
                            pressedA = PressedA.P2;
                        }
                        if (pressedA == PressedA.P1)
                        {
                            pressedA = PressedA.P1P2;
                        }
                        P2TalkBox.SetActive(false);
                    }
                    break;
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                    if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p2Joy))))
                    {
                        if (pressedA == PressedA.Non)
                        {
                            pressedA = PressedA.P2;
                        }
                        if (pressedA == PressedA.P1)
                        {
                            pressedA = PressedA.P1P2;
                        }
                        P2TalkBox.SetActive(false);
                    }
                    break;
            }
        }

        public void reset()
        {
            pressedA = PressedA.Non;
            p1.transform.position = new Vector3(17.75f, 6.0f, 9.6f);
            p2.transform.position = new Vector3(17.75f, 4.2f, 9.6f);
            PlayerManager.HP = 40;
            p1.v = Vector3.zero;
            p1.DashA = Vector3.zero;
            p1.HardStraightA = Vector3.zero;
            p1.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            p2.v = Vector3.zero;
            p2.DashA = Vector3.zero;
            p2.HardStraightA = Vector3.zero;
            p2.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        public void reStartBubble()
        {
            reset();
            stat = TutorialStat.slimeAndBubble;
            statTimer = 0;
            p1.playerStat = PlayerManager.PlayerStat.CantMove;
            p2.playerStat = PlayerManager.PlayerStat.CantMove;
            TutorialText.text = "史萊姆、毒泡泡會影響你們的行動\n\r快速按      可掙扎擺脫";
            P1Talk.text = "";
            P2Talk.text = "";
            monsterAnimator.SetBool("Bubble", true);
            p1.ConfusionTimer = 10;
            p1.StickTimer = 10;
            p1.ConfusionUIRenderer.enabled = false;
            p1.ConfusionUIcontroler.enabled = false;
            p2.ConfusionTimer = 10;
            p2.StickTimer = 10;
            p2.ConfusionUIRenderer.enabled = false;
            p2.ConfusionUIcontroler.enabled = false;
        }
    }
}