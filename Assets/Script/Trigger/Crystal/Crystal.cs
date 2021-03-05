using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace com.DungeonPad
{
    public class Crystal : MonoBehaviour
    {
        public Collider2D[] myCollider2d;
        public SpriteRenderer mySpriteRendererBottom, mySpriteRendererTop;
        public Sprite breakSprite;
        public Light2D crystalLight;
        public SpriteRenderer CircleLight;
        Dictionary<int, int> myCrystalPos = new Dictionary<int, int>(), myCrystalSidePos = new Dictionary<int, int>();
        static int myCrystalPosKeyNum;

        protected enum CrystalStat
        {
            unUse,
            use,
        }
        protected CrystalStat crystalStat = CrystalStat.unUse;

        protected virtual void Start()
        {
            int posX = Mathf.RoundToInt(transform.position.x), posY = Mathf.RoundToInt(transform.position.y);
            int i, j;
            Navigate.CrystalPos.Add(myCrystalPosKeyNum++, posX * MazeCreater.totalCol + posY);
            myCrystalPos.Add(myCrystalPosKeyNum++, posX * MazeCreater.totalCol + posY);
            for(i = -1; i <= 1; i++)
            {
                for (j = -1; j <= 1; j++)
                {
                    Navigate.CrystalSidePos.Add(myCrystalPosKeyNum++, (posX + i) * MazeCreater.totalCol + (posY + j));
                    myCrystalSidePos.Add(myCrystalPosKeyNum++, (posX + i) * MazeCreater.totalCol + (posY + j));
                }
            }
        }

        protected virtual void End()
        {
            foreach(int key in myCrystalPos.Keys)
            {
                Navigate.CrystalPos.Remove(key);
            }
            foreach (int key in myCrystalSidePos.Keys)
            {
                Navigate.CrystalSidePos.Remove(key);
            }
        }

        public virtual void hited()
        {
            mySpriteRendererBottom.enabled = false;
            mySpriteRendererTop.enabled = true;
            /*mySpriteRendererBottom.sprite = breakSprite;
            mySpriteRendererBottom.transform.Translate(Vector3.down);
            mySpriteRendererTop.enabled = false;*/
            for(int i= 0; i < myCollider2d.Length; i++)
            {
                myCollider2d[i].enabled = false;
            }
            crystalLight.enabled = false;
            CircleLight.enabled = false;
            crystalStat = CrystalStat.use;
            End();
        }
    }
}