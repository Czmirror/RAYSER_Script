using UnityEngine;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// サブウェポンの弾(未使用になるかもしれない)
    /// </summary>
    public class SubWeaponBullet : MonoBehaviour
    {
        // 弾丸の振る舞いやプロパティを定義
        public float speed = 10f;
        public int damage = 1;

        void Update()
        {
            // ここで弾丸の動きを更新
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
