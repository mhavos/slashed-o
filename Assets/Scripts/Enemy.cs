using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace oslashed
{
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour
    {
        public int health;
        public int maxHealth;
        public HealthBar hb;

        public float attackProbability;
        public Animator anim;

        public int arrowType;
        public bool floating;
        public bool stunned;
        public bool isAbleToAttack;
        
        public int attackStatus;
        private static readonly int Dir = Animator.StringToHash("dir");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private List<Image> signals;
        
        void Start()
        {
            anim = GetComponent<Animator>();
            signals = BeatBar.instance.targetImages.GetRange(9, 4);
        }

        void Update()
        {
            
        }

        public bool Beat(bool mayBeginAttack)
        {
            if (isAbleToAttack)
            {
                //if the enemy is not charging an attack:
                if (mayBeginAttack & attackStatus == 0)
                {
                    //roll for attack
                    if (UnityEngine.Random.value < attackProbability)
                    {
                        Debug.Log("anger");
                        arrowType = UnityEngine.Random.Range(0, 4);
                        signals[BeatBar.instance.actualBeat - 1].sprite = BeatBar.instance.arrackArrows[arrowType];
                        anim.SetInteger(Dir, arrowType);
                        anim.SetTrigger(Attack1);
                        attackStatus = 4;
                        return false;
                    }
                }

                //on the beat the attack hits:
                if (attackStatus == 1)
                {
                    signals[BeatBar.instance.actualBeat].sprite = BeatBar.instance.emptyArrow;
                    BeatBar.instance.EnemyAttack(this, arrowType);
                }

                if (attackStatus > 0)
                {
                    attackStatus--;
                }
            }

            return true;
        }

        void Die()
        {
            //sipka pod indikatorom zmizne
        }

        void Attack()
        {
            
        }
    }
}
