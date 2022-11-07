using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    protected GameManager() { }

    //Might be nice to keep track of these and present to the user at the end of the experience using a UI canvas?
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
}