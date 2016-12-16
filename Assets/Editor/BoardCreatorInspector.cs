using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorInspector : Editor {
	
	public BoardCreator current {
    get {
       return (BoardCreator)target;
    }
	}

	public override void OnInspectorGUI () {
    DrawDefaultInspector();
 
    if (GUILayout.Button("UP"))
      current.MoveMarker("UP");
    if (GUILayout.Button("DOWN"))
      current.MoveMarker("DOWN");
    if (GUILayout.Button("RIGHT"))
      current.MoveMarker("RIGHT");
    if (GUILayout.Button("LEFT"))
      current.MoveMarker("LEFT");

    if (GUILayout.Button("Clear"))
      current.Clear();
    if (GUILayout.Button("Grow"))
			current.Grow();
    if (GUILayout.Button("Shrink"))
			current.Shrink();
    if (GUILayout.Button("Grow Area"))
      current.GrowArea();
    if (GUILayout.Button("Shrink Area"))
      current.ShrinkArea();
    if (GUILayout.Button("Save"))
      current.Save();
    if (GUILayout.Button("Load"))
      current.Load();
 
    if (GUI.changed)
			current.UpdateMarker ();
	}

	void OnSceneGUI() {
    Event e = Event.current;

		switch (e.type)
    {
        case EventType.keyDown:
        {
            if (Event.current.keyCode == (KeyCode.W))
            {
						Debug.Log("SUCCESS");
                current.MoveMarker("UP");
            }
            break;
        }
    }

    /*if (e.type == EventType.keyDown) {
			Debug.Log(e + " is KeyDown");
			switch(Event.current.keyCode) {
				case KeyCode.UpArrow:			current.MoveMarker("UP"); break;
				case KeyCode.DownArrow:		current.MoveMarker("DOWN"); break;
				case KeyCode.RightArrow:	current.MoveMarker("RIGHT"); break;
				case KeyCode.LeftArrow:		current.MoveMarker("LEFT"); break;
			}          
    }*/
  }
}
