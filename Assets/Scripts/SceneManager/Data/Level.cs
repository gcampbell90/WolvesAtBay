using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagerSystem
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Scene Data/Level")]
    public class Level : SceneData
    {
        [Header("Level Type")]
        public LevelType levelType;

        //Settings specific to level only
        //[Header("Level specific")]
        //public int enemiesCount;
    }

    public enum LevelType
    {
        Playable,
        CutScene
    }
}
