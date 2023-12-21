using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid; //�÷��̾��� ������ٵ�
    private RaycastHit2D hit2D;
    private BoxCollider2D playerBoxColl;

    //�÷��̾ ������ �Ŵ���
    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    [Header("�̵�")]
    [SerializeField] private float speed = 1.0f; //�÷��̾��� �̵��ӵ�
    private Vector2 moveVec; //�÷��̾��� �������� ���� ����
    private bool leftKey; //���� Ű�� ������ ��
    private bool rightKey; //������ Ű�� ������ ��

    private bool isGround = false; //�÷��̾ ���� �پ����� ������ false
    private float gravityVelocity;

    [Header("����")]
    [SerializeField] private float jumpPower = 1.0f; //������ �ϱ� ���� ��
    private bool isJump = false; //������ �ߴ���
    private bool jumpKey; //���� Ű�� ������ ��

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); //�÷��̾� �ڽ��� ������ٵ� �޾ƿ�
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        keyManager = KeyManager.instance;
    }

    private void Update()
    {
        if (gameManager.GamePause() == true)
        {
            return;
        }
        playerCheckGround();
        playerMove();
        playerJump();
        playerGravity();
    }

    /// <summary>
    /// �÷��̾ ������ �ƴ��� Ȯ���� ����ϴ� �Լ�
    /// </summary>
    private void playerCheckGround()
    {
        isGround = false;

        if (gravityVelocity > 0)
        {
            return;
        }
    }

    /// <summary>
    /// �÷��̾ �¿�� �����̴°� ����ϴ� �Լ�
    /// </summary>
    private void playerMove()
    {
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

        moveVec.y = gravityVelocity;
        rigid.velocity = moveVec;
    }

    /// <summary>
    /// �÷��̾��� ������ ����ϴ� �Լ�
    /// </summary>
    private void playerJump()
    {
        jumpKey = Input.GetKeyDown(keyManager.PlayerJumpKey());

        if (jumpKey == true)
        {
            gravityVelocity = jumpPower;
            isJump = true;
        }
    }

    /// <summary>
    /// �÷��̾��� �߷��� ����ϴ� �Լ�
    /// </summary>
    private void playerGravity()
    {
        gravityVelocity -= gameManager.gravityScale() * Time.deltaTime; //���������� �޴� �߷�

        if (gravityVelocity < gameManager.gravityScale()) //�������� �ӵ��� gravityScale���� �۾����� gravityScale�� ����
        {
            gravityVelocity = -gameManager.gravityScale();
        }
    }
}
