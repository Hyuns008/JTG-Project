using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class SaveObject : MonoBehaviour
{
    public class SavedObj
    {
        public float playerDamage;
        public int playerArmor;
        public int playerHp;
        public float playerCritical;
        public float playerCriDamage;
    }
    private SavedObj savedObj = new SavedObj();

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void JsonSave(PlayerData _playerData)//Ÿ ��ũ��Ʈ���� �����ϸ� �ȴٰ� �˷��ִ� �Լ�
    {       
        savedObj.playerDamage = _playerData.playerDamage;
        savedObj.playerArmor = _playerData.playerArmor;
        savedObj.playerHp = _playerData.playerHp;
        savedObj.playerCritical = _playerData.playerCritical;
        savedObj.playerCriDamage = _playerData.playerCriDamage;

        save();
    }

    private void save()//Json���� ����
    {
        string savedData = JsonConvert.SerializeObject(savedObj);
        PlayerPrefs.SetString("playerDataSave", savedData);
    }

    public void Load()
    {
        string savedData = PlayerPrefs.GetString("playerDataSave");

        if (savedData == string.Empty)
        {
            return;
        }

        savedObj = JsonConvert.DeserializeObject<SavedObj>(savedData);

        player.PlayerSavedData(savedObj);
    }
}
