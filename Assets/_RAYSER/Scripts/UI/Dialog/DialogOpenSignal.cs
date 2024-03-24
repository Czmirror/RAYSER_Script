using _RAYSER.Scripts.Item;

namespace _RAYSER.Scripts.UI.Dialog
{
    /// <summary>
    /// ダイアログオープンシグナルクラス
    /// </summary>
    public struct DialogOpenSignal
    {
        public IItem Item { get; private set; }

        public DialogOpenSignal(IItem item)
        {
            Item = item;
        }
    }
}
