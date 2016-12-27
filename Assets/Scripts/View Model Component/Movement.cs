using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public abstract class Movement : MonoBehaviour {
	public int range;
	public int jumpHeight;
	protected Unit unit;
	protected Transform jumper;

	protected virtual void Awake () {
    unit = GetComponent<Unit>();
    jumper = transform.FindChild("Jumper");
	}

	public virtual List<Tile> GetTilesInRange (Board board) {
    List<Tile> retValue = board.Search( unit.tile, ExpandSearch );
    Filter(retValue);
    return retValue;
	}

	protected virtual bool ExpandSearch (Tile from, Tile to) {
    return (from.distance + 1) <= range;
	}

	protected virtual void Filter (List<Tile> tiles) {
    for (int i = tiles.Count - 1; i >= 0; --i) {
			if (tiles[i].content != null)
				tiles.RemoveAt(i);
		}
	}

	public abstract IEnumerator Traverse (Tile tile);

	protected virtual IEnumerator Turn (Directions directions) {
    TransformLocalEulerTweener tweener = (TransformLocalEulerTweener)transform.RotateToLocal(directions.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);
     
    // When rotating between North and West, we must make an exception so it looks like the unit
    // rotates the most efficient way (since 0 and 360 are treated the same)
    if (Mathf.Approximately(tweener.startValue.y, 0f) && Mathf.Approximately(tweener.endValue.y, 270f))
      tweener.startValue = new Vector3(tweener.startValue.x, 360f, tweener.startValue.z);
    else if (Mathf.Approximately(tweener.startValue.y, 270) && Mathf.Approximately(tweener.endValue.y, 0))
      tweener.endValue = new Vector3(tweener.startValue.x, 360f, tweener.startValue.z);
 
    unit.directions = directions;
     
    while (tweener != null)
      yield return null;
	}
}