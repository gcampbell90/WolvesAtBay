using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif

namespace SceneManagerSystem
{
    public class SceneController : MonoBehaviour
    {
        private static bool allowActivation = false;

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

        static IEnumerator LoadSceneAsync(int id)
        {
            //Debug.Log("Load Scene called");
            //string sceneName = SceneDataList.levels[id].name;
            yield return null;
            float timer = 0;

            AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);
            sceneLoad.allowSceneActivation = true;

            while (sceneLoad.progress < 0.9f)
            {
                timer += Time.deltaTime;
                Debug.Log($"Loading..{sceneLoad.progress}");
                yield return null;
                Debug.Log($"LoadingTime:{timer}");
            }

            while (!allowActivation)
            {
                Debug.Log("Loaded. Press button to activate" + sceneLoad.progress);
                yield return null;
            }
            sceneLoad.allowSceneActivation = true;
            Debug.Log("Loaded." + sceneLoad.isDone);



            //var myScene = SceneManager.GetSceneByName(sceneName);
            //SceneManager.SetActiveScene(myScene);
        }

            static IEnumerator UnloadSceneAsync(int id)
        {
            //Debug.Log("Load Scene called");
            //string sceneName = SceneDataList.levels[id].name;
            yield return null;

            AsyncOperation sceneLoad = SceneManager.UnloadSceneAsync(id);

            while (sceneLoad.progress < 0.9f)
            {
                Debug.Log("Unloading" + sceneLoad.progress);

                yield return null;
            }

            Debug.Log("Unloaded." + sceneLoad.isDone);

        }

        public static void LoadScene(MonoBehaviour context, int id)
        {
            context.StartCoroutine(LoadSceneAsync(id));
        }
        public static void UnloadScene(MonoBehaviour context, int id)
        {
            context.StartCoroutine(UnloadSceneAsync(id));
        }

        public static void ActivateScene()
        {
            allowActivation = true;
        }
    }
}
