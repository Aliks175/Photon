
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField]
    private float _mouseSensitivity = 0.4f; // чувствительность мышы 
    [SerializeField]
    private float _moveSpeed = 2f; // скорость перемещения 

    private float _rotationX;
    private float _rotationY;

    private MyAction_Actions inputActions; // скрипт с нашими картами действий 
    private InputAction _moveAction;// действие движение Vector2 (wasd)
    private InputAction _sprintAction;// действие ускорения на shift 
    private InputAction _heightAction;// действие Высоты на Q E регулировка высоты 1D Axes (float)
    private InputAction _lookAction;// действие поворота камеры отслеживает мышь Vector2

    private void Start()
    {
        inputActions = new MyAction_Actions();// создаем скрипт 
        _moveAction = inputActions.Player.Move;
        _sprintAction = inputActions.Player.Sprint;
        _heightAction = inputActions.Player.Height;
        _lookAction = inputActions.Player.Look;

        inputActions.Player.Enable(); // вкл карту действий 
       
    }

    void Update()
    {
        if (!_photonView.IsMine) return;
        Move();
        Rotate();
    }

    void Move()
    {
        float shiftMult = 1f;
        if (_sprintAction.IsPressed()) // если зажата shift то мы умножаем нашу скорость на 3 
        {
            shiftMult = 3f;
        }

        Vector2 movement = _moveAction.ReadValue<Vector2>();// считываем движение 
        float right = movement.x;
        float forward = movement.y;



        float up = _heightAction.ReadValue<float>(); // регулируем высоту 

        Vector3 offset = new Vector3(right, up, forward) * _moveSpeed * shiftMult * Time.unscaledDeltaTime;// кэшируем движение 
        transform.Translate(offset); // что это ? 
    }

    void Rotate()
    {
        if (inputActions.Player.UnLockLook.IsPressed()) // во время того как нажата ПКМ = True
        {
            Vector3 _mouseDelta = _lookAction.ReadValue<Vector2>();// считывает дельту мышы

            _rotationX -= _mouseDelta.y * _mouseSensitivity;
            _rotationY += _mouseDelta.x * _mouseSensitivity;

            transform.localEulerAngles = new Vector3(_rotationX, _rotationY, 0f);
        }
    }

}
