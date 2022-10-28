using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SceneManagerSystem
{
    [CreateAssetMenu(fileName = "New Menu", menuName = "Scene Data/Menu")]
    public class Menu : SceneData
    {
        [Header("MenuSpecific")]
        public MenuType levelType;

        public enum MenuType
        {
            Main_Menu,
            PauseMenu
        }
    }
}
