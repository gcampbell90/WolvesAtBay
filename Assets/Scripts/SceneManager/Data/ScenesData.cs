using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagerSystem
{
    [CreateAssetMenu(fileName = "AppScenesDB", menuName = "Scene Data/Database")]
    public class ScenesData : ScriptableObject
    {
        public List<Level> levels = new List<Level>();
        public List<Menu> menus = new List<Menu>();

    }
}
