using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
 
[System.Serializable]
public class SpeakerData : ScriptableObject {
  public List<string> messages;
  public Sprite speaker;
	public int spriteOrientation;
  public TextAnchor anchor;

	public void Load (string line) {
		string[] elements = line.Split(';');
    name = elements[0];
    anchor = (TextAnchor) Enum.Parse(typeof(TextAnchor), elements[1]);
		spriteOrientation = Convert.ToInt32(elements[2]);
		speaker = Resources.Load<Sprite>("Conversations/Avatars/" + elements[3]);
		messages = new List<string>(elements[4].Split('\\'));
	}
}