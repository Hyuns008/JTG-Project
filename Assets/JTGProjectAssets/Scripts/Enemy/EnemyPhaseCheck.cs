using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhaseCheck : MonoBehaviour
{
    [SerializeField, Tooltip("�� ���� ��ũ��Ʈ")] private CreateEnemy createEnemy;
    [SerializeField, Tooltip("���� ������ ������Ʈ")] private GameObject nextPhaseObj;
    [SerializeField, Tooltip("���� ����� ���� óġ�� ��")] private int enemyKill;
    private int enemyCount;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyCount--;
        }
    }

    private void Awake()
    {
        enemyCount = enemyKill;
    }

    private void Update()
    {
        nextPhaseOn();
    }

    /// <summary>
    /// ���� ���� ���� óġ �� ���� ����
    /// </summary>
    private void nextPhaseOn()
    {
        if (enemyCount <= 0)
        {
            createEnemy.gameObject.SetActive(false);
            nextPhaseObj.SetActive(true);
        }
    }
}
