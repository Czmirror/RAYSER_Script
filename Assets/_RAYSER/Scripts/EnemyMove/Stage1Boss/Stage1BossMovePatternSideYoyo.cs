using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Shield;
using Turret;

[AddComponentMenu("")]
public class Stage1BossMovePatternSideYoyo : StateBehaviour {

    public StateLink MoveToRightLink;
    public StateLink MoveToLeftLink;
    public StateLink MoveToCenterLink;
    public StateLink MoveToNextPatternLink;
    [SerializeField] private EnemyShield _enemyShield;

    [SerializeField] private EnemyTurret _beamTurret;
    [SerializeField] private EnemyTurret _laserTurret;

    /// <summary>
    /// 移動回数
    /// </summary>
    private int moveCount = 0;

    /// <summary>
    /// パターン変更移動回数
    /// </summary>
    private int movePatternChangeCount = 4;

    /// <summary>
    /// 次のステートに遷移するEnemyShieldの値
    /// </summary>
    private float _nextPatternShieldValue = 30f;

	// Use this for initialization
	void Start () {
        _laserTurret.gameObject.SetActive(false);
	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin() {

        // ボスのシールドが次のパターンに移行する値を下回ったらパターン変更
        if (_enemyShield.CurrentShield < _nextPatternShieldValue)
        {
            Transition(MoveToNextPatternLink);
            return;
        }

        moveCount++;

        // 移動回数がパターン変更回数に達したらパターン変更
        if (moveCount == movePatternChangeCount)
        {
            moveCount = 0;
            Transition(MoveToCenterLink);
            return;
        }

        // 移動回数が奇数の時は右に移動
        if (moveCount % 2 == 1)
        {
            Transition(MoveToRightLink);
        }

        // 移動回数が偶数の時は左に移動
        else
        {
            Transition(MoveToLeftLink);
        }

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
