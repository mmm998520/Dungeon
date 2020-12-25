using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TractorBeam : MonoBehaviour
    {
        public bool elongation = true;//是否在伸長狀態
        public float speed = 10f;
        public Transform target = null;
        public TractorBeamGen tractorBeamGen;
        public GameObject tractorBeamSide;

        void Start()
        {

        }

        void Update()
        {
            if (elongation)
            {
                GetComponent<Rigidbody2D>().velocity = transform.right * speed;
                //transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = transform.right * -speed;
                //transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
            if (target != null)
            {
                target.transform.position = transform.GetChild(0).position;
                target.GetComponent<PlayerManager>().HardStraightTimer = 0;
            }
        }
        public int times = 0;
        public bool canBack = false;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                target = collider.transform;
            }

            if (collider.gameObject.layer == 8 || (canBack && collider.gameObject.layer == 12))
            {
                elongation = false;
            }
        }
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject == tractorBeamSide)
            {
                canBack = true;
            }
        }
        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject == tractorBeamSide && !elongation)
            {
                tractorBeamGen.stat = TractorBeamGen.TractorBeamStat.non;
                tractorBeamGen.transform.position = Vector3.right * 100;
                transform.localPosition = Vector3.right * -15;
                times = 0;
                target = null;
                canBack = false;
                elongation = true;
            }
        }
    }
}