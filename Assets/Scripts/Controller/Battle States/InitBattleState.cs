using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class InitBattleState : BattleState {
  public override void Enter () {
    base.Enter ();
    StartCoroutine(Init());
  }
 
  IEnumerator Init () {
    board.Load( levelData );
    Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
    SelectTile(p);
    yield return null;
    //owner.ChangeState<MoveTargetState>();
		owner.ChangeState<CutSceneState>();
  }
	
	void SpawnTestUnits () {
		System.Type[] components = new System.Type[] {
			typeof(WalkMovement),
			typeof(FlyMovement),
			typeof(TeleportMovement)
		};

		for (int i = 0; i < 3; ++i) {
      GameObject instance = Instantiate(owner.heroPrefab) as GameObject;
 
      Point point = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
 
      Unit unit = instance.GetComponent<Unit>();
      unit.Place(board.GetTile(point));
      unit.Match();
 
      Movement movement = instance.AddComponent(components[i]) as Movement;
      movement.range = 5;
      movement.jumpHeight = 1;
    }
	}
}