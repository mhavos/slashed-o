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
        public List<Image> targetImages;
        public Sprite activated;
        public Sprite deactivated;
        public float tempo;
        public float lastBeatTime;
        public float actualLastBeatTime;
        public float BEAT_COMPLETION_THRESHOLD = 0.25f;
        public float realThresholdInMillis;
        public int beat;
        public int actualBeat;
        public int[] casted = new int[] {-2, -2, -2, -2 };
        
        public List<Sprite> arrows;
        public Sprite emptyArrow;
        
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
                try
                {
                    TIMELINE_BEAT_PROPERTIES beat =
                        (TIMELINE_BEAT_PROPERTIES) Marshal.PtrToStructure(parameters, typeof(TIMELINE_BEAT_PROPERTIES));
                    lastBeatTime = Time.time;
                    tempo = beat.tempo;
                    realThresholdInMillis = 60000 / tempo * BEAT_COMPLETION_THRESHOLD;
                    this.beat = beat.beat;
                    beatToggle = false;
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
            return RESULT.OK;
        }

        private bool toggle;
        private bool beatToggle;
        private void Update()
        {
            var mil = (Time.time - lastBeatTime) * 1000;
            if (mil < realThresholdInMillis || mil > 60000/tempo - realThresholdInMillis)
            {
                if (!toggle)
                {
                    Toggle(beat);
                    toggle = true;
                }
            }
            else
            {
                 Toggle(-1);
                 toggle = false;
            }

            if (mil > 60000 / tempo / 2 && !beatToggle)
            {
                actualLastBeatTime = Time.time;
                actualBeat = beat;
                beatToggle = true;
            }
        }

        public void Toggle(int beat)
        {
            if (beat == -1)
            {
                targetImages.GetRange(1,4).ForEach(x => x.sprite = deactivated);
                return;
            }

            if (beat == 1)
            {
                targetImages.GetRange(5,4).ForEach(x => x.sprite = emptyArrow);
                ResolveSpell();
            }
            targetImages[(beat + 2) % 4 +1].sprite = deactivated;
            targetImages[beat].sprite = activated;
        }

        public void ResolveSpell()
        {
            
            casted = new []{-2, -2, -2, -2};
        }
    } 
}
