using Newtonsoft.Json;
using UnityEngine;
using static Player;
using static Status;

public class SaveObject : MonoBehaviour
{
    public class SavedObjectData
    {
        public float playerDamage;
        public int playerArmor;
        public int playerHp;
        public int playerCurHp;
        public float playerCritical;
        public float playerCriDamage;
        public int playerLevel;
        public float playerMaxExp;
        public float playerExp;
        public int playerLevelPoint;
        public int playerWeaponA;
        public int playerWeaponB;
        public int playerPet;
    }

    private SavedObjectData savedObj = new SavedObjectData();

    public class SaveStatusData
    {
        public bool lv2click;
        public bool lv4click;
        public bool lv6click;
        public bool lv8click;
        public bool lv10click;
        public bool damage;
        public bool armor;
        public bool health;
    }

    private SaveStatusData saveStatusData = new SaveStatusData();

    private Player player;
    [SerializeField] private Status status;

    private bool dataReset = false;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        playerObjectResetData();
    }

    /// <summary>
    /// �÷��̾ ���嵥���͸� �������� �� �޾ƿ� �ʱⵥ����
    /// </summary>
    private void playerObjectResetData()
    {
        if (dataReset == true)
        {
            savedObj.playerDamage = 0;
            savedObj.playerArmor = 0;
            savedObj.playerHp = 50;
            savedObj.playerCurHp = 50;
            savedObj.playerCritical = 0;
            savedObj.playerCriDamage = 2;
            savedObj.playerLevel = 1;
            savedObj.playerMaxExp = 1;
            savedObj.playerExp = 0;
            savedObj.playerLevelPoint = 0;
            savedObj.playerWeaponA = 0;
            savedObj.playerWeaponB = 0;
            savedObj.playerPet = 0;

            saveStatusData.lv2click = false;
            saveStatusData.lv4click = false;
            saveStatusData.lv6click = false;
            saveStatusData.lv8click = false;
            saveStatusData.lv10click = false;
            saveStatusData.damage = false;
            saveStatusData.armor = false;
            saveStatusData.health = false;

            saveData();
            statusSaveData();

            string savedData = PlayerPrefs.GetString("playerDataSave");

            savedObj = JsonConvert.DeserializeObject<SavedObjectData>(savedData);

            player.PlayerSavedData(savedObj);

            dataReset = false;
        }
    }

    /// <summary>
    /// �÷��̾ ���� �Ѿ ������ ����Ǵ� ������
    /// </summary>
    /// <param name="_playerData"></param>
    public void PlayerObjectSaveData(PlayerData _playerData)//Ÿ ��ũ��Ʈ���� �����ϸ� �ȴٰ� �˷��ִ� �Լ�
    {
        if (dataReset == false)
        {
            savedObj.playerDamage = _playerData.playerDamage;
            savedObj.playerArmor = _playerData.playerArmor;
            savedObj.playerHp = _playerData.playerHp;
            savedObj.playerCurHp = _playerData.playerCurHp;
            savedObj.playerCritical = _playerData.playerCritical;
            savedObj.playerCriDamage = _playerData.playerCriDamage;
            savedObj.playerLevel = _playerData.playerLevel;
            savedObj.playerMaxExp = _playerData.playerMaxExp;
            savedObj.playerExp = _playerData.playerExp;
            savedObj.playerLevelPoint = _playerData.playerLevelPoint;
            savedObj.playerWeaponA = _playerData.playerWeaponA;
            savedObj.playerWeaponB = _playerData.playerWeaponB;
            savedObj.playerPet = _playerData.playerPet;
        }

        saveData();
    }

    private void saveData()//Json���� ����
    {
        string savedData = JsonConvert.SerializeObject(savedObj);
        PlayerPrefs.SetString("playerDataSave", savedData);
    }

    /// <summary>
    /// �÷��̾ ���� �Ѿ ������ ����Ǵ� �������ͽ� ������
    /// </summary>
    /// <param name="_statusData"></param>
    public void PlayerStatusSaveData(StatusData _statusData)
    {
        if (dataReset == false)
        {
            saveStatusData.lv2click = _statusData.lv2click;
            saveStatusData.lv4click = _statusData.lv4click;
            saveStatusData.lv6click = _statusData.lv6click;
            saveStatusData.lv8click = _statusData.lv8click;
            saveStatusData.lv10click = _statusData.lv10click;
            saveStatusData.damage = _statusData.damage;
            saveStatusData.armor = _statusData.armor;
            saveStatusData.health = _statusData.health;
        }

        statusSaveData();
    }

    private void statusSaveData()//Json���� ����
    {
        string savedStatusData = JsonConvert.SerializeObject(saveStatusData);
        PlayerPrefs.SetString("playerStatusDataSave", savedStatusData);
    }

    /// <summary>
    /// ���̺� �����͸� �ҷ��� �÷��̾� �����Ϳ� ���� ����� �Լ�
    /// </summary>
    public void PlayerObjectDataLoad()
    {
        string savedData = PlayerPrefs.GetString("playerDataSave");
        string savedStatusData = PlayerPrefs.GetString("playerStatusDataSave");

        if (savedData == string.Empty && savedStatusData == string.Empty)
        {
            return;
        }

        savedObj = JsonConvert.DeserializeObject<SavedObjectData>(savedData);
        saveStatusData = JsonConvert.DeserializeObject<SaveStatusData>(savedStatusData);

        player.PlayerSavedData(savedObj);
        status.PlayerStatusSavedData(saveStatusData);
    }

    public void PlayerDataResetOn(bool _reset)
    {
        dataReset = _reset;
    }
}
