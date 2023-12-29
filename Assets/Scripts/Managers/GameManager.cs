using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("���� ����")]
    [SerializeField] private bool gamePause = false;

    [Header("�߷�")]
    [SerializeField] private float gravity;

    [Header("������UI")]
    [SerializeField] private GameObject reloadingObj;
    [SerializeField] private Slider reloadingUI;

    [Header("��ų UI")]
    [SerializeField] private GameObject playerSkill;
    [SerializeField] private Image playerSkillImage;
    [SerializeField] private GameObject playerSkillCoolTimePanel;
    [SerializeField] private Image playerCoolTimePanelImage;
    [SerializeField] private GameObject playerSkillCollTimeText;
    [SerializeField] private TMP_Text playerCoolTimeText;
    [Space]
    [SerializeField] private GameObject weaponSkill;
    [SerializeField] private Image weaponSkillImage;
    [SerializeField] private GameObject weaponSkillCoolTimePanel;
    [SerializeField] private Image weaponCoolTimePanelImage;
    [SerializeField] private GameObject weaponSkillCollTimeText;
    [SerializeField] private TMP_Text weaponCoolTimeText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool GamePause()
    {
        return gamePause;
    }

    public float gravityScale()
    {
        return gravity;
    }

    public GameObject ReloadingObj()
    {
        return reloadingObj;
    }

    public Slider ReloadingUI()
    {
        return reloadingUI;
    }

    /// <summary>
    /// �÷��̾��� ��ų �̹����� ȭ�鿡 ���̰� ��
    /// </summary>
    public void PlayerSkillOn(bool _skillOn)
    {
        playerSkill.SetActive(_skillOn);
    }

    public Image PlayerSkillImage()
    {
        return playerSkillImage;
    }

    /// <summary>
    /// �÷��̾� ��ų�� ��� �� ��Ÿ���� ����� �� Ȱ��ȭ ��
    /// </summary>
    /// <param name="_objOn"></param>
    public void PlayerSkillCoolTime(bool _objOn)
    {
        playerSkillCoolTimePanel.SetActive(_objOn);
        playerSkillCollTimeText.SetActive(_objOn);
    }

    public Image PlayerCoolTimeImage()
    {
        return playerCoolTimePanelImage;
    }

    public TMP_Text PlayerCoolTimeText()
    {
        return playerCoolTimeText;
    }

    /// <summary>
    /// ������ ��ų �̹����� ȭ�鿡 ���̰� ��
    /// </summary>
    public GameObject WeaponSkillOn()
    {
        return weaponSkill;
    }

    public Image WeaponSkillImage()
    {
        return weaponSkillImage;
    }

    /// <summary>
    /// ���� ��ų�� ��� �� ��Ÿ���� ����� �� Ȱ��ȭ ��
    /// </summary>
    /// <param name="_objOn"></param>
    public void WeaponSkillCoolTime(bool _objOn)
    {
        weaponSkillCoolTimePanel.SetActive(_objOn);
        weaponSkillCollTimeText.SetActive(_objOn);
    }

    public Image WeaponCoolTimePanelImage()
    {
        return weaponCoolTimePanelImage;
    }

    public TMP_Text WeaponCoolTimeText()
    {
        return weaponCoolTimeText;
    }
}
