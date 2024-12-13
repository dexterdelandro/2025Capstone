using MalbersAnimations.Scriptables;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
    /// <summary> Based on 3DKit Controller from Unity </summary>
    [AddComponentMenu("Malbers/Utilities/Transform/Simple Scaler")]
    [SelectionBase]
    public class MSimpleScaler : MSimpleTransformer
    {
        [ContextMenuItem("Invert", nameof(InvertStartEnd))]
        public Vector3Reference startScale = new(Vector3.one);
        [ContextMenuItem("Invert", nameof(InvertStartEnd))]
        public Vector3Reference endScale = new(new Vector3(1.5f, 1.5f, 1.5f));

        private Vector3 difference;

        private void Awake()
        {
            Inverted = false;
            difference = endScale.Value - startScale.Value;
        }

        public override void Evaluate(float position)
        {
            Object.localScale = Vector3.LerpUnclamped(startScale, endScale, m_Curve.Evaluate(position));
        }



        /// <summary> When using Additive the rotation will continue from the last position  </summary>
        protected override void Pre_End()
        {
            if (loopType == LoopType.Once && endType == EndType.Additive)
            {
                startScale.Value = endScale.Value; //use the end value as start value
                endScale.Value += difference;
            }
        }

        protected override void Pos_End()
        {
            if (loopType == LoopType.Once && endType == EndType.Invert)
                InvertStartEnd();
        }


        public void Invert_Value()
        {
            // if (!enabled) return; //Do not invert while disabled

            if (Playing) { Debug.Log("Cannot invert value while playing"); return; } //Do not invert while playing

            Inverted ^= true;
            difference *= -1;
            endScale.Value = startScale.Value + difference;

            Debug.Log("Position Value Inverted");
        }

        public void Invert_Value_Positive() { if (Inverted) Invert_Value(); }
        public void Invert_Value_Negative() { if (!Inverted) Invert_Value(); }


        private void InvertStartEnd()
        {
            (startScale.Value, endScale.Value) = (endScale.Value, startScale.Value);

            Evaluate(0);
            MTools.SetDirty(this);
        }

        protected override void Reset()
        {
            base.Reset();
            if (startScale.UseConstant)
                startScale.Value = Object.localScale;
        }
    }
}