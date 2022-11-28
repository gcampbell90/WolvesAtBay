//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using static UnityEngine.GraphicsBuffer;

//[CustomEditor(typeof(AllyController))]
//public class AllyControllerEditor : Editor
//{
//    void OnSceneGUI()
//    {
//        AllyController allyController = (AllyController)target;

//        if (allyController == null)
//        {
//            return;
//        }

//        Handles.color = Color.blue;
//        Handles.Label(allyController.transform.position + Vector3.up * 2,
//            allyController.transform.position.ToString() + "\nShieldArea: " +
//            allyController.shieldArea.ToString());

//        Handles.BeginGUI();
//        if (GUILayout.Button("Reset Area", GUILayout.Width(100)))
//        {
//            allyController.shieldArea = 5;
//        }
//        Handles.EndGUI();

//        foreach (var follower in allyController.Allies)
//        {
//            Handles.DrawWireArc(follower.transform.position,
//                allyController.transform.up,
//                -allyController.transform.right,
//                180,
//                allyController.shieldArea);
//        }

//        //Handles.DrawWireArc(allyController.transform.position,
//        //    allyController.transform.up,
//        //    -allyController.transform.right,
//        //    180,
//        //    allyController.shieldArea);

//        allyController.shieldArea =
//            Handles.ScaleValueHandle(allyController.shieldArea,
//                allyController.transform.position + allyController.transform.forward * allyController.shieldArea,
//                allyController.transform.rotation,
//                1,
//                Handles.ConeHandleCap,
//                1);
//    }
//}