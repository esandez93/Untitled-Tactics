using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
  public const float STEP_HEIGHT = 0.25f;

  public Point position;
  public int height;
  public Vector3 center {
    get {
      return new Vector3(position.x, height * STEP_HEIGHT, position.y);
    }
  }

  void Match () {
    transform.localPosition = new Vector3(position.x, height * STEP_HEIGHT / 2f, position.y);
    transform.localScale = new Vector3(1, height * STEP_HEIGHT, 1);
  }

  public void Grow () {
    height++;
    Match();
  }

  public void Shrink () {
    height--;
    Match();
  }

  public void Load ( Point p, int h ) {
    position = p;
    height = h;
    Match();
  }

  public void Load ( Vector3 vector ) {
    Load(new Point((int) vector.x, (int) vector.z), (int) vector.y);
  }
}