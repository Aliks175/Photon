using UnityEngine;

public class CharacterData : MonoBehaviour, ICharacterData
{
    [SerializeField] private CharacterDataConfig _playerConfig;

    public PlayerHealth PlayerHealth
    {
        get
        {
            if (_playerHealth == null)
            {
                PlayerHealth = new PlayerHealth(_playerConfig.StartHealthPlayer);
            }
            return _playerHealth;
        }
        private set
        {
            _playerHealth = value;
        }
    }

    public SystemLevel SystemLevel
    {
        get
        {
            if (_systemLevel == null)
            {
                _systemLevel = new SystemLevel(_playerConfig.LevelLine, _playerConfig.ListUp, this);
            }
            return _systemLevel;
        }
        private set
        {
            _systemLevel = value;
        }
    }

    public InventoryPlayer Inventory => _inventory;

    private PlayerHealth _playerHealth;
    private SystemLevel _systemLevel;
    private InventoryPlayer _inventory;

    public void Initialization()
    {
        PlayerHealth ??= new PlayerHealth(_playerConfig.StartHealthPlayer);
        SystemLevel ??= new SystemLevel(_playerConfig.LevelLine, _playerConfig.ListUp, this);
        _inventory = GetComponent<InventoryPlayer>();

        SetUp();
    }

    private void SetUp()
    {
        _inventory.Initialization(this);
        _playerHealth.Initialization();
        _systemLevel.Initialization();
    }
}

public interface ICharacterData
{
    public PlayerHealth PlayerHealth { get; }
    public SystemLevel SystemLevel { get; }
    public InventoryPlayer Inventory { get; }

    public abstract void Initialization();

}