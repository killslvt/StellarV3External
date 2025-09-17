using UnityEngine;

namespace StellarV3External.GUIButtonAPI
{
    internal class GUIButtonAPI
    {
        public class GUISingleButton
        {
            private string text;
            private Action action;
            private int yOffset;

            public GUISingleButton(string text, Action action, int yOffset)
            {
                this.text = text;
                this.action = action;
                this.yOffset = yOffset;
                Draw();
            }

            public void Draw()
            {
                GUI.color = Color.white;
                GUI.backgroundColor = Color.black;
                if (GUI.Button(new Rect(20, yOffset, 200, 25), text))
                {
                    action?.Invoke();
                }
            }
        }

        public class GUIToggleButton
        {
            private string text;
            private Action onAction;
            private Action offAction;
            private Func<bool> getState;
            private int yOffset;

            public GUIToggleButton(string text, Action onAction, Action offAction, Func<bool> getState, int yOffset)
            {
                this.text = text;
                this.onAction = onAction;
                this.offAction = offAction;
                this.getState = getState;
                this.yOffset = yOffset;
                Draw();
            }

            public void Draw()
            {
                bool currentState = getState();
                GUI.color = currentState ? Color.green : Color.red;
                GUI.backgroundColor = Color.black;
                string label = currentState ? $"{text} (On)" : $"{text} (Off)";
                if (GUI.Button(new Rect(20, yOffset, 200, 25), label))
                {
                    if (currentState)
                        offAction?.Invoke();
                    else
                        onAction?.Invoke();
                }
            }
        }


        public class GUISlider
        {
            private string label;
            private float min;
            private float max;
            private float value;
            private int yOffset;
            private Action<float> onValueChanged;

            public GUISlider(string label, float initialValue, float min, float max, int yOffset, Action<float> onValueChanged)
            {
                this.label = label;
                this.min = min;
                this.max = max;
                this.value = initialValue;
                this.yOffset = yOffset;
                this.onValueChanged = onValueChanged;
                Draw();
            }

            public void Draw()
            {
                GUI.color = Color.white;

                GUI.Label(new Rect(20, yOffset, 400, 20), $"{label}: {value:0.0}");

                float newValue = GUI.HorizontalSlider(new Rect(20, yOffset + 20, 200, 20), value, min, max);
                if (Math.Abs(newValue - value) > 0.001f)
                {
                    value = newValue;
                    onValueChanged?.Invoke(value);
                }
            }
        }
    }
}
