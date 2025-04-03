using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _RAYSER.Scripts.Event.Signal;
using Arbor;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shield;
using Turret;
using UniRx;
using UnityEngine.Serialization;

[AddComponentMenu("")]
public class Stage2BossMoveVertexShift : StateBehaviour {

    public StateLink MoveToNextPatternLink;
    public StateLink selfLink;

    [SerializeField] private Vector3[] vertexShiftPositions;
    private int vertexShiftIndex = 0;
    [SerializeField] private EnemyShield enemyShield;
    [SerializeField] private EnemyTurret beamTurret;
    [SerializeField] private EnemyTurret bombTurret;

    // <summary>
    /// ボスの移動スピード
    /// </summary>
    private float moveTime = 3f;

    /// <summary>
    /// 次のステートに遷移するEnemyShieldの値
    /// </summary>
    private float _nextPatternShieldValue = 30f;


	// Use this for initialization
	void Start () {

	}

	// Use this for awake state
	public override void OnStateAwake() {
        bombTurret.gameObject.SetActive(false);
        bombTurret.StopShooting();

        beamTurret.gameObject.SetActive(false);
        beamTurret.StopShooting();

	}

	// Use this for enter state
	public override void OnStateBegin() {

        MoveVertexShift();
	}

    private async UniTask MoveVertexShift()
    {
        Debug.Log("Stage2Moss Move Vertex Shift");
        var position = vertexShiftPositions[vertexShiftIndex];
        Move(position);
        int time = (int)moveTime * 1000;
        await UniTask.Delay(time);
        beamTurret.gameObject.SetActive(true);
        beamTurret.StartShootingAsync().Forget();
        await UniTask.Delay(time);
        beamTurret.gameObject.SetActive(false);
        beamTurret.StopShooting();

        // ボスのシールドが次のパターンに移行する値を下回ったらパターン変更
        if (enemyShield.CurrentShield < _nextPatternShieldValue)
        {
            Debug.Log("Stage2Moss Move To Next Pattern");
            beamTurret.StopShooting();
            beamTurret.gameObject.SetActive(false);
            Transition(MoveToNextPatternLink);
            return;
        }
        else
        {
            // それ以外は次の位置に移動
            vertexShiftIndex++;
            if (vertexShiftIndex >= vertexShiftPositions.Length)
            {
                vertexShiftIndex = 0;
            }

            Transition(selfLink);
        }

    }

    private void Move(Vector3 position)
    {
        transform.DOMove(position, moveTime)
            .Pause()
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Restart();
    }

    private async UniTask ShootBeam()
    {
        Debug.Log("Stage2Moss Shoot Beam");
        beamTurret.gameObject.SetActive(true);
        beamTurret.StartShootingAsync().Forget();
        await UniTask.Delay(3000);
        beamTurret.gameObject.SetActive(false);
        beamTurret.StopShooting();
    }

    // Use this for exit state
	public override void OnStateEnd() {
	}

	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() {
        // ボスのシールドが次のパターンに移行する値を下回ったらパターン変更
        if (enemyShield.CurrentShield < _nextPatternShieldValue)
        {
            Debug.Log("Stage2Moss Move To Next Pattern");
            beamTurret.StopShooting();
            beamTurret.gameObject.SetActive(false);
            MessageBroker.Default.Publish(new Stage2BossPatternChange());
            Transition(MoveToNextPatternLink);
            return;
        }
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}
