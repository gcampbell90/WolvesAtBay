using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR 
using UnityEditor.SearchService;
#endif

namespace SceneManagerSystem
{
    public class SceneController : MonoBehaviour
    {
        public static ScenesData SceneDataList { get; private set; }

        public static void SetScenesData(ScenesData scenesData)
        {
            SceneDataList = scenesData;
            CreateSceneIDs();
        }


        public static void CreateSceneIDs()
        {
            //Create Scene IDs
            var myObject = SceneDataList.levels;
            int count = 0;
            foreach (var level in myObject)
            {
                level.Id = count++;

                //Debug.Log($"Scene controller created reference to scene: ID: {level.Id} Name: {level.name}");
            }
        }

        //public void NextLevelTriggered()
        //{
        //    currLevelIndex++;
        //    LoadSceneAsync(currLevelIndex);
        //}

        public static IEnumerator LoadSceneAsync(int id)
        {
            //Debug.Log("Load Scene called");
            string sceneName = SceneDataList.levels[id].name;

            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (sceneLoad.progress != 1f)
            {
                yield return null;
            }

            var myScene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(myScene);
        }

        public void LoadScene(int id)
        {
            StartCoroutine(SceneController.LoadSceneAsync(id));
        }
    }
}
