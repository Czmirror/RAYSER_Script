using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using DG.Tweening;
using Status;
using UniRx;

[AddComponentMenu("")]
public class Stage2BossMoveInitial : StateBehaviour {

    /// <summary>
    /// ゲームステータス
    /// </summary>
    [SerializeField] private GameStatus _gameStatus;

    /// <summary>
    /// ボスの初期地点
    /// </summary>
    private Vector3 initialPositionGoal = new Vector3(-830, 0, 0);

    // <summary>
    /// ボスの初期地点移動スピード
    /// </summary>
    private float moveInitialTime = 3f;

    public StateLink MoveCompleteLink;

	// Use this for initialization
	void Start () {

	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() {
        _gameStatus.CurrentGameStateReactiveProperty
            .Where(x =>
                x == GameState.Stage2Boss
            )
            .Subscribe(_ => InitialMove())
            .AddTo(this);
	}

    private void InitialMove()
    {
        transform.DOMove(initialPositionGoal, moveInitialTime)
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
