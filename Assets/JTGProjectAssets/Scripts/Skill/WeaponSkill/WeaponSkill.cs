using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkill : MonoBehaviour
{
    public enum WeaponSkillType
    {
        skillA,
        skillB,
        skillC,
        skillD,
    }

    [SerializeField] WeaponSkillType skillType;

    //���� ��ų�� ������ �Ŵ���
    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    private TrashPreFab trashPreFab;
    private Weapons weapons;

    [Header("��ų ������")]
    [SerializeField, Tooltip("���� ��ų ������")] private List<GameObject> skillPrefabs = new List<GameObject>();

    [Header("��ų�� ��Ÿ�� ��ġ �� ȸ�� ��")]
    [SerializeField, Tooltip("��ų�� �ߵ��� ��ġ")] private Transform skillPos;
    [SerializeField, Tooltip("��ų�� ȸ�� ��")] private Transform skillRot;

    [Header("��ų �⺻ ����")]
    [SerializeField, Tooltip("��ų ��Ÿ��")] private float skillCoolTime = 1.0f;
    private float skillCoolTimer = 0.0f; //��ų ��Ÿ�̸�
    private bool skillCoolOn = false; //��ų�� ���� �� ��Ÿ���� �۵���Ű�� ���� ����
    private SpriteRenderer weaponRen; //������ ��������Ʈ ������
    private bool useSkill = false;
    private float skillDamage;

    [Header("��ų UI")]
    [SerializeField, Tooltip("Ȱ��ȭ ��ų ��ų ������Ʈ")] private GameObject weaponSkill;
    [SerializeField, Tooltip("��ų �̹���")] private Image weaponSkilImage; //��ų �̹����� �޾ƿ� ����
    [SerializeField, Tooltip("��ų ��Ÿ�� �̹���")] private GameObject weaponSkillCoolTimePanel;
    private Image weaponCoolTimePanelImage; //��ų ��Ÿ�� �̹��� ������Ʈ�� �޾ƿ� ����
    [SerializeField, Tooltip("��ų ��Ÿ�� �ؽ�Ʈ")] private GameObject weaponSkillCollTimeText;
    private TMP_Text weaponCoolTimeText; //��ų ��Ÿ�� �ؽ�Ʈ�� �޾ƿ� ����

    [Header("��ųA ����")]
    [SerializeField, Tooltip("��ų A�� ���ӽð�")] private float skillATime = 0.0f;
    private float skillATimer = 0.0f; //��ų ���� �� ���ư��� Ÿ�̸�
    private bool skillAOn = false; //��ų A�� �ߵ��� �Ǿ������� üũ���ִ� ����
    private GameObject curBullet; //�Ѿ��� ���� �����͸� ���� ����

    [Header("��ųC ����")]
    [SerializeField, Tooltip("��ų C�� ��¡ 1�ܰ� �ð�")] private float chargingLevel1Time;
    [SerializeField, Tooltip("��ų C�� �ִ� ��¡ �ð�")] private float lastChargingTime;
    [SerializeField] private GameObject chargingObj;
    private Image chargingImage;
    private float chargingTimer;

    private void Awake()
    {
        weapons = GetComponent<Weapons>();
        weaponRen = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        keyManager = KeyManager.instance;

        trashPreFab = TrashPreFab.Instance;

        weaponCoolTimePanelImage = weaponSkillCoolTimePanel.GetComponent<Image>();
        weaponCoolTimeText = weaponSkillCollTimeText.GetComponent<TMP_Text>();
        chargingImage = chargingObj.GetComponent<Image>();

        skillCoolTimer = skillCoolTime;
    }

    private void Update()
    {
        weaponSkillOn();

        if (gameManager.GetGamePause() == true)
        {
            return;
        }

        if (weaponSkill.activeSelf == false)
        {
            skillATimer = 0;
            chargingTimer = 0;
            chargingImage.fillAmount = 0;
            return;
        }

        skillCool();
        weaponSpecialAttack();
        skillAControll();
    }

    private void weaponSkillOn()
    {
        if (weaponRen.enabled == true && weapons.ShootingOnCheck() == true)
        {
            weaponSkill.SetActive(true);
        }
        else if (weaponRen.enabled == false && weapons.ShootingOnCheck() == false)
        {
            weaponSkill.SetActive(false);
        }
    }

    /// <summary>
    /// ��ų ��� �� ��Ÿ�ӵ�
    /// </summary>
    private void skillCool()
    {
        if (skillCoolOn == true)
        {
            if (weaponRen.enabled == true)
            {
                weaponSkillCoolTimePanel.SetActive(true);
                weaponSkillCollTimeText.SetActive(true);
            }

            if (skillCoolTimer > 1.0f)
            {
                string timerTextInt = $"{(int)skillCoolTimer}";
                weaponCoolTimeText.text = timerTextInt;
            }
            else if (skillCoolTimer < 1.0f)
            {
                string timerTextInt = $"{skillCoolTimer.ToString("F1")}";
                weaponCoolTimeText.text = timerTextInt;
            }

            skillCoolTimer -= Time.deltaTime;
            weaponCoolTimePanelImage.fillAmount = skillCoolTimer / skillCoolTime;
            if (skillCoolTimer < 0.0f)
            {
                skillCoolOn = false;
                weaponSkillCoolTimePanel.SetActive(false);
                weaponSkillCollTimeText.SetActive(false);
                skillCoolTimer = skillCoolTime;
            }
        }
    }

    /// <summary>
    /// ���� ��ų�� ����ϴ� �Լ�
    /// </summary>
    private void weaponSpecialAttack()
    {
        //��ų A�� B�� �����ϴ� �ڵ�
        if (Input.GetKeyDown(keyManager.WeaponSkiilKey()) && skillCoolOn == false
            && weapons.ShootingOnCheck() == true && weaponRen.enabled == true && skillType.ToString() != "skillC")
        {
            skillCoolOn = true;

            useSkill = true;

            weapons.WeaponUseShooting(false);

            if (skillType.ToString() == "skillA")
            {
                curBullet = weapons.CurBullet();
                weapons.BulletChange(skillPrefabs[0]);
                skillATimer = skillATime;
                skillAOn = true;
                useSkill = false;
            }
            else if (skillType.ToString() == "skillB")
            {
                GameObject bulletObj = Instantiate(skillPrefabs[0], skillPos.position, skillRot.rotation, trashPreFab.transform);
                Bullet bulletSc = bulletObj.GetComponent<Bullet>();
                bulletSc.BulletDamage(skillDamage, 2, true, weapons.HitCriticalCheck());
                useSkill = false;
            }
            else if (skillType.ToString() == "skillD")
            {
                Instantiate(skillPrefabs[0], skillPos.position, skillRot.rotation, trashPreFab.transform);
            }
        }

        //��ų C�� �����ϴ� ���� �ڵ�
        if (Input.GetKey(keyManager.WeaponSkiilKey()) && skillCoolOn == false
            && weapons.ShootingOnCheck() == true && weaponRen.enabled == true && skillType.ToString() == "skillC")
        {
            useSkill = true;
            chargingObj.SetActive(true);
            chargingTimer += Time.deltaTime;
            chargingImage.fillAmount = chargingTimer / lastChargingTime;
        }
        else if (Input.GetKeyUp(keyManager.WeaponSkiilKey()) && skillCoolOn == false
            && weapons.ShootingOnCheck() == true && weaponRen.enabled == true && skillType.ToString() == "skillC")
        {
            skillCoolOn = true;

            useSkill = false;

            weapons.WeaponUseShooting(false);

            if (chargingTimer < chargingLevel1Time)
            {
                GameObject skillObj = Instantiate(skillPrefabs[0], skillPos.position,
                skillRot.rotation, trashPreFab.transform);
                Bullet bulletSc = skillObj.GetComponent<Bullet>();
                bulletSc.BulletDamage(skillDamage, 1.2f, true, weapons.HitCriticalCheck());
                skillObj.transform.localScale = new Vector2(1.0f, 1.0f);
            }
            else if (chargingTimer >= chargingLevel1Time && chargingTimer <= lastChargingTime)
            {
                GameObject skillObj = Instantiate(skillPrefabs[1], skillPos.position,
                skillRot.rotation, trashPreFab.transform);
                Bullet bulletSc = skillObj.GetComponent<Bullet>();
                bulletSc.BulletDamage(skillDamage, 2f, true, weapons.HitCriticalCheck());
                skillObj.transform.localScale = new Vector2(2.0f, 2.0f);
            }
            else if (chargingTimer >= lastChargingTime)
            {
                GameObject skillObj = Instantiate(skillPrefabs[2], skillPos.position,
                skillRot.rotation, trashPreFab.transform);
                Bullet bulletSc = skillObj.GetComponent<Bullet>();
                bulletSc.BulletDamage(skillDamage, 3f, true, weapons.HitCriticalCheck());
                skillObj.transform.localScale = new Vector2(3.0f, 3.0f);
            }

            chargingObj.SetActive(false);

            chargingImage.fillAmount = 0.0f;

            chargingTimer = 0.0f;
        }
    }

    /// <summary>
    /// ��ų A�� ����� ��� �ִ� �Լ�
    /// </summary>
    private void skillAControll()
    {
        if (skillAOn == true)
        {
            skillATimer -= Time.deltaTime;
            if (skillATimer < 0.0f)
            {
                weapons.BulletChange(curBullet);
                skillAOn = false;
                skillATimer = skillATime;
            }
        }
    }

    public bool SkillAOn()
    {
        return skillAOn;
    }

    public bool UseSkill()
    {
        return useSkill;
    }

    public void WeaponSkillOff(bool _skillOff)
    {
        weaponSkill.SetActive(_skillOff);
    }

    public void GetSkillDamage(float _skillDmg)
    {
        skillDamage = _skillDmg;
    }
}
