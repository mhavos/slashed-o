using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace oslashed
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        public Camera uiCam;
        public float currentSpeed = 0.55555555555555555f; //0.6666666666f;

        public bool playTutorial;
        public Transform beatBar;
        public Transform tutorialDialog;
        public TMP_Text tutorialText;
        public TMP_Text nextText;
        public TMP_Text introText;
        public bool canCast;

        public Transform slotA;
        public Transform slotB;

        public List<Enemy> enemies;

        public Player p;
        public event TriggerTutorial triggerTutorialEvent;

        public LevelManager()
        {
            if (instance == null || instance.Equals(null))
                instance = this;
            else
                Destroy(this);
        }

        public bool PASS = true;
        public int tutorialStep;
        public List<string> tutorialPrompts;

        public Dictionary<string, string> spellReactions = new Dictionary<string, string>()
        {
            {
                "poison",
                "Ok I Will Assume This Was An Accident.\\Next Time Make Sure Not To Cast Down Twice And Up On The Last Beat.\\Because I Hate Being Poisoned.\\Did You Think Robots Can Not Be Poisoned Haha That Was A Myth We Spread.\\Poison Will Hurt Us Over Time And Will Pierce Any Defense Which Is Annoying.\\I Guess It Does Take Some Time To Be Effective So It Is Somewhat Balanced."
            },
            {
                "bouba",
                "A Burst Of Water.\\Seriously.\\How Unoriginal.\\But I Mean It Works Well Against Robots I Guess.\\Avoid Casting Left Twice And Right On The Last Beat Please.\\I Would Make Myself Waterproof But That Is A Scrapped Mechanic The Robot Factory Never Went Through WIth The Idea.\\I Have A Better Thing In Mind.\\Force Fields Block Physical Attacks.\\I Will Cover Myself In A Force Field.\\I Will Wait For You To Attack.\\I Will Block.\\Block Your Attack I Mean."
            },
            {
                "kiki",
                "Be Careful You Just Hit What Sounded Like A Human.\\When Using The Eruption Attack Always Be Aware You Are Targeting Everything In Front Of You At Once.\\One Day You Will Cast Up Twice And Down On The Last Beat And Accidentally Hit An Elderly Robot.\\You Know What I Think I Am Going To Start Flying Now.\\If You Cast Eruption Again It Will Not Hit Me Obviously Haha."
            },
            {
                "shielddisarm",
                "What How Did You Breach My Forcefield Mortal.\\Not Like I Can Not Get A New One But It Is Annoying.\\Stop Casting Right Twice And Left On The Last Beat It Is Not Efficient In The Long Run.\\Now Please Excuse Me While There Is A Window Of Vulnerability That Attacks Can Hit Me In.\\Just Wait Until My Force Field Comes Back."
            },
            {
                "shieldlessdisarm",
                "Haha Stupid Organic.\\You Cast A Spell Which Is Completely Useless.\\Seriously I Am Completely Fine Casting Right Twice And Left On The Last Beat Does Nothing.\\What Even Is This Spell Called.\\Breach Forcefields.\\Does Not Seem Important.\\Let Us Resume Your Extermination."
            },
            {
                "shield",
                "Haha You Cast The Spell I Told You To Stupid Organic.\\You Cast The Directions Down With Right And Up In The End Almost As If You Were Raising A Barrier In Front Of You From The Ground Up.\\And a Barrier Is Precisely What You Have Right Now.\\It Will Absorb A Single Hit But Will Disappear After That Measure Ends.\\Except I Am Not Attacking You So You Just Wasted The Spell Completely.\\If You Want To Try Out Another Cool Spell Try Casting Right Down And Then Up On The Last Beat."
            },
            {
                "swap",
                "What.\\Did You Do.\\Do You Like That Human Model More Than Me.\\Do You Prefer Its Company Over Mine.\\Is That Why You Swapped Our Positions.\\Am I Not Funny Enough I Can Download Some More Jokes From The Internet.\\If You Change Your Mind Just Cast Down With Left And Finally Right Again.\\Almost Like A Little Tap Dance That Swaps The Enemies In Front Of You."
            },
            {
                "heal",
                "My Readings Show That Your Healthbar Has Lessened Its Emptiness.\\Did You Have A Hearty Breakfast Or Perhaps Some Vitamin Supplement.\\Oh No You Simply Healed Yourself By Casting Left With Up And In The End Down.\\Retreating From The Battle To Regain Your Footing Sounds Like A Good Idea.\\Make Sure To Heal When Your Health Bar Is Full To Effectively Waste A Measure."
            },
            {
                "stunfront",
                "Hey That Was Uncalled For.\\Can You Not See That I Am Updating.\\You Do Not Have To Stun Me I Am Just Going To Resume The Update Anyway.\\I Would Tell You To Cast Up With Right And Then Left On The Last Beat Some More To Waste Your Time But It Is Really Unpleasant.\\Just Imagine A Robot Walks Up To You Overloads Your Eyes And Leaves What Do You Do.\\Please Waste Your Time Through Some Other Means I Promise I Will Destroy You Soon."
            },
            {
                "stunback",
                "Whatever Spell You Are Casting It Has No Effect.\\I Mean At Least The Human Storage Prototype Does Not Seem Affected.\\I Will Not Count This Attempt The Spell Actually Needs To Do Something."
            },
        };

        public List<Func<bool>> tutorialConditions = new List<Func<bool>>()
        {
            () =>
            {
                return BeatBar.instance.casted.ToList().All(x => x != -2);
            }
        };

        public List<Action> tutorialActions = new List<Action>()
        {
            () =>
            {
                instance.canCast = true;
            }
        };

        private static readonly int Intro = Animator.StringToHash("Intro");

        IEnumerator WaitForNextAction()
        {
            while (!Keyboard.current.anyKey.wasPressedThisFrame)
            {
                yield return null;
            }

            PASS = true;
        }
        
        IEnumerator TutorialPrompt(int u)
        {
            var p = tutorialPrompts[u];
            var listofparts = p.Split('\\');
            foreach (var a in listofparts)
            {
                tutorialText.text = "";
                nextText.enabled = false;tutorialDialog.gameObject.SetActive(true);
                Time.timeScale = 0;
                tutorialDialog.gameObject.SetActive(true);
                for (int i = 0; i < a.Length; i++)
                {
                    tutorialText.text = a.Substring(0, i);
                    yield return new WaitForSecondsRealtime(0.05f);
                }
                yield return new WaitForSecondsRealtime(0.5f);
                nextText.enabled = true;
                yield return StartCoroutine(WaitForNextAction());
            }
            tutorialDialog.gameObject.SetActive(false);
            tutorialActions[u]();
            Time.timeScale = 1;
        }

        IEnumerator Starting()
        {
            yield return StartCoroutine(WaitForNextAction());
            beatBar.LeanMove(new Vector2(0, 40), 0.5f);
            p.musicEmitter.Play();
            p.anim.SetTrigger(Intro);
            introText.enabled = false;
            Instantiate(enemies[0].gameObject, slotA);
            Instantiate(enemies[1].gameObject, slotB);
            yield return new WaitForSeconds(3);
            OnTriggerTutorialEvent(0);
        }

        IEnumerator PlayTutorial()
        {
            for (int i = 0; i < tutorialPrompts.Count; i++)
            {
                StartCoroutine(TutorialPrompt(i));
                while (!tutorialConditions[i]())
                {
                    yield return null;
                }
            }
        }
        
        private void Start()
        {
            triggerTutorialEvent += (s, i) =>
            {
                StartCoroutine(PlayTutorial());
            };
            StartCoroutine(Starting());
        }
        
        protected virtual void OnTriggerTutorialEvent(int i)
        {
            triggerTutorialEvent?.Invoke(this, i);
        }
    }

    public delegate void TriggerTutorial(object sender, int i);
}
