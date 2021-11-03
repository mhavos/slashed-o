using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace oslashed
{
    public class MenuManager : MonoBehaviour
    {
        public void StartGame() {
            TransitionManager.instance.LoadScene(SceneIndexes.Game);
        }

        public void ToggleSettings()
        {
            
        }

        public void OpenWeb()
        {
            Application.OpenURL("https://www.smnd.sk/mikey/PHP/spongia/spongia_2021/");
        }

        public void ToggleSound()
        {
            
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
