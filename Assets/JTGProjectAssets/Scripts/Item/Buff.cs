using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public enum BuffType
    {
        damage,
        armor,
        heal,
    }

    [Header("���� �ɷ�")]
    [SerializeField] BuffType buffType;

    [Header("���� ��ġ")]
    [SerializeField] private float buffValue;

    [Header("������ ����")]
    [SerializeField, Tooltip("���� �������� ������")] private float buffMove;
    private Vector3 moveVec;
    private Vector3 moveRot;
    private float changeTimer;

    private void Awake()
    {
        moveVec.y = 1f;
        moveRot.y = 360;
    }

    private void Update()
    {
        moveChangeTime();
        itmeMove();
    }

    /// <summary>
    /// ������ �������� ���� �ð��� ������ ���Ʒ��� �ٲ��ִ� �Լ�
    /// </summary>
    private void moveChangeTime()
    {
        changeTimer += Time.deltaTime;

        if (changeTimer >= 0.5f)
        {
            changeTimer = 0;
            moveVec.y *= -1f;
        }
    }

    /// <summary>
    /// �������� �������� ����ϴ� �Լ�
    /// </summary>
    private void itmeMove()
    {

        transform.position += moveVec * buffMove * Time.deltaTime;
        transform.Rotate(moveRot * Time.deltaTime * 0.5f);
    }

    public float BuffTypeValue()
    {
        float bufftypeValue = 0;
        if (buffType.ToString() == "damage")
        {
            bufftypeValue = buffValue;
        }
        else if (buffType.ToString() == "armor")
        {
            bufftypeValue = buffValue;
        }
        else if (buffType.ToString() == "heal")
        {
            bufftypeValue = buffValue;
        }
        return bufftypeValue;
    }

    public BuffType GetBuffType()
    {
        return buffType;
    }
}
