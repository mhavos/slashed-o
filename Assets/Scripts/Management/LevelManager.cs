using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace oslashed
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
        public Camera uiCam;
        public float currentSpeed = 0.55555555555555555f;//0.6666666666f;

        public bool playTutorial;
        public Transform tutorialDialog;
        public TMP_Text tutorialText;
        public TMP_Text nextText;
        
        public LevelManager()
        {
            if (instance == null || instance.Equals(null))
                instance = this;
            else
                Destroy(this);
        }

        public int tutorialStep;
        public List<string> tutorialPrompts;
        public Dictionary<string, string> spellReactions = new Dictionary<string, string>()
        {
            {"poison", "Ok I Will Assume This Was An Accident.\nNext Time Make Sure Not To Cast Down Twice And Up On The Last Beat.\nBecause I Hate Being Poisoned.\nDid You Think Robots Can Not Be Poisoned Haha That Was A Myth We Spread.\nPoison Will Hurt Us Over Time And Will Pierce Any Defense Which Is Annoying.\nI Guess It Does Take Some Time To Be Effective So It Is Somewhat Balanced."},
            {"bouba", "A Burst Of Water.\nSeriously.\nHow Unoriginal.\nBut I Mean It Works Well Against Robots I Guess.\nAvoid Casting Left Twice And Right On The Last Beat Please.\nI Would Make Myself Waterproof But That Is A Scrapped Mechanic The Robot Factory Never Went Through WIth The Idea.\nI Have A Better Thing In Mind.\nForce Fields Block Physical Attacks.\nI Will Cover Myself In A Force Field.\nI Will Wait For You To Attack.\nI Will Block.\nBlock Your Attack I Mean."},
            {"kiki", "Be Careful You Just Hit What Sounded Like A Human.\nWhen Using The Eruption Attack Always Be Aware You Are Targeting Everything In Front Of You At Once.\nOne Day You Will Cast Up Twice And Down On The Last Beat And Accidentally Hit An Elderly Robot.\nYou Know What I Think I Am Going To Start Flying Now.\nIf You Cast Eruption Again It Will Not Hit Me Obviously Haha."},
            {"shielddisarm", "What How Did You Breach My Forcefield Mortal.\nNot Like I Can Not Get A New One But It Is Annoying.\nStop Casting Right Twice And Left On The Last Beat It Is Not Efficient In The Long Run.\nNow Please Excuse Me While There Is A Window Of Vulnerability That Attacks Can Hit Me In.\nJust Wait Until My Force Field Comes Back."},
            {"shieldlessdisarm", "Haha Stupid Organic.\nYou Cast A Spell Which Is Completely Useless.\nSeriously I Am Completely Fine Casting Right Twice And Left On The Last Beat Does Nothing.\nWhat Even Is This Spell Called.\nBreach Forcefields.\nDoes Not Seem Important.\nLet Us Resume Your Extermination."},
            {"shield", "Haha You Cast The Spell I Told You To Stupid Organic.\nYou Cast The Directions Down With Right And Up In The End Almost As If You Were Raising A Barrier In Front Of You From The Ground Up.\nAnd a Barrier Is Precisely What You Have Right Now.\nIt Will Absorb A Single Hit But Will Disappear After That Measure Ends.\nExcept I Am Not Attacking You So You Just Wasted The Spell Completely.\nIf You Want To Try Out Another Cool Spell Try Casting Right Down And Then Up On The Last Beat."},
            {"swap", "What.\nDid You Do.\nDo You Like That Human Model More Than Me.\nDo You Prefer Its Company Over Mine.\nIs That Why You Swapped Our Positions.\nAm I Not Funny Enough I Can Download Some More Jokes From The Internet.\nIf You Change Your Mind Just Cast Down With Left And Finally Right Again.\nAlmost Like A Little Tap Dance That Swaps The Enemies In Front Of You."},
            {"heal", "My Readings Show That Your Healthbar Has Lessened Its Emptiness.\nDid You Have A Hearty Breakfast Or Perhaps Some Vitamin Supplement.\nOh No You Simply Healed Yourself By Casting Left With Up And In The End Down.\nRetreating From The Battle To Regain Your Footing Sounds Like A Good Idea.\nMake Sure To Heal When Your Health Bar Is Full To Effectively Waste A Measure."},
            {"stunfront", "Hey That Was Uncalled For.\nCan You Not See That I Am Updating.\nYou Do Not Have To Stun Me I Am Just Going To Resume The Update Anyway.\nI Would Tell You To Cast Up With Right And Then Left On The Last Beat Some More To Waste Your Time But It Is Really Unpleasant.\nJust Imagine A Robot Walks Up To You Overloads Your Eyes And Leaves What Do You Do.\nPlease Waste Your Time Through Some Other Means I Promise I Will Destroy You Soon."},
            {"stunback", "Whatever Spell You Are Casting It Has No Effect.\nI Mean At Least The Human Storage Prototype Does Not Seem Affected.\nI Will Not Count This Attempt The Spell Actually Needs To Do Something."},
        };
        public List<Action> tutorialConditions;
        public List<Action> TutorialActions;
        
        private void Start()
        {
            throw new NotImplementedException();
        }
    }
}
