using System.Collections;
using Damage;
using Target;
using UnityEngine;

namespace Turret
{
    public class Laser : MonoBehaviour, IDamageableToEnemy
    {
        /// <summary>
        /// ラインレンダラーのコンポーネント
        /// </summary>
        [SerializeField] private LineRenderer lineRenderer;

        /// <summary>
        /// マズルフラッシュのゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject muzzleFlash;

        /// <summary>
        /// ヒットエフェクトのゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject hitFlash;

        /// <summary>
        /// レーザー発射地点
        /// </summary>
        private Vector3 laserLaunchPoint;

        /// <summary>
        /// レーザー目標地点
        /// </summary>
        private Vector3 laserTargetPoint;

        /// <summary>
        /// 自機のTransform
        /// </summary>
        private Transform _playerTransform;

        /// <summary>
        /// 目標のゲームオブジェクト
        /// </summary>
        private GameObject targetGameObject;

        /// <summary>
        /// レーザー消失時間
        /// </summary>
        [SerializeField] private float laserDisappearanceTime = 1.0f;

        /// <summary>
        /// 自機のターゲッティング処理
        /// </summary>
        private PlayerTargeting _playerTargeting;

        /// <summary>
        /// レーザーの攻撃力
        /// </summary>
        [SerializeField] private float laserPower = 1f;

        /// <summary>
        /// レーザー当たり判定の半径
        /// </summary>
        [SerializeField] private float laserRadius = 1.5f;

        public void Initialize(Transform playerTransform, PlayerTargeting playerTargeting)
        {
            _playerTransform = playerTransform;
            _playerTargeting = playerTargeting;

            LaserShot();
        }

        public float AddDamage()
        {
            return laserPower;
        }

        /// <summary>
        /// レーザー発射処理
        /// </summary>
        private void LaserShot()
        {
            positionSetting();
            drewLaserLine();
            raySetting();

            StartCoroutine("FadeOut");
            Destroy(gameObject, laserDisappearanceTime);
        }

        /// <summary>
        /// レーザーの座標設定
        /// </summary>
        private void positionSetting()
        {
            laserLaunchPoint = _playerTransform.position;
            targetGameObject = _playerTargeting.CurrentTargetGameObject();
            laserTargetPoint = targetGameObject.transform.position;
        }

        /// <summary>
        /// レーザー描画処理
        /// </summary>
        private void drewLaserLine()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
            lineRenderer.SetVertexCount(2);

            // Draw Laser
            lineRenderer.SetPosition(0, laserLaunchPoint);
            lineRenderer.SetPosition(1, laserTargetPoint);

            // MuzzleFlash
            var _muzzleFlash = Instantiate(muzzleFlash);
            _muzzleFlash.transform.position = laserLaunchPoint;
            _muzzleFlash.transform.LookAt(targetGameObject.transform);
            // _muzzleFlash.GetComponent<MuzzleAndHitFlash>().Initialize(_playerTransform, targetGameObject.transform);

            // HitFlash
            var _hitFlash = Instantiate(hitFlash);
            _hitFlash.transform.position = laserTargetPoint;
            _hitFlash.transform.LookAt(_playerTransform.transform);
            // _hitFlash.GetComponent<MuzzleAndHitFlash>().Initialize(targetGameObject.transform, _playerTransform);
        }

        /// <summary>
        /// Ray設定
        /// </summary>
        private void raySetting()
        {
            // Ray Setting
            var heading = laserTargetPoint - laserLaunchPoint; // Rayの目標座標
            var distance = heading.magnitude; // Rayの飛ばせる距離
            var direction = heading / distance; // Rayのベクトル

            // 命中した敵機に対してイベントを発行
            var hitObjects = Physics.SphereCastAll(laserLaunchPoint, laserRadius, direction * distance);

            foreach (var hit in hitObjects)
            {
                if (hit.transform.TryGetComponent(out IHitByLaser enemy))
                {
                    enemy.GetHitLaser(this);
                }
            }
        }

        /// <summary>
        /// レーザーフェードアウト処理
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeOut()
        {
            var a = 1.0f;
            while (a > 0)
            {
                // TODO レーザーの再描画
                // positionSetting();
                // lineRenderer.SetPosition(0, laserLaunchPoint);
                // lineRenderer.SetPosition(1, laserTargetPoint);

                var color = new Vector4(1.0f, 1.0f, 1.0f, a);

                // SetColors(StartColot, EndColor)
                // 線の先頭と末尾で同じ色を使用しているので同じ値を指定している
                // 先端から徐々にフェードアウトしたい、みたいなケースは個別に指定が必要
                lineRenderer.SetColors(color, color);
                yield return new WaitForSeconds(0.02f);
                a -= 0.1f;
            }

            // α値が0になったら線の描画自体をやめる
            lineRenderer.enabled = false;
        }
    }
}
