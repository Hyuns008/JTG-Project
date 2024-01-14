using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNextLevel : MonoBehaviour
{
    [Header("���� ���� �� óġ���� �� Ȱ��ȭ�Ǵ� ������Ʈ")]
    [SerializeField] private GameObject nextLevel;
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
        nextLevelOn();
    }

    /// <summary>
    /// ���� ���� ���� óġ �� ���� ������ ���� ����
    /// </summary>
    private void nextLevelOn()
    {
        if (enemyCount <= 0)
        {
            nextLevel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
