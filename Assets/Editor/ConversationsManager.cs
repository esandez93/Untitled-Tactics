using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
 
public class ConversationsManager : AssetPostprocessor {
  static Dictionary<string, Action> parsers; 
 
  static ConversationsManager () {
    parsers = new Dictionary<string, Action>();
    parsers.Add("Conversations.csv", ParseConversations);
  }
 
  static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
    for (int i = 0; i < importedAssets.Length; i++) {
      string fileName = Path.GetFileName( importedAssets[i] );
      if (parsers.ContainsKey(fileName))
        parsers[fileName]();
    }

    AssetDatabase.SaveAssets ();
    AssetDatabase.Refresh();
  }
 
  static void ParseConversations () {
    string filePath = Application.dataPath + "/Settings/Conversations.csv";
    if (!File.Exists(filePath)) {
      Debug.LogError("Missing Conversations Data: " + filePath);
      return;
    }
 
    string[] readText = File.ReadAllLines("Assets/Settings/Conversations.csv");
    filePath = "Assets/Resources/Conversations/";
    for (int i = 1; i < readText.Length; ++i) {
      SpeakerData speakerData = ScriptableObject.CreateInstance<SpeakerData>();
      speakerData.Load(readText[i]);
      string fileName = string.Format("{0}{1}.asset", filePath, i + "_" + speakerData.name.ToLower());
      AssetDatabase.CreateAsset(speakerData, fileName);
    }

		Debug.Log("Conversations loaded successfully.");
  }
}