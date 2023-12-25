using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid; //�÷��̾��� ������ٵ�
    private RaycastHit2D hit2D;
    private BoxCollider2D playerBoxColl2D; //�÷��̾��� �ڽ� �ݶ��̴�
    private Camera mainCam; //���� ī�޶�
    private Animator anim;

    //�÷��̾ ������ �Ŵ���
    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    [Header("�̵�")]
    [SerializeField, Tooltip("�÷��̾��� �̵��ӵ�")] private float speed = 1.0f; //�÷��̾��� �̵��ӵ�
    private Vector2 moveVec; //�÷��̾��� �������� ���� ����
    private bool leftKey; //���� Ű�� ������ ��
    private bool rightKey; //������ Ű�� ������ ��

    private bool isGround = false; //�÷��̾ ���� �پ����� ������ false
    private float gravityVelocity;  //�߷°� ���õ� ���� �޾ƿ��� ���� ����

    [Header("����")]
    [SerializeField, Tooltip("������ �ϱ� ���� ��")] private float jumpPower = 1.0f; //������ �ϱ� ���� ��
    private bool isJump = false; //������ �ߴ���
    [SerializeField, Tooltip("���� ������ �ϱ� ���� �ð�")] private float doubleJumpTime = 1.0f; //���� ������ �ϱ� ���� ���ؽð�
    private float doubleJumpTimer = 1.0f; //���� ������ �ϱ� ���� �����ð�
    private bool noAirJump = false; //���� Ű�� ������ �ʰ� ���߿� ���� ��츦 üũ
    private bool jumpKey; //���� Ű�� ������ ��
    private bool animIsJump = false; //���� Ű�� ���� ������ �ߴ��� �� �ߴ��� üũ, ������ �ִϸ��̼� ����
    [SerializeField, Tooltip("���� �ִϸ��̼��� ���ӵǴ� �ð�")] private float animJumpTime = 1.0f; //������ ���� �� �ִϸ��̼��� �۵��� ���ߴ� ���ؽð�
    private float animTimer = 0.0f; //������ ���� �� �ִϸ��̼��� �۵��ϱ� ���ӽð�

    private Transform playerHand; //�÷��̾��� �� ��ġ
    private bool mouseAimRight = false;

    [Header("�뽬")]
    [SerializeField, Tooltip("�뽬 ��")] private float dashPower = 1.0f;
    [SerializeField, Tooltip("�뽬 ����")] private float dashRange = 1.0f;
    [SerializeField, Tooltip("�뽬 ������ �ð�")] private float dashCoolTime = 1.0f;
    private float dashCoolTimer = 0.0f;
    private float dashRangeTimer = 0.0f;
    private bool dashCoolOn = false;
    private bool isDash = false;
    private bool dashKey;
    private TrailRenderer dashEffect;

    [Header("���� ���� ����")]
    [SerializeField, Tooltip("���� ���� ������")] private float weaponsChangeCoolTime = 1.0f; //���� ������ ��� �ð�
    private float weaponsChangeCoolTimer = 0.0f; //���� ������ ���� Ÿ�̸�
    private bool weaponsChangeCoolOn = false; //���Ⱑ ������ �����ϸ� false
    private List<GameObject> weaponPrefabs = new List<GameObject>(); //���⸦ ���� �κ��丮 ����
    private GameObject getWeapon; //�ݶ��̴��� ���� ������Ʈ�� ��� �� ����
    private bool weaponSwap = false; //���� ������ ����ϴ� ����
    private int weaponCount = 0; //�ߺ����� ����������ʰ� �������ִ� ����

    private void OnDrawGizmos() //�ڽ�ĳ��Ʈ�� ���信�� ������ Ȯ���� �����ϰ� ������
    {
        if (playerBoxColl2D != null) //�ݶ��̴��� null�� �ƴ϶�� �ڽ����� ������ ���信�� Ȯ���� �� �ְ�
        {
            Gizmos.color = Color.red;
            Vector3 pos = playerBoxColl2D.bounds.center - new Vector3(0, 0.1f, 0);
            Gizmos.DrawWireCube(pos, playerBoxColl2D.bounds.size);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item" || collision.gameObject.tag == "Weapon") //�÷��̾ ���� ������Ʈ �±װ� ������ �Ǵ� �������� üũ
        {
            ItemPickUp itemPickUpSc = collision.gameObject.GetComponent<ItemPickUp>();  //�÷��̾ ���� ������Ʈ�� ������ ��ũ��Ʈ�� ������
            ItemPickUp.ItemType itemPickUpType = itemPickUpSc.GetItemType(); //
            if (itemPickUpType == ItemPickUp.ItemType.Weapon) //������ ������ Ÿ�԰� ��ũ��Ʈ�� ������ Ÿ���� ��ġ�ϸ� �۵�
            {           
                if (Input.GetKey(keyManager.PickUpItemKey())) //EŰ�� ������ �۵�
                {
                    if (weaponPrefabs.Count > 1) //���� ī��Ʈ�� 1���� ũ�� �ڽ��� ������ ������ ������Ʈ�� ��Ȱ��ȭ ���� ��
                    {
                        int count = weaponPrefabs.Count;
                        for (int i = 0; i < count - 1; i++)
                        {
                            weaponPrefabs[i].SetActive(false);
                        }
                    }

                    Weapons weaponsSc = collision.gameObject.GetComponent<Weapons>();
                    weaponsSc.ShootingOn(true);
                    if (weaponCount < 2) //weaponCount�� �̿��ؼ� ���Ⱑ �ߺ����� �������� �ʰ� ������
                    {
                        getWeapon = Instantiate(collision.gameObject, playerHand.position, playerHand.rotation, playerHand);
                        weaponCount++;
                    }
                    Destroy(collision.gameObject); //���Ⱑ ������ �� �� ȭ�鿡 �����ִ� ���⸦ ����
                    weaponPrefabs.Add(getWeapon); //���⸦ �κ��丮 ������ �ϴ� �迭�� �߰���
                }
            }
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); //�÷��̾� �ڽ��� ������ٵ� ������
        playerBoxColl2D = GetComponent<BoxCollider2D>(); //�÷��̾� �ڽ��� �ڽ��ݶ��̴�2D�� ������
        anim = GetComponent<Animator>(); //�÷��̾��� �ִϸ��̼��� ������
        playerHand = transform.Find("PlayerHand");
        dashEffect = GetComponent<TrailRenderer>();

        dashEffect.enabled = false;
    }

    private void Start()
    {
        gameManager = GameManager.Instance; //���� �Ŵ����� ������ gameManager�� ��� ��
        keyManager = KeyManager.instance; //Ű�Ŵ����� ������ keyManager ��� ��

        mainCam = Camera.main; //���� ī�޶� ������ mainCam�� ��� ��
    }

    private void Update()
    {
        if (gameManager.GamePause() == true) //���ӸŴ������� gamePause�� true��� �÷��̾� ������ ����
        {
            return;
        }

        playerCheckGround();
        playerAim();
        playerMove();
        playerGravity();
        playerDash();
        playerWeaponChange();
        playerAni();
    }

    /// <summary>
    /// �÷��̾ ������ �ƴ��� Ȯ���� ����ϴ� �Լ�
    /// </summary>
    private void playerCheckGround()
    {
        isGround = false; //�ٸ� ���ǽ��� ���� ��� �׻� isGround�� false�� ����

        if (gravityVelocity > 0) //gravityVelocity���� 0���� Ŭ ��� �Ʒ� �ڵ尡 ������ ����
        {
            return;
        }

        hit2D = Physics2D.BoxCast(playerBoxColl2D.bounds.center, playerBoxColl2D.bounds.size,
            0.0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground")); //�÷��̾��� �ڽ� �ݶ��̴��� ũ�⸦ �����ͼ� �ڽ�ĳ��Ʈ�� ����

        if (hit2D.transform != null && 
            hit2D.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) //ĳ��Ʈ�� ���� Ʈ�������� null�� �ƴϰ�, ���̾ Ground�� �۵�
        {
            isGround = true;
            isJump = false;
            noAirJump = true;
            doubleJumpTimer = 0.0f;
        }
    }

    /// <summary>
    /// �÷��̾��� ���콺 ����
    /// </summary>
    private void playerAim()
    {
        Vector3 mouseInputPos = Input.mousePosition; //�Էµ� ���콺�������� �޾ƿ�
        mouseInputPos.z = -mainCam.transform.position.z; //���콺������ ���� ����ī�޶��� ������z���� �޾ƿ�
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseInputPos);

        Vector3 mouseDistance = mouseWorldPos - transform.position;

        Vector3 scale = transform.localScale; //�÷��̾��� ��������Ʈ �¿캯���� ���� �������� �޾ƿ�
        scale.x *= -1;

        if (mouseDistance.x > 0 && transform.localScale.x != 1) //���콺 ������ �������̸� ĳ���͵� �������� �ٶ�
        {
            transform.localScale = scale;
            mouseAimRight = true;
        }
        else if (mouseDistance.x < 0 && transform.localScale.x != -1) //���콺 ������ �����̸� ĳ���͵� �������� �ٶ�
        {
            transform.localScale = scale;
            mouseAimRight = false;
        }

        Vector3 mouseAim = Vector3.right; //�׻� ������ �������� ���� ��
        if (mouseAimRight == false) //�������� �ٶ󺸸� ���ӵ� �������� ����
        {
            mouseAim = Vector3.left;
        }

        float angle = Quaternion.FromToRotation(mouseAim, mouseDistance).eulerAngles.z;

        playerHand.rotation = Quaternion.Euler(playerHand.rotation.x, playerHand.rotation.y, angle);
    }

    /// <summary>
    /// �÷��̾ �¿�� �����̴°� ����ϴ� �Լ�
    /// </summary>
    private void playerMove()
    {
        if (isDash == true)
        {
            return;
        }

        leftKey = Input.GetKey(keyManager.PlayerLeftKey()); //Ű �Ŵ������� ���� Ű�� �޾ƿͼ� ��� 
        rightKey = Input.GetKey(keyManager.PlayerRightKey()); //Ű �Ŵ������� ������ Ű�� �޾ƿͼ� ���        

        if (leftKey == true) //���� Ű�� ������ ��� ��������
        {
            moveVec.x = -1 * speed;
        }
        else if (rightKey == true) //������ Ű�� ������ ��� ����������
        {
            moveVec.x = 1 * speed;
        }
        else
        {
            moveVec.x = 0;
        }

        moveVec.y = gravityVelocity; //moveVec�� �߷��� ����
        rigid.velocity = moveVec;
    }

    /// <summary>
    /// �÷��̾��� ������ ����ϴ� �Լ�
    /// </summary>
    private void playerJump()
    {
        if (isDash == true)
        {
            return;
        }

        jumpKey = Input.GetKeyDown(keyManager.PlayerJumpKey());

        if (noAirJump == true && isGround == false)
        {
            isJump = true;
            noAirJump = false;
        }

        if (isGround == false && isJump == true) //isGround�� false��, isJump�� true ���� ������ �ϱ� ���� �ð��� �۵�
        {
            doubleJumpTimer += Time.deltaTime;
        }

        if (jumpKey == true && isGround == true && isJump == false)
        {
            gravityVelocity = jumpPower;
            animIsJump = true;
            isJump = true;
        }
        else if (jumpKey == true && isGround == false && isJump == true && doubleJumpTimer >= doubleJumpTime) //���� ��� �� �ð��� ������ �ѹ� �� ����
        {
            gravityVelocity = jumpPower;
            animIsJump = true;
            isJump = false;
            doubleJumpTimer = 0.0f;
        }
    }

    /// <summary>
    /// �÷��̾��� �߷��� ����ϴ� �Լ�
    /// </summary>
    private void playerGravity()
    {
        if (isDash == true)
        {
            return;
        }

        if (isGround == false)
        {
            gravityVelocity -= gameManager.gravityScale() * Time.deltaTime; //���������� �޴� �߷�         
        }
        else
        {
            gravityVelocity = -1; //isGround�� true�� ��� �߷��� -1�� ����� ���� �ٰ� ����
        }

        playerJump();
    }

    /// <summary>
    /// �÷��̾��� �뽬�� ����ϴ� �Լ�
    /// </summary>
    private void playerDash()
    {
        dashKey = Input.GetKeyDown(keyManager.PlayerDashKey());

        if (isDash == true) //�뽬�� ���ӽð�
        {
            dashRangeTimer += Time.deltaTime;
            if (dashRangeTimer >= dashRange)
            {
                isDash = false;
                dashRangeTimer = 0.0f;
                dashEffect.Clear();
                dashEffect.enabled = false;
            }
        }

        if (dashCoolOn == true) //�뽬�� �ϱ� ���� ��Ÿ��
        {
            dashCoolTimer += Time.deltaTime;
            if (dashCoolTimer >= dashCoolTime)
            {
                dashCoolOn = false;
                dashCoolTimer = 0.0f;
            }
        }

        if (dashKey == true && isDash == false && dashCoolOn == false)
        {
            isDash = true;

            dashCoolOn = true;

            gravityVelocity = 0.0f;

            if (moveVec.x > 0) //moveVec.x �� 0���� ũ�� ���������� �뽬
            {
                moveVec.x = dashPower;
            }
            else if (moveVec.x < 0) //moveVec.x �� 0���� ������ �������� �뽬
            {
                moveVec.x = -dashPower;
            }
            else if (moveVec.x == 0) //moveVec.x �� 0�̸� ���콺 ������ �ִ� ������ �뽬
            {
                moveVec.x = dashPower;
                if (mouseAimRight == false)
                {
                    moveVec.x = -dashPower;
                }
            }

            rigid.velocity = moveVec;

            dashEffect.enabled = true;
        }
    }

    /// <summary>
    /// �÷��̾ ������ �ִ� ������ �ϳ��� ������ ����ϴ� �Լ�
    /// </summary>
    private void playerWeaponChange()
    {
        if (weaponPrefabs == null || weaponPrefabs.Count < 2) //���� �迭�� �ƹ��͵� ���ų�, 2���� ������ ������ ����
        {
            return;
        }

        if (weaponPrefabs.Count > 2) //���� �迭�� ���̰� 2�� �Ѿ�� �� �Ŀ� ����� �迭�� ����
        {
            weaponPrefabs.RemoveAt(2);
        }

        if (weaponsChangeCoolOn == true) //���� ���� Ÿ�̸Ӱ� true�� �۵�
        {
            weaponsChangeCoolTimer += Time.deltaTime;
            if (weaponsChangeCoolTimer >= weaponsChangeCoolTime)
            {
                weaponsChangeCoolOn = false;
                weaponsChangeCoolTimer = 0.0f;
            }
        }

        if (Input.GetKeyDown(keyManager.WeaponChangeKey()) && weaponSwap == false && weaponsChangeCoolOn == false) 
        {
            weaponPrefabs[0].SetActive(true);
            weaponPrefabs[1].SetActive(false);
            weaponSwap = true;
            weaponsChangeCoolOn = true;
        }
        else if (Input.GetKeyDown(keyManager.WeaponChangeKey()) && weaponSwap == true && weaponsChangeCoolOn == false)
        {
            weaponPrefabs[0].SetActive(false);
            weaponPrefabs[1].SetActive(true);
            weaponSwap = false;
            weaponsChangeCoolOn = false;
        }
    }

    /// <summary>
    /// �÷��̾��� �ִϸ��̼��� ����ϴ� �Լ�
    /// </summary>
    private void playerAni()
    {
        anim.SetInteger("isWalk", (int)moveVec.x);
        anim.SetBool("isJump", animIsJump);
        anim.SetBool("isGround", isGround);

        if (animIsJump == true)
        {
            animTimer += Time.deltaTime;
        }

        if (animTimer >= animJumpTime)
        {
            animIsJump = false;
            animTimer = 0.0f;
        }
    }
}
