using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerCharacterController))]
[CanEditMultipleObjects]
public class PlayerCharacterControllerEditor : Editor
{
    
    //private PlayerCharacterController character { get { return (target as PlayerCharacterController); } }

    //SerializedProperty movementStyle;

    //int selected;
    //static string[] options = new string[] { "Square by Square", "Flight" };

    //MovementStyle.Style style;

    //private void OnEnable()
    //{
    //    //movementStyle = serializedObject.FindProperty("MovementStyle");
    //}

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //    serializedObject.Update();

    //    GUILayout.Label("MovementStyle");
    //    selected = GUILayout.SelectionGrid(selected, options, 2, EditorStyles.radioButton);
    //    character.movementStyle.style = MovementStyle.FromInt(selected);
    //    Debug.Log(character.movementStyle.style);

    //    //style = (MovementStyle.Style)EditorGUILayout.EnumFlagsField(style);

    //    EditorUtility.SetDirty(target);
    //    AssetDatabase.SaveAssets();
    //}
}