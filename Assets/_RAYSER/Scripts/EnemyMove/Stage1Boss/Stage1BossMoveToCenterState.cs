using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Turret;
using Arbor.Threading.Tasks;

[AddComponentMenu("")]
public class Stage1BossMoveToCenterState : StateBehaviour {

    private Vector3 stage1BossMovePoint = new Vector3(-20, 300, 0);

    private float moveTime = 0.5f;

    public StateLink MoveCompleteLink;

    [SerializeField] private EnemyTurret _beamTurret;
    [SerializeField] private EnemyTurret _laserTurret;

	// Use this for initialization
	void Start () {

	}

	// Use this for awake state
	public override void OnStateAwake() {
	}

	// Use this for enter state
	public override void OnStateBegin()
    {
        Move();
	}

    private async UniTask Move()
    {
        transform.DOMove(stage1BossMovePoint, moveTime)
            .OnComplete(() =>
            {
                ShootBeam();
            })
            .Pause()
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Restart();
    }

    private async UniTask ShootBeam()
    {
        _beamTurret.gameObject.SetActive(false);
        _beamTurret.StopShooting();

        _laserTurret.gameObject.SetActive(true);
        _laserTurret.StartShootingAsync().Forget();
        await UniTask.Delay(3000);

        _laserTurret.StopShooting();
        _laserTurret.gameObject.SetActive(false);

        _beamTurret.gameObject.SetActive(true);
        _beamTurret.StartShootingAsync().Forget();

        Transition(MoveCompleteLink);
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
