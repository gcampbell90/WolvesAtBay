using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SceneManagerSystem
{
    public class SceneData : ScriptableObject
    {
        [Header("Information")]
        public string sceneName;

        //[Header("Sounds")]
        //[Header("Visuals")]
        //public PostProcessProfile postProcess;

        public int Id { get; set; }
    }
}
