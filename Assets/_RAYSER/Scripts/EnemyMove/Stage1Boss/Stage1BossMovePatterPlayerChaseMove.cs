using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using DG.Tweening;
using PlayerMove;
using Shield;

[AddComponentMenu("")]
public class Stage1BossMovePatterPlayerChaseMove : StateBehaviour {

    /// <summary>
    /// 追尾するプレイヤー
    /// </summary>
    [SerializeField] private PlayerMoveCore player;

    public StateLink MoveCompleteLink;

    private Vector3 movePoint;

    // <summary>
    /// ボスの移動スピード
    /// </summary>
    private float moveTime = 1f;
	void Start () {

	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() {
        // 自機の座標を取得
        Vector3 playerPosition = player.transform.position;

        movePoint = new Vector3(transform.position.x, transform.position.y, playerPosition.z);

        transform.DOMoveZ(playerPosition.z, moveTime)
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
