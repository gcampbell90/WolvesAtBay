using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    private static RectTransform _rectTransform;
    private static GameObject _togglePrefab;

    static int count = 0;

    private void Awake()
    {
        //Do awake stuff
        _togglePrefab = Resources.Load("Prefabs/Button") as GameObject;
        _rectTransform = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>();
    }
  
    //Create buttons for stuff we need
    public static void CreateButton(string title, Action<bool> function)
    {
        count++;
        var m_toggle = Instantiate(_togglePrefab, _rectTransform);
        var m_btnTitle = $"{count}-{title}";
        m_toggle.name = m_btnTitle;
        m_toggle.GetComponentInChildren<TextMeshProUGUI>().text = m_btnTitle;

        var toggle = m_toggle.GetComponent<Toggle>();

        //var parent = m_toggle.transform.parent;
        //var group = parent.GetComponent<ToggleGroup>();
        //toggle.group = group;

        toggle.onValueChanged.AddListener((value) =>
        {
            function(value);
            var toggleCol = toggle.colors;
            toggleCol.normalColor = value ?  new Color(33, 156, 32) : new Color(0.5f, 0.5f, 0.5f);
            toggleCol.highlightedColor = value ? new Color(33, 156, 32) : new Color(0.5f, 0.5f, 0.5f) ;
            toggleCol.pressedColor = value ? new Color(33, 156, 32) : new Color(0.5f, 0.5f, 0.5f);
            toggleCol.selectedColor = value ? new Color(33, 156, 32) : new Color(0.5f, 0.5f, 0.5f);
            toggle.colors = toggleCol;
           
        });
    }


}
