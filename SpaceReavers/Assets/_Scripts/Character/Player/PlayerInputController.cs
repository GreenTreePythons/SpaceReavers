using UnityEngine;
using UnityEngine.InputSystem;
using Joystick = _Scripts.UI.Joystick;

namespace _Scripts.Character
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] Joystick m_Joystick;
        
        public Vector2 InputDirection { get; private set; }

        private void OnMove(InputValue value) => SetInputDirection(value.Get<Vector2>());
        private void SetInputDirection(Vector2 value) => InputDirection = value;
        
        private void OnEnable()
        {
            if(m_Joystick != null) m_Joystick.OnJoystickDirection += SetInputDirection;
        }

        private void OnDisable()
        {
            if(m_Joystick != null) m_Joystick.OnJoystickDirection -= SetInputDirection;
        }
    }
}