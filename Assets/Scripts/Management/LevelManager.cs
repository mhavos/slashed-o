using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace oslashed
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        public Camera uiCam;
        public float currentSpeed = 0.55555555555555555f;//0.6666666666f;
    
        public LevelManager()
        {
            if (instance == null || instance.Equals(null))
                instance = this;
            else
                Destroy(this);
        }
        
        
    }
}
