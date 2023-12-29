using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkill : MonoBehaviour
{
    //���� ��ų�� ������ �Ŵ���
    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    private TrashPreFab trashPreFab;

    private Weapons weapons;

    [Header("��ų ������")]
    [SerializeField] private List<GameObject> skillPrefabs = new List<GameObject>();

    [Header("��ų�� ��Ÿ�� ��ġ �� ȸ�� ��")]
    [SerializeField] private Transform skillPos;
    [SerializeField] private Transform skillRot;

    [Header("��ų ����")]
    [SerializeField] private float skillCoolTime = 1.0f;
    private float skillCoolTimer = 0.0f;
    private bool skillCoolOn = false;
    private float coolTime;

    private void Awake()
    {
        weapons = GetComponent<Weapons>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        keyManager = KeyManager.instance;

        trashPreFab = TrashPreFab.instance;

        skillCoolTimer = skillCoolTime;

        coolTime = gameManager.WeaponCoolTimePanelImage().fillAmount;
    }

    private void Update()
    {
        skillCool();
        weaponSpecialAttack();
    }

    /// <summary>
    /// ��ų ��� �� ��Ÿ�ӵ�
    /// </summary>
    private void skillCool()
    {
        if (skillCoolOn == true)
        {
            gameManager.WeaponSkillCoolTime(true);

            if (skillCoolTimer > 1.0f)
            {
                string timerTextInt = $"{(int)skillCoolTimer}";
                gameManager.WeaponCoolTimeText().text = timerTextInt;
            }
            else if (skillCoolTimer < 1.0f)
            {
                string timerTextInt = $"{skillCoolTimer.ToString("F1")}";
                gameManager.WeaponCoolTimeText().text = timerTextInt;
            }

            skillCoolTimer -= Time.deltaTime;
            gameManager.WeaponCoolTimePanelImage().fillAmount = skillCoolTimer / skillCoolTime;
            if (skillCoolTimer < 0.0f)
            {
                skillCoolOn = false;
                gameManager.WeaponSkillCoolTime(false);
                gameManager.WeaponCoolTimePanelImage().fillAmount = coolTime;
                skillCoolTimer = skillCoolTime;
            }
        }
    }

    /// <summary>
    /// ���� ��ų�� ����ϴ� �Լ�
    /// </summary>
    private void weaponSpecialAttack()
    {
        if (Input.GetKeyDown(keyManager.WeaponSkiilKey()) && skillCoolOn == false)
        {
            skillCoolOn = true;

            GameObject skillObj = Instantiate(skillPrefabs[0], skillPos.position,
                    skillRot.rotation, trashPreFab.transform);
        }
    }
}
