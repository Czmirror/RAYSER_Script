using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using DG.Tweening;

[AddComponentMenu("")]
public class Stage1BossMoveTackle : StateBehaviour {
    public StateLink MoveCompleteLink;

    private Vector3 movePoint;

    private float moveTime = 1f;

    private Vector3 initialPosition;

    /// <summary>
    /// 体当たりのz軸の移動値
    /// </summary>
    private float moveX = 50f;

	// Use this for initialization
	void Start () {

	}

	// Use this for awake state
	public override void OnStateAwake() {

	}

	// Use this for enter state
	public override void OnStateBegin() {
        // 元座標を取得
        Vector3 initialPosition = transform.position;

        MoveTackle();
	}

    private void MoveTackle()
    {
        movePoint = new Vector3(transform.position.x, transform.position.y,  moveX);

        transform.DOLocalMoveX(moveX, moveTime)
            .OnComplete(() =>
            {
                MoveBack();
            })
            .Pause()
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Restart();
    }

    // 元の位置に戻る
    private void MoveBack()
    {
        var backX = moveX * -1;
        transform.DOLocalMoveX(backX , moveTime)
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
	}

	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() {
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
