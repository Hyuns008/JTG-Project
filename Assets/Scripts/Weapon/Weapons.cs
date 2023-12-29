using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Weapons : MonoBehaviour
{
    public enum WeaponType
    {
        weaponTypeA,
        weaponTypeB,
        weaponTypeC,
        weaponTypeD,
    }

    [SerializeField] private WeaponType weaponType;

    private BoxCollider2D weaponBoxColl2D;

    //���⿡ ������ �Ŵ���
    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    private TrashPreFab trashPreFab;
    private ItemPickUp itemPickUp;
    private WeaponSkill weaponSkill;
    private SpriteRenderer weaponRen;

    [Header("���� ����")]
    [SerializeField, Tooltip("���� ������")] private float shootDelay = 0.5f;
    private float shootTimer;
    [SerializeField, Tooltip("������")] private bool shootingOn = false;

    [Header("�Ѿ� ����")]
    [SerializeField, Tooltip("�Ѿ� ������")] private GameObject bullet;
    [SerializeField, Tooltip("�Ѿ��� ������ ��ġ")] private Transform bulletPos;
    [Space]
    [SerializeField, Tooltip("�ִ� źâ")] private int maxMagazine; //źâ�� ���� �ִ� �Ѿ� ��
    [SerializeField, Tooltip("���� źâ")] private int curMagazine; //���� ��â�� �������� �Ѿ� ��
    [SerializeField, Tooltip("������ �ð�")] private float reloadingTime; //�������� ���� �ð�
    [SerializeField] private float reloadingTimer;
    private bool reloading = false;
    private float curReloadingSlider;
    [SerializeField] private float autoReloadingTimer;

    [Header("�ݱ� Ű �̹���")]
    [SerializeField] private GameObject pickUpKeyImage;
    private bool imageOff = false;

    [Header("źâ UI")]
    [SerializeField] TMP_Text magazineText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (imageOff == true)
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            pickUpKeyImage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (imageOff == true)
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            pickUpKeyImage.SetActive(false);
        }
    }

    private void Awake()
    {
        itemPickUp = GetComponent<ItemPickUp>();
        weaponBoxColl2D = GetComponent<BoxCollider2D>();
        weaponSkill = GetComponent<WeaponSkill>();
        weaponRen = GetComponent<SpriteRenderer>();

        autoReloadingTimer = 3.0f;
    }

    private void Start()
    {
        gameManager = GameManager.Instance; //���� �Ŵ����� ������ gameManager�� ��� ��
        keyManager = KeyManager.instance; //Ű�Ŵ����� ������ keyManager ��� ��

        trashPreFab = TrashPreFab.instance;

        curReloadingSlider = gameManager.ReloadingUI().value;

        pickUpKeyImage.SetActive(false);
    }

    private void Update()
    {
        autoReloading();

        magazineText.text = $"{curMagazine} / {maxMagazine}";

        if (shootingOn == false || itemPickUp.GetItemType().ToString() != "Weapon")
        {
            return;
        }

        if (imageOff == true)
        {
            pickUpKeyImage.SetActive(false);
        }

        reloadingWeapon();
        shootWeapon();
    }

    /// <summary>
    /// ���⸦ 3�� �̻� �տ� ��� ���� ������ �ڵ����� �������� �� �Ѿ��� ���� ä����
    /// </summary>
    private void autoReloading()
    {
        if (weaponRen.enabled == false && curMagazine != maxMagazine)
        {
            autoReloadingTimer -= Time.deltaTime;
            if (autoReloadingTimer < 0.0f)
            {
                autoReloadingTimer = 3.0f;
                gameManager.ReloadingObj().SetActive(false);
                gameManager.ReloadingUI().value = 1.0f;
                reloadingTimer = reloadingTime;
                curMagazine = maxMagazine;
                reloading = false;
            }
        }
    }

    /// <summary>
    /// ���� �������� ����ϴ� �Լ�
    /// </summary>
    private void reloadingWeapon()
    {
        if (weaponSkill.SkillAOn() == true)
        {
            gameManager.ReloadingObj().SetActive(false);
            gameManager.ReloadingUI().value = 1.0f;
            reloadingTimer = reloadingTime;
            curMagazine = maxMagazine;
            reloading = false;
            return;
        }

        if (Input.GetKeyDown(keyManager.ReloadingKey()) && curMagazine != maxMagazine && reloading == false)
        {
            curMagazine = 0;
        }

        if (reloading == true) //�������� true�� Ÿ�̸Ӹ� �۵��ð� UI�� Ȱ��ȭ��
        {
            reloadingTimer -= Time.deltaTime;
            gameManager.ReloadingUI().value = reloadingTimer / reloadingTime;
            gameManager.ReloadingObj().SetActive(true);
            if (reloadingTimer < 0)
            {
                gameManager.ReloadingObj().SetActive(false);
                reloadingTimer = reloadingTime;
                curMagazine = maxMagazine;
                reloading = false;
            }
        }
    }   

    /// <summary>
    /// ���� �߻縦 ����ϴ� �Լ�
    /// </summary>
    private void shootWeapon()
    {
        if (curMagazine <= 0) //���� �Ѿ��� ���ٸ� �������� true�� �ٲٰ� �߻縦 �� �ϰ� ��
        {
            reloading = true;
            return;
        }

        if (shootTimer != 0.0f) //�߻� ������
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer < 0)
            {
                shootTimer = 0.0f;
            }
        }

        if ((Input.GetKeyDown(keyManager.PlayerAttackKey()) && shootTimer == 0.0f && reloading == false) //���콺�� ���� �� ���� �߻�
            || (Input.GetKey(keyManager.PlayerAttackKey()) && shootTimer == 0.0f && reloading == false)) //���콺�� ������ ������ �߻�
        {
            shootBullet();
            shootTimer = shootDelay;

            if (weaponSkill.SkillAOn() == true)
            {
                return;
            }

            curMagazine--;
        }
    }

    /// <summary>
    /// �Ѿ� ������ ����ϴ� �Լ�
    /// </summary>
    /// <param name="_rot"></param>
    private void shootBullet(float _rot = 0.0f)
    {
        Instantiate(bullet, bulletPos.position, bulletPos.rotation, trashPreFab.transform);
        if (weaponType.ToString() == "weaponTypeB")
        {
            Instantiate(bullet, bulletPos.position, bulletPos.rotation * Quaternion.Euler(new Vector3(0, 0, 5)), trashPreFab.transform);
            Instantiate(bullet, bulletPos.position, bulletPos.rotation * Quaternion.Euler(new Vector3(0, 0, -5)), trashPreFab.transform);
        }
    }

    /// <summary>
    /// ���� �������� ����ϴ� �Լ�, ������ �� �� �ִ��� ������
    /// </summary>
    /// <param name="_shooting"></param>
    /// <returns></returns>
    public void ShootingOn(bool _shooting)
    {
        shootingOn = _shooting;
    }

    public bool ShootingOnCheck()
    {
        return shootingOn;
    }

    public void PickUpImageOff(bool _ImageOff)
    {
        imageOff = _ImageOff;
    }

    /// <summary>
    /// �ҷ� �������� ���� �����͸� �޾ƿ��� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public GameObject CurBullet()
    {
        return bullet;
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� �Ѿ� ������ �ϱ� ���� ����ϴ� �Լ�
    /// </summary>
    /// <param name="_bullet"></param>
    public void BulletChange(GameObject _bullet)
    {
        bullet = _bullet;
    }
}
