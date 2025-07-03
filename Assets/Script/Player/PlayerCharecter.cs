using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterData), typeof(PlayerMove), typeof(PlayerAnimation))]

public class PlayerCharecter : MonoBehaviourPunCallbacks
{
    public ICharacterData CharacterData { get { return _characterData; } private set { _characterData = value; } }
    public PlayerMove PlayerMoved { get { return _playerMove; } private set { _playerMove = value; } }

    [SerializeField] private float _movementSmoothingSpeed = 1f;


    private InputAction _moveAction;
    private InputAction _defenceAction;
    private InputAction _attackAction;
    private InputAction _pauseAction;
    private InputActionMap _actionMapPlayer;
    private InputActionMap _actionMapMenu;


    private ICharacterData _characterData;
    private PlayerMove _playerMove;
    private PlayerAnimation _playerAnimation;
    private Vector3 _rawInputMovement;
    private Vector3 _smoothInputMovement;

    private Coroutine _coroutine;

    private const string actionMapPlayerControls = "Player";
    private const string actionMapMenuControls = "UI";

    public override void OnDisable()
    {
        base.OnDisable();
        _actionMapMenu.FindAction("Pause").performed -= OnPause;
        _actionMapPlayer.FindAction("Pause").performed -= OnPause;
        _defenceAction.canceled -= OnDefence;
        _defenceAction.performed -= OnDefence;
        _attackAction.started -= OnAttack;
        _moveAction.performed -= OnMovement;
        _moveAction.canceled -= OnMovement;
    }


    private void Update()
    {
        if (!photonView.IsMine) return;
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
        UpdatePlayerAnimationMovement();
    }

    private void SetUpPlayerCharecterV1_3()
    {
        _actionMapPlayer = InputSystem.actions.FindActionMap(actionMapPlayerControls);
        _actionMapMenu = InputSystem.actions.FindActionMap(actionMapMenuControls);
        _moveAction = InputSystem.actions.FindAction("Move");
        _attackAction = InputSystem.actions.FindAction("Attack");
        _defenceAction = InputSystem.actions.FindAction("Defence");

        CharacterData.Initialization();

        CharacterData.PlayerHealth.OnDied += OnDied;
        CharacterData.PlayerHealth.OnHited += TakeDamage;
        _attackAction.started += OnAttack;
        _actionMapMenu.FindAction("Pause").performed += OnPause;
        _actionMapPlayer.FindAction("Pause").performed += OnPause;
        _defenceAction.performed += OnDefence;
        _defenceAction.canceled += OnDefence;
        _moveAction.performed += OnMovement;
        _moveAction.canceled += OnMovement;
    }
    public void SetSub()
    {
        SetupPlayerCharecter();
        SetUpPlayerCharecterV1_3();
        _playerMove.SetupPlayerMove();
        _playerAnimation.SetupPlayerAnimation();
    }

    public void EnableGameplayControls()
    {
        _actionMapPlayer.Enable();
        _actionMapMenu.Disable();
        OnStopMove();
    }

    public void EnablePauseMenuControls()
    {
        _actionMapPlayer.Disable();
        _actionMapMenu.Enable();
        OnStopMove();
    }

    public void OnPause(InputAction.CallbackContext value)
    {
        GameLogicManager.Instance.Pause();
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            Vector2 inputMovement = value.ReadValue<Vector2>();
            _rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
        }
        if (value.phase == InputActionPhase.Canceled)
        {
            OnStopMove();
        }
    }

    private void OnStopMove()
    {
        _rawInputMovement = Vector3.zero;
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Started)
        {
            _playerAnimation.PlayAttackAnimation();

            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(.1f, .1f);
            }
        }
    }

    public void OnDefence(InputAction.CallbackContext value)
    {
        if (value.phase == InputActionPhase.Performed)
        {
            _playerMove.Speed = 3f;
            _playerAnimation.PlayDefenceAnimation(true);
        }
        if ((value.phase == InputActionPhase.Canceled))
        {
            _playerMove.Speed = 5f;
            _playerAnimation.PlayDefenceAnimation(false);
        }
    }


    private void TakeDamage()
    {
        _playerAnimation.PlayGetDamageAnimation();
        _actionMapPlayer.Disable();
        OnStopMove();

        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(OnDizzy());
        }

    }

    private void OnDied()
    {
        _playerAnimation.PlayDieAnimation();

        _coroutine = StartCoroutine(OnRespawn());
    }

    private IEnumerator OnRespawn()
    {
        yield return new WaitForSeconds(5f);
        _actionMapPlayer.Enable();
        _characterData.PlayerHealth.FullHealth();
        //_playerHealth.Respawn();
        _playerAnimation.PlayRespawnAnimation();
        _coroutine = null;
    }

    private IEnumerator OnDizzy()
    {
        yield return new WaitForSeconds(2f);
        _actionMapPlayer.Enable();
        _coroutine = null;
    }




    private void SetupPlayerCharecter()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        CharacterData = GetComponent<CharacterData>();
    }

    private void CalculateMovementInputSmoothing()
    {
        _smoothInputMovement = Vector3.Lerp(_smoothInputMovement, _rawInputMovement, Time.deltaTime * _movementSmoothingSpeed);
    }

    private void UpdatePlayerMovement()
    {
        _playerMove.UpdateMovementData(_smoothInputMovement);
    }

    private void UpdatePlayerAnimationMovement()
    {
        float magnitude = _smoothInputMovement.magnitude;
        if (magnitude < 0.001f) { magnitude = 0; }
        _playerAnimation.UpdateMovementAnimation(magnitude);
    }
}
