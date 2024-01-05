using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    [SerializeField] private Player playerSc;

    private bool isWallHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerSc == null)
        {
            return;
        }

        isWallHit  = true; //�ڽ� �ݶ��̴��� ���� ��Ҵٸ� true
        playerSc.playerWallCheck(isWallHit, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerSc == null)
        {
            return;
        }

        isWallHit = false;//�ڽ� �ݶ��̴��� ���� ���� �ʾҴٸ� false
        playerSc.playerWallCheck(isWallHit, collision);
    }
}
