
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField]
    private float _mouseSensitivity = 0.4f; // ���������������� ���� 
    [SerializeField]
    private float _moveSpeed = 2f; // �������� ����������� 

    private float _rotationX;
    private float _rotationY;

    private MyAction_Actions inputActions; // ������ � ������ ������� �������� 
    private InputAction _moveAction;// �������� �������� Vector2 (wasd)
    private InputAction _sprintAction;// �������� ��������� �� shift 
    private InputAction _heightAction;// �������� ������ �� Q E ����������� ������ 1D Axes (float)
    private InputAction _lookAction;// �������� �������� ������ ����������� ���� Vector2

    private void Start()
    {
        inputActions = new MyAction_Actions();// ������� ������ 
        _moveAction = inputActions.Player.Move;
        _sprintAction = inputActions.Player.Sprint;
        _heightAction = inputActions.Player.Height;
        _lookAction = inputActions.Player.Look;

        inputActions.Player.Enable(); // ��� ����� �������� 
       
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
        if (_sprintAction.IsPressed()) // ���� ������ shift �� �� �������� ���� �������� �� 3 
        {
            shiftMult = 3f;
        }

        Vector2 movement = _moveAction.ReadValue<Vector2>();// ��������� �������� 
        float right = movement.x;
        float forward = movement.y;



        float up = _heightAction.ReadValue<float>(); // ���������� ������ 

        Vector3 offset = new Vector3(right, up, forward) * _moveSpeed * shiftMult * Time.unscaledDeltaTime;// �������� �������� 
        transform.Translate(offset); // ��� ��� ? 
    }

    void Rotate()
    {
        if (inputActions.Player.UnLockLook.IsPressed()) // �� ����� ���� ��� ������ ��� = True
        {
            Vector3 _mouseDelta = _lookAction.ReadValue<Vector2>();// ��������� ������ ����

            _rotationX -= _mouseDelta.y * _mouseSensitivity;
            _rotationY += _mouseDelta.x * _mouseSensitivity;

            transform.localEulerAngles = new Vector3(_rotationX, _rotationY, 0f);
        }
    }

}
