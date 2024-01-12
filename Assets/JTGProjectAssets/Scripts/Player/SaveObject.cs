using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        }

        saveData();
    }

    private void saveData()//Json���� ����
    {
        string savedData = JsonConvert.SerializeObject(savedObj);
        PlayerPrefs.SetString("playerDataSave", savedData);
    }

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
        string savedData = JsonConvert.SerializeObject(saveStatusData);
        PlayerPrefs.SetString("playerStatusDataSave", savedData);
    }

    /// <summary>
    /// ���̺� �����͸� �ҷ��� �÷��̾� �����Ϳ� ���� ����� �Լ�
    /// </summary>
    public void PlayerObjectDataLoad()
    {
        string savedData = PlayerPrefs.GetString("playerDataSave");

        if (savedData == string.Empty)
        {
            return;
        }

        savedObj = JsonConvert.DeserializeObject<SavedObjectData>(savedData);

        player.PlayerSavedData(savedObj);
    }

    public void PlayerDataResetOn(bool _reset)
    {
        dataReset = _reset;
    }
}
