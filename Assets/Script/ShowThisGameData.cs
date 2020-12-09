using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace com.DungeonPad
{
    public class ShowThisGameData : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (GameManager.DiedBecauseTimer >= 0.3f)
            {
                GameManager.DiedBecause = "Distance";
            }
            writeFile("TestData", "\r" + GameManager.DiedBecause + "," + (int)GameManager.PlayTime/60 + " : " + (int)GameManager.PlayTime % 60 + "," + 
                GameManager.KillSpider + "," + GameManager.KillSlime + "," + 
                GameManager.P1SpiderShooted + "," + GameManager.P1SpiderHit + "," + GameManager.P1SlimeHit + "," + GameManager.P1BubbleTimes + "," + 
                GameManager.P2SpiderShooted + "," + GameManager.P2SpiderHit + "," + GameManager.P2SlimeHit + "," + GameManager.P2BubbleTimes);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void writeFile(string fileName, string content)
        {
            Debug.LogWarning(content);
            FileStream fs;
            try
            {
                fs = new FileStream(Application.streamingAssetsPath + "//" + fileName + ".csv", FileMode.Append);   //開啟一個寫入流
            }
            catch
            {
                fs = new FileStream(Application.streamingAssetsPath + "//" + fileName + ".csv", FileMode.Create);   //開啟一個寫入流
            }
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();     //流會緩衝，此行程式碼指示流不要緩衝資料，立即寫入到檔案。
            fs.Close();     //關閉流並釋放所有資源，同時將緩衝區的沒有寫入的資料，寫入然後再關閉。
            fs.Dispose();   //釋放流所佔用的資源，Dispose()會呼叫Close(),Close()會呼叫Flush();    也會寫入緩衝區內的資料。
        }
    }
}