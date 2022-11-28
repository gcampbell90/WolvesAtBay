using SceneManagerSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //GM instance
    static GameManager _instance;
    protected GameManager() { }

    //Scene Data to load into scene GMLevels base class GM
    [SerializeField] ScenesData scenesData;

    //Might be nice to keep track of these and present to the user at the end of the experience using a UI canvas?
    //Yes, also the GMLevel scripts are tracking those during play, so its good they are stored here even after those levels will be destroyed.
    public int Level1TotalEnemiesKilled;
    public int Level2TotalEnemiesKilled;
    public int Level3TotalEnemiesKilled;

    static public bool isActive
    {
        get
        {
            return GameManager._instance != null;
        }
    }

    // Singleton pattern implementation
    public static GameManager Instance
    {
        get
        {
            if (GameManager._instance == null)
            {
                GameManager._instance = Object.FindObjectOfType(typeof(GameManager)) as GameManager;

                if (GameManager._instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    DontDestroyOnLoad(go);
                    GameManager._instance = go.AddComponent<GameManager>();
                }
            }
            return GameManager._instance;
        }
    }

    private void Awake()
    {
        SceneController.SetScenesData(scenesData);
        LoadScene(1);
    }

    public void LoadScene(int sceneID)
    {
        SceneController.LoadScene(this, sceneID);
        //Destroy(sceneController);
    }

    public void ActivateScene()
    {
        SceneController.ActivateScene();
    }
}
