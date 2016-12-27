using UnityEngine;
using System.Collections;
 
public class MoveSequenceState : BattleState {
  public override void Enter () {
    base.Enter ();
    StartCoroutine("Sequence");
  }
     
  IEnumerator Sequence () {
    Movement movement = owner.currentUnit.GetComponent<Movement>();
    yield return StartCoroutine(movement.Traverse(owner.currentTile));
    owner.ChangeState<SelectUnitState>();
  }
}