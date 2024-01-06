using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;

public class MainSceneManager : MonoBehaviour
{
    [Header("��ư ����")]
    [SerializeField, Tooltip("������ ó�� ������ �� ��ư")] private Button startButton;
    [SerializeField, Tooltip("���� ������ �ҷ����� ��ư")] private Button loadButton;
    [SerializeField, Tooltip("���� ��ư")] private Button exitButton;
    private bool loadOn = false;
    [Space]
    [SerializeField, Tooltip("����� ����")] private GameObject saveFileObj;
    [SerializeField, Tooltip("���� ���� �ݱ� ��ư")] private Button saveCloseButton;
    [SerializeField, Tooltip("���� ���� A")] private Button saveFileA;
    [SerializeField, Tooltip("���� ���� B")] private Button saveFileB;
    [SerializeField, Tooltip("���� ���� C")] private Button saveFileC;


    private void Awake()
    {
        saveFileObj.SetActive(false);

        startButton.onClick.AddListener(() =>
        {
            saveFileObj.SetActive(true);
        });

        loadButton.onClick.AddListener(() =>
        {
            saveFileObj.SetActive(true);
            loadOn = true;
        });

        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        saveCloseButton.onClick.AddListener(() =>
        {
            saveFileObj.SetActive(false);
            loadOn = false;
        });

        saveFileA.onClick.AddListener(() =>
        {
            if (loadOn == false)
            {
                SceneManager.LoadSceneAsync("TutorialScene");
            }
            else if (loadOn == true)
            {

            }
        });

        saveFileB.onClick.AddListener(() =>
        {
            if (loadOn == false)
            {

            }
            else if (loadOn == true)
            {

            }
        });

        saveFileC.onClick.AddListener(() =>
        {
            if (loadOn == false)
            {

            }
            else if (loadOn == true)
            {

            }
        });
    }

    private void resetSaveFileA()
    {
    }
}
