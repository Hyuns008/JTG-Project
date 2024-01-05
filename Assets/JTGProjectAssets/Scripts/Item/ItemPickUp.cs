using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public enum ItemType
    {
        Weapon,
        Buff,
        Pet
    }

    [SerializeField] ItemType itemType;

    public ItemType GetItemType() //������ Ÿ���� ��ȯ�ϱ� ���� �Լ�
    {
        return itemType;
    }
}
