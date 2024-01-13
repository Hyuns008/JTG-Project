using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        gamePauseOn();
    }

    private void gamePauseOn()
    {
        if (gamePause == false)
        {
            Time.timeScale = 1;
        }
        else if (gamePause == true)
        {
            Time.timeScale = 0;
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

    public void SetGamePause(bool _gamePause)
    {
        gamePause = _gamePause;
    }

    public bool GetGamePause()
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

    public Transform ItemDropTrs()
    {
        return itemDropTrs;
    }
}
