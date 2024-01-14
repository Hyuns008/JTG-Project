using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    private PoolManager poolManager;

    [Header("�� ���� ���� ����")]
    [SerializeField, Tooltip("������ ���� ��ȣ")] private int enemyCreateNumber;
    [SerializeField, Tooltip("���� ������ ��ġ")] private List<Transform> enemyCreatePos;
    private bool isCreate = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            enemyCreate();
        }
    }

    private void Start()
    {
        poolManager = PoolManager.Instance;
    }

    private void enemyCreate()
    {
        if (isCreate == false)
        {
            for (int i = 0; i < enemyCreatePos.Count; i++)
            {
                poolManager.PoolingPrefab(enemyCreateNumber, enemyCreatePos[i]);
            }
            isCreate = true;
        }
    }
}
