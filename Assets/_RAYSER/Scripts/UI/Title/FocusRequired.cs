using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Title
{
    public class FocusRequired : MonoBehaviour
    {
        /// <summary>
        /// <see cref="Selectable"/> をフックするクラスです。
        /// </summary>
        private class SelectionHooker : MonoBehaviour, IDeselectHandler
        {
            /// <summary>親コンポーネント。</summary>
            public FocusRequired Restrictor;

            /// <summary>
            /// 選択解除時にそれまで選択されていたオブジェクトを覚えておく。
            /// </summary>
            /// <param name="eventData"></param>
            public void OnDeselect(BaseEventData eventData)
            {
                Restrictor.PreviousSelection = eventData.selectedObject;
            }
        }

        /// <summary>選択させないオブジェクト一覧。</summary>
        [SerializeField] private GameObject[] NotSelectables;

        /// <summary>直前まで選択されていたオブジェクト。</summary>
        private GameObject PreviousSelection = null;

        /// <summary>
        /// 選択対象のオブジェクト一覧。
        /// </summary>
        private GameObject[] _selectables;

        private void Awake()
        {
            // すべての Selectable を取得する
            var selectableList = (FindObjectsOfType(typeof(Selectable)) as Selectable[]).ToList();

            // 選択除外がある場合は外す
            if (NotSelectables != null)
            {
                foreach (var item in NotSelectables)
                {
                    var sel = item?.GetComponent<Selectable>();
                    if (sel != null) selectableList.Remove(sel);
                }
            }

            _selectables = selectableList.Select(x => x.gameObject).ToArray();

            // フォーカス許可オブジェクトに SelectionHooker をアタッチ
            foreach (var selectable in this._selectables)
            {
                var hooker = selectable.AddComponent<SelectionHooker>();
                hooker.Restrictor = this;
            }

            // フォーカス制御用コルーチンをスタート
            StartCoroutine(RestrictSelection());
        }

        /// <summary>
        /// フォーカス制御処理。
        /// </summary>
        /// <returns></returns>
        private IEnumerator RestrictSelection()
        {
            while (true)
            {
                // 別なオブジェクトを選択するまで待機
                yield return new WaitUntil(
                    () => (EventSystem.current != null) &&
                          (EventSystem.current.currentSelectedGameObject != PreviousSelection));

                // まだオブジェクトを未選択、または許可リストを選択しているなら何もしない
                if ((PreviousSelection == null) || _selectables.Contains(EventSystem.current.currentSelectedGameObject))
                {
                    continue;
                }

                // 選択しているものがなくなった、または許可していない Selectable を選択した場合は前の選択に戻す
                EventSystem.current.SetSelectedGameObject(PreviousSelection);
            }
        }
    }
}
