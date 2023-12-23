using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    [Header("플레이어 키 설정")]
    [SerializeField] KeyCode playerMoveLeftKey;
    [SerializeField] KeyCode playerMoveRightKey;
    [SerializeField] KeyCode playerJumpKey;
    [SerializeField] KeyCode playerAttackKey;
    [SerializeField] KeyCode playerDashKey;
    [SerializeField] KeyCode reroadingKey;
    [SerializeField] KeyCode weaponChangeKey;
    [SerializeField] KeyCode pickUpItemKey;
    [SerializeField] KeyCode dropItemKey;

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

    public KeyCode ReroadingKey()
    {
        return reroadingKey;
    }

    public KeyCode WeaponChangeKey()
    {
        return weaponChangeKey;
    }

    public KeyCode PickUpItemKey()
    {
        return pickUpItemKey;
    }

    public KeyCode DropItemKey()
    {
        return dropItemKey;
    }
}
