#if UNITY_EDITOR
using Alchemy.Editor;
using Alchemy.Inspector;
using Event;
using Event.Signal;
using UI.Game;
using UniRx;
using UnityEditor;
using UnityEngine;

public class RayserDebugWindow : AlchemyEditorWindow
{
    [MenuItem("RAYSER/DebugWindow")]
    static void Open()
    {
        var window = GetWindow<RayserDebugWindow>();
        window.Show();
    }

    [HelpBox("RAYSERのデバッグ用ウィンドウです")]
    [Title("Command")]
    [Button]
    void TimeScaleを1に設定()
    {
        _timeScale = 1f;
    }

    [Button]
    void ScoreAdd()
    {
        MessageBroker.Default.Publish(new ScoreAccumulation { Score = 1000000 });
    }

    [Title("TimeScale")] [SerializeField] [OnValueChanged("OnTimeScaleChanged")] [Range(0f, 10f)]
    float _timeScale = 1f;


    void OnTimeScaleChanged(float value)
    {
        Time.timeScale = value;
    }

    [Title("Stage1Move")]
    [Button]
    [HorizontalGroup("Stage1")]
    void Stage1Boss()
    {
        MessageBroker.Default.Publish(new Stage1BossEncounter(TalkEnum.TalkStart));
    }

    [Title("Stage2Move")]
    [Button]
    [HorizontalGroup("Stage2")]
    void Stage2()
    {
        MessageBroker.Default.Publish(new Stage2IntervalStart(TalkEnum.TalkStart));
    }

    [Button]
    [HorizontalGroup("Stage2")]
    void Stage2Boss()
    {
        MessageBroker.Default.Publish(new Stage2BossEncounter(TalkEnum.TalkStart));
    }


    [Title("Stage3Move")]
    [Button]
    [HorizontalGroup("Stage3")]
    void Stage3()
    {
        MessageBroker.Default.Publish(new Stage3IntervalStart(TalkEnum.TalkStart));
    }

    [Title("GameStatus")]
    [Button]
    [HorizontalGroup("Game")]
    void GameClear()
    {
        MessageBroker.Default.Publish(new GameClear(TalkEnum.TalkStart));
    }

    [Button]
    [HorizontalGroup("Game")]
    void GameOver()
    {
        MessageBroker.Default.Publish(new Gameover());
    }
}
#endif
