using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapons : MonoBehaviour
{
    public enum WeaponType
    {
        weaponA,
        weaponB, 
        weaponC, 
        weaponD,
    }

    [SerializeField] WeaponType weaponType;

    //���⿡ ������ �Ŵ���
    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    //private int weaponNum = 0;

    [Header("���� ����")]
    [SerializeField, Tooltip("���� ������")] private float shootDelay = 0.5f;
    private float shootTimer;

    [Header("�Ѿ� ����")]
    [SerializeField, Tooltip("�Ѿ� ������")] private GameObject bullet;
    [SerializeField, Tooltip("�Ѿ��� ������ ��ġ")] private Transform bulletPos;
    [SerializeField, Tooltip("�Ѿ� �����յ��� ��ġ�� ������Ʈ")] private Transform bulletObj;
    [Space]
    [SerializeField, Tooltip("�ִ� źâ")] private int maxMagazine; //źâ�� ���� �ִ� �Ѿ� ��
    [SerializeField, Tooltip("���� źâ")] private int curMagazine; //���� ��â�� �������� �Ѿ� ��
    [SerializeField, Tooltip("������ �ð�")] private float reroadingTime; //�������� ���� �ð�
    private float reroadingTimer;
    private bool reroading = false;
    [Space]
    [SerializeField, Tooltip("������ UI ������Ʈ")] private GameObject reroadingUI; //�������� �ð�ȭ �����ֱ� ���� UI ������Ʈ
    [SerializeField, Tooltip("������ UI �����̴�")] private Slider reroadingSlider;
    private float curReroadingSlider;

    private void Start()
    {
        gameManager = GameManager.Instance; //���� �Ŵ����� ������ gameManager�� ��� ��
        keyManager = KeyManager.instance; //Ű�Ŵ����� ������ keyManager ��� ��

        curReroadingSlider = reroadingSlider.value;
    }

    private void Update()
    {
        reroadingWeapon();
        shootWeapon();
    }

    /// <summary>
    /// ���� �������� ����ϴ� �Լ�
    /// </summary>
    private void reroadingWeapon()
    {
        if (reroading == true) //�������� true�� Ÿ�̸Ӹ� �۵��ð� UI�� Ȱ��ȭ��
        {
            reroadingTimer -= Time.deltaTime;
            reroadingSlider.value = reroadingTimer;
            reroadingUI.SetActive(true);
            if (reroadingTimer < 0)
            {
                reroadingUI.SetActive(false);
                reroadingSlider.value = 1.0f;
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
        Instantiate(bullet, bulletPos.position, bulletPos.rotation, bulletObj);
    }
}
