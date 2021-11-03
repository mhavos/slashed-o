using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace oslashed
{
    public class BeatBar : MonoBehaviour
    {
        public static BeatBar instance;
        EventDescription ed;
        EVENT_CALLBACK cb;
        private List<Image> targetImages;
        public Sprite activated;
        public Sprite deactivated;
        public float tempo;
        public float lastBeatTime;
        public float BEAT_COMPLETION_THRESHOLD = 0.25f;
        public float realThresholdInMillis;
        
        public BeatBar()
        {
            if (instance == null || instance.Equals(null))
                instance = this;
            else
                Destroy(this);
        }
        
        private void Awake()
        {
            RuntimeManager.StudioSystem.getEvent("event:/tutorialTheme", out ed);
            cb = StudioEventCallback;
            ed.setCallback(cb, EVENT_CALLBACK_TYPE.TIMELINE_BEAT);
            targetImages = GetComponentsInChildren<Image>().ToList();
        }

        public RESULT StudioEventCallback(EVENT_CALLBACK_TYPE type, IntPtr eventInstance, IntPtr parameters)
        {
            if (type == EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
            {
                TIMELINE_BEAT_PROPERTIES beat = (TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameters, typeof(TIMELINE_BEAT_PROPERTIES));
                try
                {
                    Toggle(beat.beat);
                }
                catch (Exception e)
                {
                    // ignored
                }

                lastBeatTime = Time.time;
                tempo = beat.tempo;
                realThresholdInMillis = 60000 / tempo * BEAT_COMPLETION_THRESHOLD;
            }
            return RESULT.OK;
        }

        public void Toggle(int beat)
        {
            targetImages[(beat + 2) % 4 +1].sprite = deactivated;
            targetImages[beat].sprite = activated;
        }
    } 
}
