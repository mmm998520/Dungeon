using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttackLine : MonoBehaviour
    {
        float insTimer, destroyTimer;
        public Transform startPlayer, endPlayer;
        public GameObject attackPicture;
        public List<Collider2D> attackedColliders = new List<Collider2D>();

        void Start()
        {

        }

        void Update()
        {
            if ((destroyTimer += Time.deltaTime) >= 0.5f)
            {
                Destroy(gameObject);
            }
            if ((insTimer += Time.deltaTime) >= 0.1f)
            {
                insTimer = 0;
                draw();
            }
        }

        public void draw()
        {
            float angle = Vector3.SignedAngle(startPlayer.right, endPlayer.position - startPlayer.position, Vector3.forward);
            PlayerAttackLineUnit playerAttackLineUnit = Instantiate(attackPicture, startPlayer.position, Quaternion.Euler(0, 0, angle)).GetComponent<PlayerAttackLineUnit>();
            playerAttackLineUnit.endPlayer = endPlayer;
            playerAttackLineUnit.playerAttackLine = this;
        }
    }
}