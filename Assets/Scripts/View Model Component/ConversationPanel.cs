using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class ConversationPanel : MonoBehaviour {
  public Text message;
  public Image speaker;
  public GameObject arrow;
  public Panel panel;
 
  void Start () {
    Vector3 position = arrow.transform.localPosition;
    arrow.transform.localPosition = new Vector3(position.x, position.y + 5, position.z);

    Tweener tweener = arrow.transform.MoveToLocal(new Vector3(position.x, position.y - 5, position.z), 0.5f, EasingEquations.EaseInQuad);
    tweener.easingControl.loopType = EasingControl.LoopType.PingPong;
    tweener.easingControl.loopCount = -1;
  }
 
  public IEnumerator Display (SpeakerData data) {
    speaker.sprite = data.speaker;
    speaker.SetNativeSize();
 
		// Implement here sprite flip	

    for (int i = 0; i < data.messages.Count; ++i) {
      message.text = data.messages[i];
      arrow.SetActive( i + 1 < data.messages.Count );
      yield return null;
    }
  }
}