using SceneManagerSystem;
using System;
using UnityEngine;

public class ButtonTesterClass : MonoBehaviour
{
    private void Start()
    {
        Action<bool> myDelegate = this.buttonFunction1;
        UI_Controller.CreateButton("Load Scene 1", buttonFunction1);

        myDelegate = this.buttonFunction2;
        UI_Controller.CreateButton("Load Player", myDelegate);
        myDelegate = this.buttonFunction3;
        UI_Controller.CreateButton("Load Load Allies", myDelegate);

    }

    private void buttonFunction1(bool value)
    {
        if (value) { SceneController.LoadScene(this,2); }
        else
        {
            SceneController.UnloadScene(this,1);
        }
        Debug.Log("Load Scene");
    }
    private void buttonFunction2(bool value)
    {
        Debug.Log("Load Player");
    }
    private void buttonFunction3(bool value)
    {
        Debug.Log("Load Allies");
    }
}