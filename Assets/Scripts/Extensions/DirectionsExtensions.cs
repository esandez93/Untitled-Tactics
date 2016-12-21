using UnityEngine;
using System.Collections;
 
public static class DirectionsExtensions {
  public static Directions GetDirection (this Tile tile1, Tile tile2) {
    if (tile1.position.y < tile2.position.y)
      return Directions.North;
    else if (tile1.position.x < tile2.position.x)
      return Directions.East;
    else if (tile1.position.y > tile2.position.y)
      return Directions.South;
		else
			return Directions.West;
  }
 
  public static Vector3 ToEuler (this Directions directions) {
    return new Vector3(0, (int)directions * 90, 0);
  }
}