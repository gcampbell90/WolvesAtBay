using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagerSystem
{
    public class SceneController : MonoBehaviour
    {
        public ScenesData scriptableObject;
        //public int currLevelIndex;

        void Awake()
        {
            CreateSceneID();
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

        //public void NextLevelTriggered()
        //{
        //    currLevelIndex++;
        //    LoadSceneAsync(currLevelIndex);
        //}

        public void LoadSceneAsync(int id)
        {
            StartCoroutine(LoadingSceneAsync(id));
        }

        public IEnumerator LoadingSceneAsync(int id)
        {
            string sceneName = scriptableObject.levels[id].name;

            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (sceneLoad.progress != 1f)
            {
                Debug.Log("Waiting..." + sceneLoad.progress);
                yield return null;
            }

            var myScene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(myScene);
        }
    }
}
