using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Turret;
using UnityEngine.Serialization;

[AddComponentMenu("")]
public class Stage2BossMoveForwardAndBackWard : StateBehaviour {
    public StateLink selfLink;

    [SerializeField] private Vector3[] vertexShiftPositions;
    private int vertexShiftIndex = 0;
    [SerializeField] private EnemyTurret bombTurret;

    // <summary>
    /// ボスの移動スピード
    /// </summary>
    private float moveTime = 3f;

	// Use this for initialization
	void Start () {

	}

	// Use this for awake state
	public override void OnStateAwake() {
        bombTurret.gameObject.SetActive(true);
        bombTurret.StartShootingAsync().Forget();
	}

	// Use this for enter state
	public override void OnStateBegin() {
        MoveVertexShift();
	}

    private async UniTask MoveVertexShift()
    {
        var position = vertexShiftPositions[vertexShiftIndex];
        Move(position);

        int time = (int)moveTime * 1000;
        await UniTask.Delay(time);

        vertexShiftIndex++;
        if (vertexShiftIndex >= vertexShiftPositions.Length)
        {
            vertexShiftIndex = 0;
        }

        Transition(selfLink);
    }

    private void Move(Vector3 position)
    {
        transform.DOMove(position, moveTime)
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
