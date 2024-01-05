using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPreFab : MonoBehaviour
{
    public static TrashPreFab instance; //����� �����յ��� ���� ��ũ��Ʈ

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
