using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPreFab : MonoBehaviour
{
    public static TrashPreFab Instance; //����� �����յ��� ���� ��ũ��Ʈ

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
