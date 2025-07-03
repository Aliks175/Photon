using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private int _playerMovementAnimationID;
    private int _playerAttackAnimationID;
    private int _playerGetDamageAnimationID;
    private int _playerDieAnimationID;
    private int _playerRespawnAnimationID;
    private int _playerDefenceAnimationID;

    private void SetupAnimationIDs()
    {
        _playerMovementAnimationID = Animator.StringToHash("Movement");
        _playerAttackAnimationID = Animator.StringToHash("Attack");
        _playerGetDamageAnimationID = Animator.StringToHash("Hit");
        _playerDieAnimationID = Animator.StringToHash("OnDie");
        _playerRespawnAnimationID = Animator.StringToHash("Respawn");
        _playerDefenceAnimationID = Animator.StringToHash("Def");
    }

    public void SetupPlayerAnimation()
    {
        _animator = GetComponentInChildren<Animator>();
        SetupAnimationIDs();
    }

    public void UpdateMovementAnimation(float movementBlendValue)
    {
        _animator.SetFloat(_playerMovementAnimationID, movementBlendValue);
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(_playerAttackAnimationID);
    }

    public void PlayDieAnimation()
    {
        _animator.SetTrigger(_playerDieAnimationID);
    }

    public void PlayGetDamageAnimation()
    {
        _animator.SetTrigger(_playerGetDamageAnimationID);
    }

    public void PlayRespawnAnimation()
    {
        _animator.SetTrigger(_playerRespawnAnimationID);
    }

    public void PlayDefenceAnimation(bool result)
    {
        _animator.SetBool(_playerDefenceAnimationID, result);
    }


}
