using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace oslashed
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public Camera uiCam;
    
        public GameManager()
        {
            if (instance == null || instance.Equals(null))
                instance = this;
            else
                Destroy(this);
        }
    }
}
