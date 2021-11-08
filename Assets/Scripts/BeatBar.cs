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
        public List<Sprite> arrackArrows;
        public Sprite emptyArrow;

        public Transform[] enemySlots = new Transform[2];
        
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
            if (mil > 60000/tempo - realThresholdInMillis)
            {
                if (!toggle && beat == 1)
                {
                    Toggle();
                    toggle = true;
                }

            }else if (mil < realThresholdInMillis){
                
                Highlight(((beat+2)%4)+1);
                    
            }
            else
            {
                Highlight(-1);
                toggle = false;
            }

            if (mil > 60000 / tempo / 2 && !beatToggle)
                {
                    actualLastBeatTime = Time.time;
                    actualBeat = beat;
                    beatToggle = true;
                    EnemyAct();
                }

        }

        public void Toggle()
        {
            targetImages.GetRange(5,4).ForEach(x => x.sprite = emptyArrow);
            ResolveSpell();
        }

        public void Highlight(int beat)
        {
            if (beat == -1)
            {
                targetImages.GetRange(1,4).ForEach(x => x.sprite = deactivated);
                return;
            }
            targetImages[(beat + 2) % 4 +1].sprite = deactivated;
            targetImages[beat].sprite = activated;
        }

        public void ResolveSpell()
        {
            
            casted = new []{-2, -2, -2, -2};
        }

        public void EnemyAct()
        {
            //all enemies are iterated over, front one first
            bool mayBeginAttack = true;
            for(int i = 0; i < enemySlots.Length; i++)
            {
                Transform slot = enemySlots[i];
                if(slot.childCount > 0){
                    GameObject child = slot.GetChild(0).gameObject;
                    mayBeginAttack = child.GetComponent<Enemy>().Beat(mayBeginAttack);
                }
            }

        }

        public void EnemyAttack(Enemy attacker, int direction)
        {
                //forward the OnAttack(Enemy attacker,int direction) function to player if last note does not counter
        }
    } 
}
