using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

namespace VRM
{
    /// <summary>
    /// VRMの口アニメーション用
    /// </summary>
    public class MouthAnimation : MonoBehaviour
    {
        [SerializeField] float mouthAnimationTime = 0.1f;
        [SerializeField] float blinkInterval = 3.0f;
        [SerializeField] bool isTalking = false;
        [SerializeField] bool isTalkingEnabled = false;
        BlendShapePreset currentFace;
        VRMBlendShapeProxy proxy;

        void Start()
        {
            proxy = GetComponent<VRMBlendShapeProxy>();
            //デフォルトの表情をセット
            // currentFace = BlendShapePreset.Neutral;
            // proxy.AccumulateValue(currentFace, 1);
        }

        void Update()
        {
            StartCoroutine("TalkMouthAnimation");
            proxy.Apply();
        }

        public void MouthAnimationStart()
        {
            isTalkingEnabled = true;
        }

        public void MouthAnimationStop()
        {
            isTalkingEnabled = false;
        }

        IEnumerator TalkMouthAnimation()
        {
            if (isTalkingEnabled == false)
            {
                yield break;
            }

            if (isTalking == true){
                yield break;
            }

            isTalking = true;

            proxy.SetValue(BlendShapePreset.A, 1f);
            yield return new WaitForSeconds(mouthAnimationTime);
            proxy.SetValue(BlendShapePreset.A, 0f);
            yield return new WaitForSeconds(mouthAnimationTime);

            isTalking = false;
        }
    }
}
