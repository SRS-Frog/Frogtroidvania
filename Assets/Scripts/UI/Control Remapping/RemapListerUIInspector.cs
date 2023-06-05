using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RemapListerUI))] 
public class RemapListerUIInspector : Editor
{ /*
    public override void OnInspectorGUI()
    {
        RemapListerUI scriptReference = (RemapListerUI)target;    //The target script
        if (GUILayout.Button ("Generate Binding Objects"))    // If the button is clicked
        {
            scriptReference.Generate();    //Execute the function in the target script
        }
        if (GUILayout.Button ("Clear Binding Objects"))    // If the button is clicked
        {
            scriptReference.Clear();    //Execute the function in the target script
        }
        
        DrawDefaultInspector ();    //This goes first
    }
    */
}