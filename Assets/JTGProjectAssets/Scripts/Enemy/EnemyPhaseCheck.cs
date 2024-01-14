using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhaseCheck : MonoBehaviour
{
    [SerializeField, Tooltip("�� ���� ��ũ��Ʈ")] private CreateEnemy createEnemy;

    [SerializeField, Tooltip("���� ������ ������Ʈ")] private GameObject nextPhaseObj;
    private int enemyCount;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyCount--;
        }
    }

    private void Start()
    {
        enemyCount = createEnemy.GetCreatePosCount();
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
