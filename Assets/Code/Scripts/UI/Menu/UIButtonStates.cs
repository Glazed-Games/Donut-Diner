using UnityEngine;

namespace DonutDiner.UIModule.Menu
{
    public class UIButtonStates : MonoBehaviour
    {
        public enum MouseState
        {
            NotHovering,
            Hovering,
            Pressed
        }

        public Material NotHoveringMat;
        public Material HoveringMat;
        public Material PressedMat;

        private MouseState _currentState = MouseState.NotHovering;
    }
}