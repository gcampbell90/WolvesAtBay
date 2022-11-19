using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ensure class initializer is called whenever scripts recompile
[InitializeOnLoadAttribute]
public static class ApplicationStateManager 
{
    //only in dev mode?
    public static bool playMode = true;

    // register an event handler when the class is initialized
    static ApplicationStateManager()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        playMode = state == PlayModeStateChange.EnteredPlayMode;
        ////Debug.Log(state + " InPlayMode?" + playMode);
    }
}
