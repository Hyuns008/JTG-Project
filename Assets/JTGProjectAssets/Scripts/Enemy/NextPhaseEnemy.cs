using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPhaseEnemy : MonoBehaviour
{
    [Header("���� ���� �� óġ���� �� ���� ��Ȱ��ȭ")]
    [SerializeField, Tooltip("óġ�ؾ��ϴ� ���� ��")] private int enemyKill;
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
            gameObject.SetActive(false);
        }
    }
}
