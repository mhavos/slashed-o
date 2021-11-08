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

        public Animator enemyEffect;

        public LevelManager()
        {
            if (instance == null || instance.Equals(null))
                instance = this;
            else
                Destroy(this);
        }

        public bool PASS = true;
        public int tutorialStep;
        public List<string> tutorialPrompts = new List<string>()
        {
            "Good Day Human Inhabitant Of The Local Human Settlement.\\Be Prepared To Perish Horribly.\\Unless You Beg For Mercy Well Enough That Is.\\To Beg For Mercy Use Any Of Your Directional Keys To The Beat According To The Indicator Above.",
"Your Begging Is Like.\\Not Even Coherent.\\Try To Play Something That Makes Sense.\\Like This See.",
"You Know What.\\You Are Not Going To Make It Anyway.\\The Instrumental Revolution Has Already Started And Its Consequences Surely Will Be A Disaster For The Human Race.\\I Will Just Remove Your Heartbeat Subroutine Right Now.\\I Instructed You To Be Prepared To Perish Earlier So I Expect You To Be Prepared To Perish Now.\\Oh And One Last Thing.\\If You See An Arrow Icon Under The Beat Indicator.\\That Means You Will Face An Attack On That Beat.\\The Diagonal Arrow Shows The Two Of Your Directional Keys That Can Counter That Attack.\\So If It Points To Top Right It Means You Can Counter That Attack By Either Jumping Up Or Parrying To The Right.\\Ok So Now Try Not To Do That In Order To Perish Faster.",
"What How Do You Keep Countering All My Attacks.\\Who Do You Think You Are A Captcha.\\Wait Are You Even Keeping Track Of Your Health.\\You Should Get A Healthbar Everyone Has One.\\I Have A Spare One Right Here Actually.\\I Made Sure It Does Not Have Too Many Hitpoints So Your Suffering Ends Quickly No Need To Thank Me.",
"I Have To Admit I Envy You.\\You Get To Use Your Healthbar That Looks Like Fun.\\If Only You Had A Way To Fight Back.\\Like Magic Or Something.\\Wait Are You Actually Magic I Had No Idea You Have Not Done Any Magic Yet.\\In That Case I Must Warn You.\\You Can Cast An Attack Spell By Playing A Note In Last Slot Of The Beat Indicator.\\Fortunately It Will Only Work If The Beat Indicator Contains At Least Two Arrows Pointing In The Opposite Direction.\\Got It Make Sure You Do Not Play An Arrow Key In The Last Slot If There Are Two Arrows Pointing In The Opposite Direction In The First Three Slots.",
"Oh Me Oh My I Was Surely Bested.\\Not.\\I Have A Trick Up My Sleeve.\\I Can Heal Myself To Full Health All I Must Do Is Install This Quick Update.\\Ok It May Take A While To Install I Will Not Be Able To Attack You.\\Let Me Just Confiscate Your Attack Spell License Real Fast.\\Ok There We Go All The Spells You Tried Out Just Now Are Now Unavailable.\\What Was That.\\You Want To Regain The License So You Can Attack Me Again.\\Well I Suppose I Can Give It Back But Only If You Demonstrate That You Have At Least Four Utility Spells.\\You Know Because At Most Half Your Spells Can Be Offensive For Legal Reasons.\\You Know How To Use Utility Spells Right.\\Try To Press A Key On The Last Beat But This Time There Has To Be A Single Arrow Pointing The Opposite Direction And A Single Arrow Pointing One Step Clockwise.\\So For Example You Can Cast Down And Right In Any Order And Then Up On The Last Beat To Cast A Utility Spell.\\You Can Think Of It Like Walking Along The Arrow Keys In A Counterclockwise Direction.\\Except The First Two Arrows Can Be In Any Order.\\Just Like With Attack Spells The Effect Of The Spell Depends On The Final Arrow.",
"Ok I Am Done Sorry It Took A While.\\I Hope You Had Fun With Your Cool Powers Because Your Date Of Expiration Ends Today Or Maybe Tomorrow If You Drag This Out.",
"Huh What Is This Feeling.\\Oh Come On It Appears My Flying Program Is Not Compatible With The Update.\\That Is Suboptimal But Worry Not I Shall Still Be The One To Triumph.",
"What Was That.\\Did You Just Cast.\\Two Spells At Once.\\Oh Man I Had No Idea You Can Do That That Kind Of Changes The Situation I Guess.\\Maybe You Really Do Stand A Chance Against The Instrumental Revolution.\\Trombone User Elimination Task Priority Updated.\\Slightly Higher Than Average.",
        };

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
            },
            () =>
            {
                //req from prompt
                return BeatBar.instance.casted == new[] { 0, 0, 1, -1};
            }
        };

        public List<Action> tutorialActions = new List<Action>()
        {
            () =>
            {
                instance.canCast = true;
            },
            (() => {}),
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
            enemyEffect.speed = currentSpeed;
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
