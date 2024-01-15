using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Animator anim;
        
    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("������ �̵��ӵ�")] private float bossSpeed;
    [SerializeField, Tooltip("������ ����ü��")] private int bossCurHp;
    [SerializeField, Tooltip("������ ���Ϻ� ü��")] private List<int> bossPatternHp;
    [SerializeField] private int curPattern;
    private bool patternChange = false;
    private float changeTimer;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();

        bossCurHp = bossPatternHp[0];

        changeTimer = 0;
    }

    private void Update()
    {
        timers();
        bossDeadCheck();
    }

    /// <summary>
    /// ������ ���õ� Ÿ�̸� ����
    /// </summary>
    private void timers()
    {
        if (patternChange == true)
        {
            changeTimer += Time.deltaTime;
            if (changeTimer > 2)
            {
                ++curPattern;
                patternHpChange();
                changeTimer = 0;
                patternChange = false;
            }
        }
    }

    /// <summary>
    /// ������ ������ ����� �� ü�µ� ����
    /// </summary>
    private void patternHpChange()
    {
        if (curPattern > 2)
        {
            return;
        }

        if (curPattern == 1)
        {
            bossCurHp = bossPatternHp[1];
        }
        else if (curPattern == 2)
        {
            bossCurHp = bossPatternHp[2];
        }
    }

    /// <summary>
    /// ������ ��� ������ ��� �� �׾��� �� ����
    /// </summary>
    private void bossDeadCheck()
    {
        if (bossCurHp <= 0 && curPattern > 2)
        {
            Destroy(gameObject);
        }
        else if (bossCurHp <= 0)
        {
            patternChange = true;
        }
    }
}
