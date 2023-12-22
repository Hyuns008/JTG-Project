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
    [SerializeField] private float speed = 1.0f; //�÷��̾��� �̵��ӵ�
    private Vector2 moveVec; //�÷��̾��� �������� ���� ����
    private bool leftKey; //���� Ű�� ������ ��
    private bool rightKey; //������ Ű�� ������ ��

    private bool isGround = false; //�÷��̾ ���� �پ����� ������ false
    private float gravityVelocity;  //�߷°� ���õ� ���� �޾ƿ��� ���� ����

    [Header("����")]
    [SerializeField] private float jumpPower = 1.0f; //������ �ϱ� ���� ��
    private bool isJump = false; //������ �ߴ���
    private bool jumpKey; //���� Ű�� ������ ��
    private bool animIsJump = false;
    [SerializeField] private float animJumpTime = 1.0f;
    private float animTimer = 0.0f;

    private void OnDrawGizmos() //�ڽ�ĳ��Ʈ�� ���信�� ������ Ȯ���� �����ϰ� ������
    {
        if (playerBoxColl2D != null)
        {
            Gizmos.color = Color.red;
            Vector3 pos = playerBoxColl2D.bounds.center - new Vector3(0, 0.1f, 0);
            Gizmos.DrawWireCube(pos, playerBoxColl2D.bounds.size);
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); //�÷��̾� �ڽ��� ������ٵ� ������
        playerBoxColl2D = GetComponent<BoxCollider2D>(); //�÷��̾� �ڽ��� �ڽ��ݶ��̴�2D�� ������
        anim = GetComponent<Animator>(); //�÷��̾��� �ִϸ��̼��� ������
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        keyManager = KeyManager.instance;

        mainCam = Camera.main;
    }

    private void Update()
    {
        if (gameManager.GamePause() == true)
        {
            return;
        }

        playerCheckGround();
        playrAim();
        playerMove();
        playerGravity();
        playerAni();
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

        hit2D = Physics2D.BoxCast(playerBoxColl2D.bounds.center, playerBoxColl2D.bounds.size,
            0.0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        if (hit2D.transform != null && hit2D.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
            isJump = false;
        }
    }

    /// <summary>
    /// �÷��̾��� ���콺 ����
    /// </summary>
    private void playrAim()
    {
        Vector3 mouseInputPos = Input.mousePosition;
        mouseInputPos.z = -mainCam.transform.position.z;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseInputPos);

        Vector3 mouseDistance = mouseWorldPos - transform.position;

        Vector3 scale = transform.localScale;
        scale.x *= -1;

        if (mouseDistance.x > 0 && transform.localScale.x != 1)
        {
            transform.localScale = scale;
        }
        else if (mouseDistance.x < 0 && transform.localScale.x != -1)
        {
            transform.localScale = scale;
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

        if (jumpKey == true && isGround == true)
        {
            gravityVelocity = jumpPower;
            animIsJump = true;
            //isJump = true;
        }
    }

    /// <summary>
    /// �÷��̾��� �߷��� ����ϴ� �Լ�
    /// </summary>
    private void playerGravity()
    {
        if (isGround == false)
        {
            gravityVelocity -= gameManager.gravityScale() * Time.deltaTime; //���������� �޴� �߷�         
        }
        else
        {
            gravityVelocity = -1;
        }

        playerJump();
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
