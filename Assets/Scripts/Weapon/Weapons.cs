using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapons : MonoBehaviour
{
    private BoxCollider2D weaponBoxColl2D;

    //���⿡ ������ �Ŵ���
    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    private TrashPreFab trashPreFab;

    private ItemPickUp itemPickUp;
    //private int weaponNum = 0;

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
    [SerializeField, Tooltip("������ �ð�")] private float reroadingTime; //�������� ���� �ð�
    private float reroadingTimer;
    private bool reroading = false;
    private float curReroadingSlider;

    private void Awake()
    {
        itemPickUp = GetComponent<ItemPickUp>();
        weaponBoxColl2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance; //���� �Ŵ����� ������ gameManager�� ��� ��
        keyManager = KeyManager.instance; //Ű�Ŵ����� ������ keyManager ��� ��

        trashPreFab = TrashPreFab.instance;

        curReroadingSlider = gameManager.ReloadingUI().value;
    }

    private void Update()
    {
        if (shootingOn == false || itemPickUp.GetItemType().ToString() != "Weapon")
        {
            return;
        }

        reroadingWeapon();
        shootWeapon();
    }

    /// <summary>
    /// ���� �������� ����ϴ� �Լ�
    /// </summary>
    private void reroadingWeapon()
    {
        if (Input.GetKeyDown(keyManager.ReroadingKey()))
        {
            curMagazine = 0;
        }

        if (reroading == true) //�������� true�� Ÿ�̸Ӹ� �۵��ð� UI�� Ȱ��ȭ��
        {
            reroadingTimer -= Time.deltaTime;
            gameManager.ReloadingUI().value = reroadingTimer;
            gameManager.ReloadingObj().SetActive(true);
            if (reroadingTimer < 0)
            {
                gameManager.ReloadingObj().SetActive(false);
                gameManager.ReloadingUI().value = 1.0f;
                reroadingTimer = reroadingTime;
                curMagazine = maxMagazine;
                reroading = false;
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
            reroading = true;
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

        if (Input.GetKeyDown(keyManager.PlayerAttackKey()) && shootTimer == 0.0f && reroading == false) //���콺�� ���� �� ���� �߻�
        {
            shootBullet();
            curMagazine--;
            shootTimer = shootDelay;
        }
        else if (Input.GetKey(keyManager.PlayerAttackKey()) && shootTimer == 0.0f && reroading == false) //���콺�� ������ ������ �߻�
        {
            shootBullet();
            curMagazine--;
            shootTimer = shootDelay;
        }
    }

    /// <summary>
    /// �Ѿ� ������ ����ϴ� �Լ�
    /// </summary>
    /// <param name="_rot"></param>
    private void shootBullet(float _rot = 0.0f)
    {
        Instantiate(bullet, bulletPos.position, bulletPos.rotation, trashPreFab.transform);
    }

    /// <summary>
    /// ���� �������� ����ϴ� �Լ�, ������ �� �� �ִ��� ������
    /// </summary>
    /// <param name="_shooting"></param>
    /// <returns></returns>
    public bool ShootingOn(bool _shooting)
    {
        return shootingOn = _shooting;
    }
}
