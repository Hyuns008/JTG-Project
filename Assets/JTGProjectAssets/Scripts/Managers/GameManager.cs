using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using static Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("���� ����")]
    [SerializeField] private bool gamePause = false;

    [Header("�÷��̾� ���� ����")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerStartPos;
    private bool playerStartOn = false;

    [Header("�߷�")]
    [SerializeField] private float gravity;

    [Header("������UI")]
    [SerializeField] private GameObject reloadingObj;
    [SerializeField] private Slider reloadingUI;

    [Header("�÷��̾� UI")]
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject playerSkill;
    [SerializeField] private Image playerSkillImage;
    [SerializeField] private GameObject playerSkillCoolTimePanel;
    [SerializeField] private Image playerCoolTimePanelImage;
    [SerializeField] private GameObject playerSkillCollTimeText;
    [SerializeField] private TMP_Text playerCoolTimeText;
    [SerializeField] private GameObject playerDashCoolPanel;
    [SerializeField] private Image playerDashCoolPanelImage;
    [SerializeField] private GameObject playerDashCoolText;
    [SerializeField] private TMP_Text playerDashCoolTimeText;
    [SerializeField] private Slider playerHpSlider;
    [SerializeField] private TMP_Text playerHpText;

    [Header("������ ��� ������Ʈ ��ġ")]
    [SerializeField] private Transform itemDropTrs;

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

    private void Start()
    {
        playerStart();
    }

    private void Update()
    {
        if (playerPrefab == null)
        {
            playerUI.SetActive(false);
        }
        else if (playerPrefab != null)
        {
            playerUI.SetActive(true);
        }
    }

    private void playerStart()
    {
        if (playerStartOn == false && playerPrefab != null)
        {
            playerPrefab.transform.position = playerStartPos.position;
            playerPrefab.transform.SetParent(playerStartPos);
        }
    }

    public void DeadScenesLoad()
    {
        SceneManager.LoadSceneAsync("DeadScene");
    }

    public bool GamePause()
    {
        return gamePause;
    }

    public GameObject PlayerPrefab()
    {
        return playerPrefab;
    }

    public float GravityScale()
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

    public GameObject PlayerDashPanel()
    {
        return playerDashCoolPanel;
    }

    public GameObject PlayerDashText()
    {
        return playerDashCoolText;
    }

    public Image PlayerDashCoolPanelImage()
    {
        return playerDashCoolPanelImage;
    }

    public TMP_Text PlayerDashCoolTimeText()
    {
        return playerDashCoolTimeText;
    }

    public Slider PlayerHpSlider()
    {
        return playerHpSlider;
    }

    public TMP_Text PlayerHpText()
    {
        return playerHpText;
    }

    public Transform ItemDropTrs()
    {
        return itemDropTrs;
    }
}