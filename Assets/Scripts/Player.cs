using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace oslashed
{
    public class Player : MonoBehaviour
    {
        public int health;
        public int maxHealth;
        public int shield;
        
        public FMODUnity.StudioEventEmitter emitter;
        public FMODUnity.StudioEventEmitter musicEmitter;
        public SpriteRenderer sr;
        public int state = 0;
        public Animator anim;
        private static readonly int Down = Animator.StringToHash("Down");
        private static readonly int Left = Animator.StringToHash("Left");
        private static readonly int Up = Animator.StringToHash("Up");
        private static readonly int Right = Animator.StringToHash("Right");
        private static readonly int Hurt = Animator.StringToHash("Hurt");

        private int stash = -2;

        private PlayerInput pi;
        private void Start()
        {
            anim = GetComponent<Animator>();
            anim.speed = LevelManager.instance.currentSpeed;
            pi = GetComponent<PlayerInput>();
        }

        private int VectorConversion(Vector2 v)
        {
            if (v == Vector2.down) return -1;
            if (v == Vector2.left) return 0;
            if (v == Vector2.up) return 1;
            if (v == Vector2.right) return 2;
            return 3;
        }
        
        [ContextMenu("Cast")]
        public void OnCast(InputAction.CallbackContext context)
        {
            if (!LevelManager.instance.canCast) return;   
            var value = context.ReadValue<Vector2>();
            if (value == Vector2.zero)
            {
                sr.color = Color.white;
                return;
            }
            if (!context.performed)
            {
                stash = -2;
            }
            
            var val = VectorConversion(value);
            var beat = BeatBar.instance.actualBeat;
            sr.color = Color.red;
        
            // if spell slot available
            if (BeatBar.instance.casted[beat - 1] == -2)
            {
                // FIGURE BOUNDS
                var mil = (Time.time - BeatBar.instance.actualLastBeatTime) * 1000;
                var thresh = BeatBar.instance.realThresholdInMillis;
                
                // LOGIC
                if (BeatBar.instance.casted[beat - 1] != 3 && mil > (60000/BeatBar.instance.tempo - 2*thresh)/2 && mil < (60000/BeatBar.instance.tempo - 2*thresh)/2 + 2*thresh)
                {
                    BeatBar.instance.casted[beat - 1] = val;
                    BeatBar.instance.targetImages[beat + 4].sprite = BeatBar.instance.arrows[val + 1];
                }
                else
                {
                    val = 3;
                }

                Debug.Log(mil);
            
                // ANIMATIONS
                switch (val)
                {
                    case -1:
                        anim.SetTrigger(Down);
                        break;
                    case 0:
                        anim.SetTrigger(Left);
                        break;
                    case 1:
                        anim.SetTrigger(Up);
                        break;
                    case 2:
                        anim.SetTrigger(Right);
                        break;
                }
            }
            else
            {
                val = 3;
            }
            
            // SOUND
            emitter.SetParameter("Direction", stash != -2 ? stash : val);
            emitter.PlayInstance();
            
            stash = val;
        }

        
        
        private void Update()
        {
            
        }

        //musicEmitter.SetParameter("Progression", ++state);
    }
}
