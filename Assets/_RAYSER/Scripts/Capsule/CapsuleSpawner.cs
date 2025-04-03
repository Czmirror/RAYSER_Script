using System;
using System.Collections.Generic;
using Event.Signal;
using Shield;
using Status;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Capsule
{
    /// <summary>
    /// カプセル生成処理
    /// </summary>
    public class CapsuleSpawner : MonoBehaviour
    {
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// プレイヤーシールド
        /// </summary>
        /// <returns></returns>
        [SerializeField]
        private PlayerShield _playerShield;

        /// <summary>
        /// 回復カプセル
        /// </summary>
        [SerializeField] private CapsuleShieldRecover _capsuleShieldRecover;

        /// <summary>
        /// ボーナスカプセル
        /// </summary>
        [SerializeField] private CapsuleBonus _capsuleBonus;

        /// <summary>
        /// レーザーカプセル
        /// </summary>
        [SerializeField] private CapsuleLaserPowerUp _capsuleLaserPowerUp;

        /// <summary>
        /// スピードカプセル
        /// </summary>
        [SerializeField] private CapsuleSpeedUp _capsuleSpeedUp;

        /// <summary>
        /// プレイヤーレーザーレベル
        /// </summary>
        [SerializeField] private Turret.PlayerLaserLevel _playerLaserLevel;

        /// <summary>
        /// プレイヤースピードレベル
        /// </summary>
        [SerializeField] private PlayerMove.PlayerMoveCore _playerMoveCore;

        private void Start()
        {
            MessageBroker.Default.Receive<CapsuleSpawn>().Subscribe(x => CapsuleSpawn(x.CapsuleSpawnPoint)).AddTo(this);
        }

        /// <summary>
        /// カプセル生成処理
        /// </summary>
        /// <param name="capsuleSpawnTransform">カプセル生成置</param>
        private void CapsuleSpawn(Transform capsuleSpawnTransform)
        {
            // リトライ時にカプセルが表示されたり、ステージ間でカプセルが表示されない処理
            if (_gameStatus.CurrentGameState == GameState.Gameover ||
                _gameStatus.CurrentGameState == GameState.GameClear ||
                _gameStatus.CurrentGameState == GameState.Stage2Interval ||
                _gameStatus.CurrentGameState == GameState.Stage3Interval)
            {
                return;
            }

            // カプセルを取得
            ICapsuleinfo capsuleInfo = GetCapsule();

            // 選択されたカプセルを生成
            var capsule = Instantiate(((MonoBehaviour)capsuleInfo).gameObject, capsuleSpawnTransform.position, Quaternion.identity);

            // 生成されたカプセルにゲームステータスを設定
            if (capsule.gameObject.TryGetComponent(out CapsuleMove capsuleMove))
            {
                capsuleMove.Initialize(_gameStatus.CurrentGameState);
            }
        }

        /// <summary>
        /// カプセルを取得する
        /// </summary>
        /// <returns>ICapsuleinfo</returns>
        private ICapsuleinfo GetCapsule()
        {
            ICapsuleinfo[] capsules = { _capsuleShieldRecover, _capsuleBonus, _capsuleLaserPowerUp, _capsuleSpeedUp };

            // シールドが危険状態の場合は回復カプセルを優先的に返す
            if (_playerShield.IsShieldDanger())
            {
                return GetRecoveryCapsule();
            }

            // 条件に基づいてカプセルを除外
            capsules = ExcludeCapsulesBasedOnConditions(capsules);

            // 残りのカプセルからランダムに選択
            return GetRandomCapsule(capsules);

        }

        /// <summary>
        /// capsuleから引数の値を除外して返却
        /// </summary>
        /// <param name="capsules"></param>
        /// <param name="capsule"></param>
        /// <returns></returns>
        private ICapsuleinfo[] ExcludeCapsulesBasedOnConditions(ICapsuleinfo[] capsules)
        {
            // 除外するカプセルの条件をリストに追加
            var excludedCapsuleTypes = new List<Type>();

            // シールドが満タンの場合、回復カプセルを除外
            if (_playerShield.IsShieldFull())
            {
                excludedCapsuleTypes.Add(typeof(CapsuleShieldRecover));
            }

            // レーザーレベルが最大の場合、レーザーカプセルを除外
            if (_playerLaserLevel.IsMaxLaserLevel())
            {
                excludedCapsuleTypes.Add(typeof(CapsuleLaserPowerUp));
            }

            // スピードレベルが最大の場合、スピードカプセルを除外
            if (_playerMoveCore.PlayerSpeedLevel.IsMaxSpeedLevel())
            {
                excludedCapsuleTypes.Add(typeof(CapsuleSpeedUp));
            }

            // フィルタリング
            return Array.FindAll(capsules, capsule => !excludedCapsuleTypes.Contains(capsule.GetType()));
        }

        /// <summary>
        /// ランダムにカプセルを選択する
        /// </summary>
        /// <returns>ICapsuleinfo</returns>
        private ICapsuleinfo GetRandomCapsule(ICapsuleinfo[] capsules)
        {
            return capsules[Random.Range(0, capsules.Length)];
        }

        /// <summary>
        /// 回復カプセルを選択する
        /// </summary>
        /// <returns>ICapsuleinfo</returns>
        private ICapsuleinfo GetRecoveryCapsule()
        {
            return _capsuleShieldRecover;
        }
    }
}
