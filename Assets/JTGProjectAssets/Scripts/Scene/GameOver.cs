using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("���� ���� �� �������� ���ư��� ��ư")]
    [SerializeField] private Button gameOverButton;

    private void Awake()
    {
        gameOverButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync(0);
        });
    }
}
