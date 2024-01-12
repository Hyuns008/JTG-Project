using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveObject;

public class Status : MonoBehaviour
{
    public class StatusData
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

    private StatusData statusData = new StatusData();
    private bool dataSave = false;

    [SerializeField] private Player player;
    [SerializeField] private SaveObject saveObject;

    [Header("�÷��̾��� �ɷ�ġ�� �ø� ��ư")]
    [SerializeField] private Button damageUp; //����ġ ����Ʈ�� �̿��Ͽ� ���ݷ��� ��� ��Ű�� ��ư
    [SerializeField] private Button armorUp; //����ġ ����Ʈ�� �̿��Ͽ� ������ ��� ��Ű�� ��ư
    [SerializeField] private Button hpUp; //����ġ ����Ʈ�� �̿��Ͽ� ü���� ��� ��Ű�� ��ư
    [SerializeField] private Button criticalUp; //����ġ ����Ʈ�� �̿��Ͽ� ġ��Ÿ Ȯ���� ��� ��Ű�� ��ư

    [Header("�÷��̾��� �нú� ��ų�� ���� �� �ִ� ��ư")]
    [SerializeField] private List<Button> lvpassiveSkills; //���� 2���� 10���� Ư�� ������ �����ؾ����� ���� �� �ִ� ��ųƮ��
    private bool lv2click = false;
    private bool lv4click = false;
    private bool lv6click = false;
    private bool lv8click = false;
    private bool lv10click = false;
    private bool damage = false;
    private bool armor = false;
    private bool health = false;

    [Header("�÷��̾��� ������ �־��� UI��")]
    [SerializeField] private TMP_Text playerLevel;
    [SerializeField] private TMP_Text playerExp;
    [SerializeField] private TMP_Text playerLevelPoint;
    [SerializeField] private TMP_Text playerDamage;
    [SerializeField] private TMP_Text playerArmor;
    [SerializeField] private TMP_Text playerCurHp;
    [SerializeField] private TMP_Text playerCritical;
    [SerializeField] private TMP_Text playerCriDamage;

    private void Awake()
    {
        statusUpButton();
        passiveUpButton();
    }

    private void Start()
    {
        saveObject.PlayerObjectDataLoad();
    }

    private void Update()
    {
        setPlayerData();
        playerStatus();
        levelCheck();
    }

    /// <summary>
    /// �÷��̾��� �������ͽ� �����͸� ���̺��ϱ� ���� �Լ�
    /// </summary>
    private void setPlayerData()
    {
        if (dataSave == true)
        {
            statusData.lv2click = lv2click;
            statusData.lv4click = lv4click;
            statusData.lv6click = lv6click;
            statusData.lv8click = lv8click;
            statusData.lv10click = lv10click;
            statusData.damage = damage;
            statusData.armor = armor;
            statusData.health = health;

            saveObject.PlayerStatusSaveData(statusData);

            dataSave = false;
        }
    }

    /// <summary>
    /// ���� ����Ʈ�� �̿��� �ɷ�ġ�� �ø��� ��ư
    /// </summary>
    private void statusUpButton()
    {
        damageUp.onClick.AddListener(() =>
        {
            player.playerPointDamageUp();
        });

        armorUp.onClick.AddListener(() =>
        {
            player.playerPointArmorUp();
        });

        hpUp.onClick.AddListener(() =>
        {
            player.playerPointHpUp();
        });

        criticalUp.onClick.AddListener(() =>
        {
            player.playerPointCriticalUp();
        });
    }

    /// <summary>
    /// Ư�� ������ �����ϸ� ���� �� �ִ� �нú� ��ų ��ư
    /// </summary>
    private void passiveUpButton()
    {
        lvpassiveSkills[0].onClick.AddListener(() =>
        {
            player.PlayerStatusDamage(2);
            player.PlayerStatusArmor(1);
            player.PlayerStatusHp(10);
            lv2click = true;
            lv4click = true;
            lvpassiveSkills[0].interactable = false;
        });

        lvpassiveSkills[1].onClick.AddListener(() =>
        {
            player.PlayerStatusDamage(5);
            damage = true;
            lv4click = false;
            lv6click = true;
            lvpassiveSkills[1].interactable = false;
        });

        lvpassiveSkills[2].onClick.AddListener(() =>
        {
            player.PlayerStatusArmor(3);
            armor = true;
            lv4click = false;
            lv6click = true;
            lvpassiveSkills[2].interactable = false;
        });

        lvpassiveSkills[3].onClick.AddListener(() =>
        {
            player.PlayerStatusHp(30);
            health = true;
            lv4click = false;
            lv6click = true;
            lvpassiveSkills[3].interactable = false;
        });

        lvpassiveSkills[4].onClick.AddListener(() =>
        {
            player.PlayerStatusCritical(10, 0);
            lv6click = false;
            lv8click = true;
            lvpassiveSkills[4].interactable = false;
        });

        lvpassiveSkills[5].onClick.AddListener(() =>
        {
            player.PlayerStatusArmor(3);
            lv6click = false;
            lv8click = true;
            lvpassiveSkills[5].interactable = false;
        });

        lvpassiveSkills[6].onClick.AddListener(() =>
        {
            player.PlayerStatusHp(30);
            lv6click = false;
            lv8click = true;
            lvpassiveSkills[6].interactable = false;
        });

        lvpassiveSkills[7].onClick.AddListener(() =>
        {
            player.PlayerStatusCritical(0, 0.2f);
            lv8click = false;
            lv10click = true;
            lvpassiveSkills[7].interactable = false;
        });

        lvpassiveSkills[8].onClick.AddListener(() =>
        {
            player.PlayerStatusArmor(3);
            lv8click = false;
            lv10click = true;
            lvpassiveSkills[8].interactable = false;
        });

        lvpassiveSkills[9].onClick.AddListener(() =>
        {
            player.PlayerStatusHp(30);
            lv8click = false;
            lv10click = true;
            lvpassiveSkills[9].interactable = false;
        });

        lvpassiveSkills[10].onClick.AddListener(() =>
        {
            player.PlayerStatusDamage(5);
            player.PlayerStatusCritical(10, 0.3f);
            lv10click = false;
            lvpassiveSkills[10].interactable = false;
        });

        lvpassiveSkills[11].onClick.AddListener(() =>
        {
            player.PlayerStatusArmor(3);
            lv10click = false;
            lvpassiveSkills[11].interactable = false;
        });

        lvpassiveSkills[12].onClick.AddListener(() =>
        {
            player.PlayerStatusHp(30);
            lv10click = false;
            lvpassiveSkills[12].interactable = false;
        });
    }

    /// <summary>
    /// ��ųƮ���� �ߺ� ó���� �����ϱ� ���� �Լ�
    /// </summary>
    private void levelCheck()
    {
        if (player.PlayerLevelReturn() >= 2 && lv2click == false)
        {
            lvpassiveSkills[0].interactable = true;
        }
        else if (player.PlayerLevelReturn() < 2 || lv2click == true)
        {
            lvpassiveSkills[0].interactable = false;
        }

        if (player.PlayerLevelReturn() >= 4 && lv4click == true)
        {
            lvpassiveSkills[1].interactable = true;
            lvpassiveSkills[2].interactable = true;
            lvpassiveSkills[3].interactable = true;
        }
        else if (player.PlayerLevelReturn() < 4 || lv4click == false)
        {
            lvpassiveSkills[1].interactable = false;
            lvpassiveSkills[2].interactable = false;
            lvpassiveSkills[3].interactable = false;
        }

        if (player.PlayerLevelReturn() >= 6 && lv6click == true)
        {
            if (damage == true)
            {
                lvpassiveSkills[4].interactable = true;
                lvpassiveSkills[5].interactable = false;
                lvpassiveSkills[6].interactable = false;
            }
            else if (armor == true)
            {
                lvpassiveSkills[4].interactable = false;
                lvpassiveSkills[5].interactable = true;
                lvpassiveSkills[6].interactable = false;
            }
            else if (health == true)
            {
                lvpassiveSkills[4].interactable = false;
                lvpassiveSkills[5].interactable = false;
                lvpassiveSkills[6].interactable = true;
            }
        }
        else if (player.PlayerLevelReturn() < 6 || lv6click == false)
        {
            lvpassiveSkills[4].interactable = false;
            lvpassiveSkills[5].interactable = false;
            lvpassiveSkills[6].interactable = false;
        }

        if (player.PlayerLevelReturn() >= 8 && lv8click == true)
        {
            if (damage == true)
            {
                lvpassiveSkills[7].interactable = true;
                lvpassiveSkills[8].interactable = false;
                lvpassiveSkills[9].interactable = false;
            }
            else if (armor == true)
            {
                lvpassiveSkills[7].interactable = false;
                lvpassiveSkills[8].interactable = true;
                lvpassiveSkills[9].interactable = false;
            }
            else if (health == true)
            {
                lvpassiveSkills[7].interactable = false;
                lvpassiveSkills[8].interactable = false;
                lvpassiveSkills[9].interactable = true;
            }
        }
        else if (player.PlayerLevelReturn() < 8 || lv8click == false)
        {
            lvpassiveSkills[7].interactable = false;
            lvpassiveSkills[8].interactable = false;
            lvpassiveSkills[9].interactable = false;
        }

        if (player.PlayerLevelReturn() >= 10 && lv10click == true)
        {
            if (damage == true)
            {
                lvpassiveSkills[10].interactable = true;
                lvpassiveSkills[11].interactable = false;
                lvpassiveSkills[12].interactable = false;
            }
            else if (armor == true)
            {
                lvpassiveSkills[10].interactable = false;
                lvpassiveSkills[11].interactable = true;
                lvpassiveSkills[12].interactable = false;
            }
            else if (health == true)
            {
                lvpassiveSkills[10].interactable = false;
                lvpassiveSkills[11].interactable = false;
                lvpassiveSkills[12].interactable = true;
            }
        }
        else if (player.PlayerLevelReturn() < 10 || lv10click == false)
        {
            lvpassiveSkills[10].interactable = false;
            lvpassiveSkills[11].interactable = false;
            lvpassiveSkills[12].interactable = false;
        }
    }

    /// <summary>
    /// �÷��̾��� ������ �ð������� Ȯ�� �����ֱ� ���� �Լ�
    /// </summary>
    private void playerStatus()
    {
        if (player.PlayerStatusOpen() == true)
        {
            playerLevel.text = $"LV: {player.PlayerLevelReturn()}";
            playerExp.text = $"Exp: {player.PlayerExpReturn()} / {player.PlayerMaxExpReturn()}";
            playerLevelPoint.text = $"����Ʈ: {player.PlayerLevelPointReturn()}";
            playerDamage.text = $"���ݷ�: {player.PlayerDamageReturn()}";
            if (player.PlayerBuffDamageReturn() <= 0)
            {
                playerDamage.text = $"���ݷ�: {player.PlayerDamageReturn()}";
            }
            else if (player.PlayerBuffDamageReturn() > 0)
            {
                playerDamage.text = $"���ݷ�: {player.PlayerDamageReturn()} <color=red>+{player.PlayerBuffDamageReturn()}</color>"; //��ġ�ؽ�Ʈ ����ϱ�
            }
            playerArmor.text = $"����: {player.PlayerArmorReturn()}";
            playerCurHp.text = $"ü��: {player.PlayerCrHpReturn()} / {player.PlayerMaxHpReturn()}";
            playerCritical.text = $"ġ��ŸȮ��: {player.PlayerCriticalReturn().ToString("F1")}%";
            playerCriDamage.text = $"ġ��Ÿ������: {(int)player.PlayerCriDamageReturn() * 100}%";
        }
    }

    public void PlayerStatusSavedData(SaveStatusData _saveStatusData)
    {
        lv2click = _saveStatusData.lv2click;
        lv4click = _saveStatusData.lv4click;
        lv6click = _saveStatusData.lv6click;
        lv8click = _saveStatusData.lv8click;
        lv10click = _saveStatusData.lv10click;
        damage = _saveStatusData.damage;
        armor = _saveStatusData.armor;
        health = _saveStatusData.health;
    }

    public void PlayerStatusSaveOn(bool _save)
    {
        dataSave = _save;
    }
}
