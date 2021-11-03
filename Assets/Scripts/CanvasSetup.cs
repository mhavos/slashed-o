using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace oslashed
{
    public class CanvasSetup : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = GameManager.instance.uiCam;
        }
    }
}
