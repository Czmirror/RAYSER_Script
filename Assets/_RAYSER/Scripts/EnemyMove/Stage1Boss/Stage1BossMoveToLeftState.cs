using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using DG.Tweening;

[AddComponentMenu("")]
public class Stage1BossMoveToLeftState : StateBehaviour {

    // <summary>
    /// ボスの移動地点
    /// </summary>
    private Vector3 stage1BossMovePoint = new Vector3(-20, 300, -55);

    // <summary>
    /// ボスの移動スピード
    /// </summary>
    private float moveTime = 3f;

    public StateLink MoveCompleteLink;

	// Use this for initialization
	void Start () {

	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() {
        transform.DOMove(stage1BossMovePoint, moveTime)
            .OnComplete(() =>
            {
                Transition(MoveCompleteLink);
            })
            .Pause()
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Restart();
	}

	// Use this for exit state
	public override void OnStateEnd() {
        // 元のステートに戻る
        // var state = this.prevTransitionState;
        // Transition(state);
	}

	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() {
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
