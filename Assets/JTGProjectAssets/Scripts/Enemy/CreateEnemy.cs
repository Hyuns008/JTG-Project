using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    [Header("�� ���� ���� ����")]
    [SerializeField, Tooltip("������ ���� ������")] private List<GameObject> enemyPrefab;
    [SerializeField, Tooltip("���� ������ ��ġ")] private List<Transform> enemyCreatePosA;
    [SerializeField, Tooltip("���� ������ ��ġ")] private List<Transform> enemyCreatePosB;
    [SerializeField, Tooltip("���� ������ ��ġ")] private List<Transform> enemyCreatePosC;
    [SerializeField, Tooltip("���� ������ ��ġ")] private List<Transform> enemyCreatePosD;
    [SerializeField, Tooltip("���� ������ ��ġ")] private List<Transform> enemyCreatePosE;
    [SerializeField, Tooltip("ó������ ������ ������Ʈ�� ������ ����")] private int enemyPhase;
    [SerializeField, Tooltip("����� �� ������ ���� �ݶ��̴�")] private List<BoxCollider2D> boxCollider2D;
    private int phase;
    private List<GameObject> enemyPrefabList = new List<GameObject>();

    private TrashPreFab trashPreFab;

    private void Awake()
    {
        phase = enemyPhase;
    }

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
        if (phase != -1)
        {
            if (phase == enemyPhase)
            {
                for (int i = 0; i < enemyPrefab.Count; i++)
                {
                    GameObject enemyObj = Instantiate(enemyPrefab[i], enemyCreatePosA[i].position, Quaternion.identity, trashPreFab.transform);
                    enemyPrefabList.Add(enemyObj);
                }
                phase--;
            }
            else if (phase < enemyPhase)
            {
                for (int i = 0; i < enemyPrefabList.Count; i++)
                {
                    enemyPrefabList[i].transform.position = enemyCreatePosB[i].position;
                    enemyPrefab[i].SetActive(true);
                }
                phase--;
            }
        }
    }
}
