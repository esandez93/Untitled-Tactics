﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
 
[RequireComponent(typeof(LayoutAnchor))]
public class Panel : MonoBehaviour {
	[SerializeField] List<Position> positionList;
	Dictionary<string, Position> positionMap;
	LayoutAnchor anchor;

	public Position CurrentPosition { get; private set; }
	public Tweener Transition { get; private set; }
	public bool InTransition { get { return Transition != null; }}
	public Position this[string name] {
		get {
			if (positionMap.ContainsKey(name))
				return positionMap[name];

			return null;
		}
	}
 
	void Awake () {
		anchor = GetComponent<LayoutAnchor>();
		positionMap = new Dictionary<string, Position>(positionList.Count);
		for (int i = positionList.Count - 1; i >= 0; --i) {
			AddPosition(positionList[i]);
		}
	}

	void Start () {
    if (CurrentPosition == null && positionList.Count > 0)
      SetPosition(positionList[0], false);
	}

	public void AddPosition (Position position) {
    positionMap[position.name] = position;
	}
 
	public void RemovePosition (Position position) {
    if (positionMap.ContainsKey(position.name))
      positionMap.Remove(position.name);
	}

	public Tweener SetPosition (string positionName, bool animated) {
    return SetPosition(this[positionName], animated);
	}
 
	public Tweener SetPosition (Position position, bool animated) {
    CurrentPosition = position;
    if (CurrentPosition == null)
      return null;
 
    if (InTransition)
      Transition.easingControl.Stop();
 
    if (animated) {
      Transition = anchor.MoveToAnchorPosition(position.myAnchor, position.parentAnchor, position.offset);
      return Transition;
    }
    else {
      anchor.SnapToAnchorPosition(position.myAnchor, position.parentAnchor, position.offset);
      return null;
    }
	}

	[Serializable]
	public class Position {
		public string name;
		public TextAnchor myAnchor;
		public TextAnchor parentAnchor;
		public Vector2 offset;
     
		public Position (string name) {
			this.name = name;
		}
     
		public Position (string name, TextAnchor myAnchor, TextAnchor parentAnchor) : this(name) {
			this.myAnchor = myAnchor;
			this.parentAnchor = parentAnchor;
		}
     
		public Position (string name, TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset) : this(name, myAnchor, parentAnchor) {
			this.offset = offset;
		}
	}
}