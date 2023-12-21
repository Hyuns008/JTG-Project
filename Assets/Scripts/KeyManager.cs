using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    [Header("�÷��̾� Ű ����")]
    [SerializeField] KeyCode playerLeftKey;
    [SerializeField] KeyCode playerRightKey;
    [SerializeField] KeyCode playerJumpKey;

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
        return playerLeftKey;
    }

    public KeyCode PlayerRightKey()
    {
        return playerRightKey;
    }

    public KeyCode PlayerJumpKey()
    {
        return playerJumpKey;
    }
}
