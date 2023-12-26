using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public enum WallCheckType
    {
        wallHitOn,
        wallHitOff,
    }

    [SerializeField] private WallCheckType checkType;

    private bool isWallHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isWallHit  = true; //�ڽ� �ݶ��̴��� ���� ��Ҵٸ� true
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isWallHit = false;//�ڽ� �ݶ��̴��� ���� ���� �ʾҴٸ� false
    }

    /// <summary>
    /// isWallHit�� ���� �ޱ� ���� �Լ�
    /// </summary>
    /// <returns></returns>
    public bool WallHit()
    {
        return isWallHit;
    } 
    
    public WallCheckType wallCheckType()
    {
        return checkType;
    }
}
