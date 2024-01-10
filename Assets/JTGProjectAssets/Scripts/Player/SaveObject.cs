using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

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

    private Player player;

    [SerializeField] private bool dataReset = false;

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

            saveData();

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
