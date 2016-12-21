using UnityEngine;
using System.Collections;
 
public abstract class BattleState : State {
  protected BattleController owner;
  public CameraRig cameraRig { get { return owner.cameraRig; }}
  public Board board { get { return owner.board; }}
  public LevelData levelData { get { return owner.levelData; }}
  public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; }}
  public Point position { get { return owner.position; } set { owner.position = value; }}
 
  protected virtual void Awake () {
    owner = GetComponent<BattleController>();
  }
 
  protected override void AddListeners () {
    InputController.moveEvent += OnMove;
    InputController.fireEvent += OnFire;
  }
     
  protected override void RemoveListeners () {
    InputController.moveEvent -= OnMove;
    InputController.fireEvent -= OnFire;
  }
 
  protected virtual void OnMove (object sender, InfoEventArgs<Point> e) {
         
  }
     
  protected virtual void OnFire (object sender, InfoEventArgs<int> e) {
         
  }
 
  protected virtual void SelectTile (Point pos) {
    if (position == pos || !board.tiles.ContainsKey(pos))
      return;
 
    position = pos;
    tileSelectionIndicator.localPosition = board.tiles[pos].center;
  }
}