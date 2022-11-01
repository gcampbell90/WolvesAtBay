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

        //TODO: Move logic to separate method to call in 'awake' function - as it is assigning references between other scripts(from an SO in this case) and should be done in awake than start
        void Awake()
        {
            //Create Scene IDs
            var myObject = scriptableObject.levels;
            int count = 0;
            foreach (var level in myObject)
            {
                level.Id = count++;

                Debug.Log($"Scene controller created reference to scene: ID: {level.Id} Name: {level.name}");
            }
            LoadSceneAsync(1);
        }

        public void LoadSceneAsync(int id)
        {
            string sceneName = scriptableObject.levels[id].name;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}
