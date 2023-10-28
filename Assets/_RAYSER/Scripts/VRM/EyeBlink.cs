using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

namespace VRM
{
    /// <summary>
    /// VRMの瞬き用
    /// </summary>
    public class EyeBlink : MonoBehaviour
    {
        [SerializeField]
        float blinkTime = 0.1f;
        [SerializeField]
        float blinkInterval = 3.0f;
        bool blinkEnabled = true;
        bool blinking = false;
        BlendShapePreset currentFace;
        VRMBlendShapeProxy proxy;
        void Start()
        {
            proxy = GetComponent<VRMBlendShapeProxy>();
            //デフォルトの表情をセット
            currentFace = BlendShapePreset.Neutral;
            proxy.AccumulateValue(currentFace, 1);
        }
        void Update()
        {
            StartCoroutine("AutoBlink");  //文字列で指定する必要あり

            proxy.Apply();
        }
        public void ChangeFace(BlendShapePreset preset = BlendShapePreset.Neutral, bool blink = false)
        {
            blinkEnabled = blink;
            if (!blink)
            {
                StopCoroutine("AutoBlink"); //文字列で指定しないと止まらないので注意
                blinking = false;
                proxy.AccumulateValue(BlendShapePreset.Blink, 0);
            }
            proxy.AccumulateValue(currentFace, 0);  //今の表情を無効化する
            proxy.AccumulateValue(preset, 1);    //新しい表情をセットする
            currentFace = preset;
        }
        IEnumerator AutoBlink()
        {
            if (!blinkEnabled | blinking)
            {
                yield break;
            }
            blinking = true;
            proxy.AccumulateValue(BlendShapePreset.Blink, 0);
            yield return new WaitForSeconds(blinkInterval);
            proxy.AccumulateValue(BlendShapePreset.Blink, 1);
            yield return new WaitForSeconds(blinkTime);
            blinking = false;
        }
    }
}
