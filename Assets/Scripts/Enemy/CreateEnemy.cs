using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    [Header("�� ���� ���� ����")]
    [SerializeField] private List<GameObject> enemyPrefab = new List<GameObject>();
    [SerializeField] private Transform enemyCreatePos;

    [Header("������ �� ������")]
    [SerializeField] private int enemyPrefabNember;

    private TrashPreFab trashPreFab;

    private bool isCreate = false;

    private void Start()
    {
        trashPreFab = TrashPreFab.instance;
    }

    private void Update()
    {
        enemyCreate();
    }

    private void enemyCreate()
    {
        if (isCreate == false && (enemyPrefabNember <= enemyPrefab.Count - 1))
        {
            Instantiate(enemyPrefab[enemyPrefabNember], enemyCreatePos.position, Quaternion.identity, trashPreFab.transform);
            isCreate = true;
        }
    }
}
