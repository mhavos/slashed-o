using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace oslashed
{
    public class Player : MonoBehaviour
    {
        public FMODUnity.StudioEventEmitter emitter;
        public FMODUnity.StudioEventEmitter musicEmitter;
        public SpriteRenderer sr;
        public int state = 0;

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
            var value = context.ReadValue<Vector2>();
            if (value == Vector2.zero)
            {
                sr.color = Color.white;
                return;
            }
            sr.color = Color.red;
            emitter.SetParameter("Direction", VectorConversion(value));
            emitter.PlayInstance();
            var mil = (Time.time - BeatBar.instance.lastBeatTime) * 1000;
            if (mil > 300) mil = 600 - mil;
            Debug.Log(mil);
        }
        
        //musicEmitter.SetParameter("Progression", ++state);
    }
}
