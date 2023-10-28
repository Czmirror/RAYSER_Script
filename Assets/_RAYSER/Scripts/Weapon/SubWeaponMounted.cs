using System.Collections;
using System.Linq;
using _RAYSER.Scripts.Item;
using MessagePipe;

namespace _RAYSER.Scripts.Weapon
{
    /// <summary>
    /// サブウェポン装備クラス
    /// </summary>
    public class SubWeaponMounted
    {
        /// <summary>
        /// 選択中のサブウェポンのインデックス
        /// </summary>
        private int _selectingSubWeaponIndex;

        /// <summary>
        /// 格納可能なサブウェポン
        /// </summary>
        private IItem[] _subWeapon = new IItem[5];

        private readonly IPublisher<CurrentSubWeaponIndex> _inputPublisher;

        public SubWeaponMounted(IPublisher<CurrentSubWeaponIndex> inputPublisher) {
            _inputPublisher = inputPublisher;
        }

        /// <summary>
        /// サブウェポンの格納
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void SetSubWeapon(int index, IItem item) {
            if (_subWeapon[index] == null) {
                _subWeapon[index] = item;
            }
        }

        /// <summary>
        /// サブウェポンの切り替え処理
        /// </summary>
        /// <param name="direction"></param>
        public void SwitchSubWeapon(SubweaponMoveDirection direction)
        {
            var move = direction == SubweaponMoveDirection.Left ? -1 : 1;
            var index = _selectingSubWeaponIndex + move;
            if (index < 0) index = _subWeapon.Length - 1;
            if (index > _subWeapon.Length - 1) index = 0;
            _selectingSubWeaponIndex = index;

            // メッセージを作成
            var inputParams = new CurrentSubWeaponIndex(_selectingSubWeaponIndex);

            // メッセージ送信
            _inputPublisher.Publish(inputParams);
        }

        /// <summary>
        /// 選択中のサブウェポンを取得する
        /// </summary>
        public IItem GetSelectingSubWeapon()
        {
            if (_subWeapon[_selectingSubWeaponIndex] == null) return null;

            return _subWeapon[_selectingSubWeaponIndex];
        }
    }
}
