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
        
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            
        }

        void Die()
        {
            
        }

        void Attack()
        {
            
        }
    }
}
