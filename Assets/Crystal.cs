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

        protected enum CrystalStat
        {
            unUse,
            use,
        }
        protected CrystalStat crystalStat = CrystalStat.unUse;

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            mySpriteRendererBottom.sprite = breakSprite;
            mySpriteRendererBottom.transform.Translate(Vector3.down);
            mySpriteRendererTop.enabled = false;
            for(int i= 0; i < myCollider2d.Length; i++)
            {
                myCollider2d[i].enabled = false;
            }
            crystalLight.enabled = false;
            crystalStat = CrystalStat.use;
        }
    }
}