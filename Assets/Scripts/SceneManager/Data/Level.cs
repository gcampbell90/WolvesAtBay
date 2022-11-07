using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace SceneManagerSystem
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Scene Data/Level")]
    public class Level : SceneData
    {
        [Header("Level Type")]
        public LevelType levelType;

        //Settings specific to level only
        [Header("Level specific")]
        public int enemyCount;

        //[SerializeField]
        //GameObject ColliderObject;
        //Collider sceneTriggerCollider;


        //private void Awake()
        //{
        //    sceneTriggerCollider = ColliderObject.GetComponent<Collider>();
        //}


    }

    public enum LevelType
    {
        Playable,
        CutScene
    }
}
