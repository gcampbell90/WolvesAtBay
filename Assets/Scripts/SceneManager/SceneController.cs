using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagerSystem
{
    public class SceneController : MonoBehaviour
    {
        public ScenesData scriptableObject;
        public int currLevelIndex;

        void Awake()
        {
            CreateSceneID();
        }

        private void Start()
        {
            currLevelIndex = 0;
        }

        public void CreateSceneID()
        {
            //Create Scene IDs
            var myObject = scriptableObject.levels;
            int count = 0;
            foreach (var level in myObject)
            {
                level.Id = count++;

                Debug.Log($"Scene controller created reference to scene: ID: {level.Id} Name: {level.name}");
            }
            //LoadSceneAsync(1);
        }

        public void NextLevelTriggered()
        {
            currLevelIndex++;
            LoadSceneAsync(currLevelIndex);
        }

        public void LoadSceneAsync(int id)
        {
            string sceneName = scriptableObject.levels[id].name;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}
