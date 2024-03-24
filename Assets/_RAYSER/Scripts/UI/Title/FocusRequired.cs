using System.Collections;
using System.Linq;
using _RAYSER.Scripts.UI;
using UniRx;
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
        [SerializeField] private GameObject PreviousSelection = null;

        /// <summary>
        /// 選択対象のオブジェクト一覧。
        /// </summary>
        [SerializeField] private GameObject[] _selectables;

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

            // messagebrokerでselectorを受け取る _selectableGameObjectに選択されたオブジェクトを入れる（UIの選択がnullになってしまうことがあるため、それを回避するための処理）
            MessageBroker.Default.Receive<UISelectorSignal>()
                .Subscribe(x =>
                {
                    // SelectionHookerがアタッチされていない場合はアタッチする
                    if (x.forcusUIGameObject.GetComponent<SelectionHooker>() == null)
                    {
                        var hooker = x.forcusUIGameObject.AddComponent<SelectionHooker>();
                        hooker.Restrictor = this;

                        // _selectablesに追加
                        _selectables = _selectables.Append(x.forcusUIGameObject).ToArray();
                    }
                })
                .AddTo(this);

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
                // Debug.Log(EventSystem.current.currentSelectedGameObject);

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
