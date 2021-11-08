using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            
        }

        public bool Beat(bool mayBeginAttack)
        {
            //if the enemy is not charging an attack:
            if(mayBeginAttack & attackStatus == 0){
                //roll for attack
                if(UnityEngine.Random.value < attackProbability){
                    Debug.Log("anger");
                    arrowType = UnityEngine.Random.Range(0,4);
                    //objavi sa sipka pod indikatorom
                    anim.SetInteger("dir",arrowType);
                    anim.SetTrigger("Attack");
                    attackStatus = 4;
                    return false;
                }
            }
            //on the beat the attack hits:
            if(attackStatus == 1){
                //sipka pod indikatorom zmizne
                BeatBar.instance.EnemyAttack(this,arrowType);
            }
            if(attackStatus > 0){
                attackStatus--;
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
