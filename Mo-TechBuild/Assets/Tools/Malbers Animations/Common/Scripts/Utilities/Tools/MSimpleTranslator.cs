using MalbersAnimations.Scriptables;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MalbersAnimations.Utilities
{
    /// <summary> Based on 3DKit Controller from Unity </summary>
    [AddComponentMenu("Malbers/Utilities/Transform/Simple Translator")]
    [SelectionBase]
    public class MSimpleTranslator : MSimpleTransformer
    {
        [ContextMenuItem("Invert", nameof(InvertStartEnd))]
        public Vector3Reference start;
        [ContextMenuItem("Invert", nameof(InvertStartEnd))]
        public Vector3Reference end = new(new Vector3(0, 2, 0));
        public bool Gizmos = true;
        private Vector3 difference;

        private void Awake()
        {
            Inverted = false;
            difference = end.Value - start.Value;
        }

        public override void Evaluate(float curveValue)
        {
            var curvePosition = m_Curve.Evaluate(curveValue);

            var pos = transform.TransformPoint(Vector3.Lerp(start, end, curvePosition));
            Object.position = pos;
        }


        /// <summary> When using Additive the rotation will continue from the last position  </summary>
        protected override void Pre_End()
        {
            if (loopType == LoopType.Once && endType == EndType.Additive)
            {
                start.Value = end.Value; //use the end value as start value
                end.Value += difference;
            }
        }

        protected override void Pos_End()
        {
            if (loopType == LoopType.Once && endType == EndType.Invert)
                InvertStartEnd();
        }

        [ContextMenu("Invert Value")]
        public void Invert_Value()
        {
            if (Playing) { Debug.Log("Cannot invert value while playing. Use this when Star and End Delay is greater than zero"); return; } //Do not invert while playing
            Inverted ^= true;
            difference *= -1;
            end.Value = start.Value + difference;
        }

        [ContextMenu("Invert Value +")]
        public void Invert_Value_Positive() { if (Inverted) Invert_Value(); }

        [ContextMenu("Invert Value -")]
        public void Invert_Value_Negative() { if (!Inverted) Invert_Value(); }


        [ContextMenu("Invert Start - End")]
        private void InvertStartEnd()
        {
            (start.Value, end.Value) = (end.Value, start.Value);
            Evaluate(value);
            MTools.SetDirty(this);
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(MSimpleTranslator), true)]
    public class SimpleTranslatorEditor : MSimpleTransformerEditor
    {
        void OnSceneGUI()
        {
            var t = target as MSimpleTranslator;

            if (!t.Gizmos) return;

            var start = t.transform.TransformPoint(t.start.Value);
            var end = t.transform.TransformPoint(t.end.Value);


            using (var cc = new EditorGUI.ChangeCheckScope())
            {
                start = Handles.PositionHandle(start, Quaternion.AngleAxis(180, t.transform.up) * t.transform.rotation);
                Handles.color = Color.yellow;
                Handles.SphereHandleCap(0, start, t.transform.rotation, 0.1f * t.transform.lossyScale.y, EventType.Repaint);
                Handles.SphereHandleCap(0, end, t.transform.rotation, 0.1f * t.transform.lossyScale.y, EventType.Repaint);

                end = Handles.PositionHandle(end, Quaternion.AngleAxis(180, t.transform.up) * t.transform.rotation);

                if (cc.changed)
                {
                    Undo.RecordObject(t, "Move Handles");
                    t.start.Value = t.transform.InverseTransformPoint(start);
                    t.end.Value = t.transform.InverseTransformPoint(end);
                    t.Evaluate(t.preview);
                }
            }
            Handles.DrawDottedLine(start, end, 5);
            Handles.Label(Vector3.Lerp(start, end, 0.5f), "Distance:" + (end - start).magnitude.ToString("F2"));
        }
    }
#endif
}