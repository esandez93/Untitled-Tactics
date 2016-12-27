using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
 
public class ConversationsManager : AssetPostprocessor {
  static Dictionary<string, Func<string, int>> parsers;
	static int count = 1;
 
  static ConversationsManager () {
    parsers = new Dictionary<string, Func<string, int>>();

		string[] conversationFiles = Directory.GetFiles("Assets/Settings/Conversations");

		for (int i = 0; i < conversationFiles.Length; ++i) {
			if (!conversationFiles[i].Contains(".meta"))
				parsers.Add(Path.GetFileName(conversationFiles[i]), ParseConversations);
		}
  }
 
  static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
    for (int i = 0; i < importedAssets.Length; i++) {
      string fileName = Path.GetFileName( importedAssets[i] );
      if (parsers.ContainsKey(fileName))
        parsers[fileName](fileName);
    }

    AssetDatabase.SaveAssets ();
    AssetDatabase.Refresh();
		count = 1;
  }
 
  static int ParseConversations (string conversationFile) {
    string filePath = Application.dataPath + "/Settings/Conversations/" + conversationFile;

    if (!File.Exists(filePath)) {
      Debug.LogError("Missing Conversations Data: " + filePath);
      return 0;
    }

		ConversationData conversation = ScriptableObject.CreateInstance<ConversationData>();
		conversation.list = new List<SpeakerData>();
		string[] readText = File.ReadAllLines(filePath);

		filePath = "Assets/Resources/Conversations/";

		for (int i = 1; i < readText.Length; ++i) {
			SpeakerData speakerData = ScriptableObject.CreateInstance<SpeakerData>();
			speakerData.Load(readText[i]);

			conversation.list.Add(speakerData);
		}

		string fileName = string.Format("{0}{1}.asset", filePath, count);
		++count;
		AssetDatabase.CreateAsset(conversation, fileName);

		return 0;
  }
}