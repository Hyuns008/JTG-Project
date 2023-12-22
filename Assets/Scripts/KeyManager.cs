using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    [Header("�÷��̾� Ű ����")]
    [SerializeField] KeyCode playerMoveLeftKey;
    [SerializeField] KeyCode playerMoveRightKey;
    [SerializeField] KeyCode playerJumpKey;
    [SerializeField] KeyCode playerAttackKey;
    [SerializeField] KeyCode playerDashKey;

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

    public KeyCode PlayerLeftKey()
    {
        return playerMoveLeftKey;
    }

    public KeyCode PlayerRightKey()
    {
        return playerMoveRightKey;
    }

    public KeyCode PlayerJumpKey()
    {
        return playerJumpKey;
    }

    public KeyCode PlayerAttackKey()
    {
        return playerAttackKey;
    }

    public KeyCode PlayerDashKey()
    {
        return playerDashKey;
    }
}
