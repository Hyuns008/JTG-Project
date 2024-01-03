using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public enum PetType
    {
        petTypeA,
        petTypeB,
        petTypeC,
        petTypeD,
    }

    [Header("���� �÷��� �÷��̾� �ɷ�ġ Ÿ��")]
    [SerializeField] private PetType petType;

    [Header("Ư�� ������Ʈ�� üũ�ϱ� ���� �ݶ��̴�")]
    [SerializeField] private CircleCollider2D playerCheck;
    [SerializeField] private CircleCollider2D enemyCheck;

    private Rigidbody2D rigid;
    private BoxCollider2D petBoxColl2D;
    private RaycastHit2D hit;
    private Vector3 moveVec;
    private Animator anim;

    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    private TrashPreFab trashPreFab;

    private float gravity;
    private bool isGround = false;
    private float gravityVelocity;

    private Player playerSc;
    private Transform playerTrs;

    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("���� �̵��ӵ�")] private float speed;
    [SerializeField, Tooltip("�÷��̾ ���� ������� üũ���ִ� ����")] private bool getPet = false;
    private bool playerIn = false;
    private int walkValue;
    private bool petRun = false;
    private bool isIdle = false;
    private float motionTimer;
    private bool motionOn = false;

    [Header("���� �ɷ°� ȿ��")]
    [SerializeField] private float petDamageA;
    [SerializeField] private GameObject petEffect;

    private void OnDrawGizmos() //�ڽ�ĳ��Ʈ�� ���信�� ������ Ȯ���� �����ϰ� ������
    {
        if (petBoxColl2D != null) //�ݶ��̴��� null�� �ƴ϶�� �ڽ����� ������ ���信�� Ȯ���� �� �ְ�
        {
            Gizmos.color = Color.red;
            Vector3 pos = petBoxColl2D.bounds.center - new Vector3(0, 0.1f, 0);
            Gizmos.DrawWireCube(pos, petBoxColl2D.bounds.size);
        }
    }

    private void checkTrigger(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerIn = true;
            playerSc = _collision.gameObject.GetComponent<Player>();
            playerTrs = _collision.gameObject.transform;

            

            Vector3 scale = transform.localScale;
            scale.x *= -1;

            if (transform.localScale.x != 1 && playerSc.playerMouseAimRight() == true && motionOn == false)
            {
                transform.localScale = scale;
            }
            else if (transform.localScale.x != -1 && playerSc.playerMouseAimRight() == false && motionOn == false)
            {
                transform.localScale = scale;
            }
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameObject petSkillObj = Instantiate(petEffect, _collision.gameObject.transform.position, Quaternion.identity, trashPreFab.transform);
            petSkillObj.transform.SetParent(_collision.gameObject.transform);
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        petBoxColl2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        motionTimer = 0;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        keyManager = KeyManager.instance;

        trashPreFab = TrashPreFab.instance;

        gravity = gameManager.GravityScale();
    }

    private void Update()
    {
        petTimer();
        colliderCheck();
        petGroundCheck();
        petPos();
        petMove();
        petGravity();
        petAni();
    }

    private void petTimer()
    {
        if (isIdle == true)
        {
            motionTimer += Time.deltaTime;
            if (motionTimer >= 6)
            {
                motionOn = true;
            }
        }
    }

    private void colliderCheck()
    {
        if (getPet == false)
        {
            return;
        }
        playerIn = false;

        Collider2D playerColl = Physics2D.OverlapCircle(playerCheck.bounds.center,
        playerCheck.radius, LayerMask.GetMask("Player"));

        Collider2D enemyColl = Physics2D.OverlapCircle(enemyCheck.bounds.center,
        enemyCheck.radius, LayerMask.GetMask("Enemy"));

        if (playerColl != null)
        {
            checkTrigger(playerColl);
        }

        if (enemyColl != null)
        {
            checkTrigger(enemyColl);
        }
    }

    private void petGroundCheck()
    {
        isGround = false;

        if (gravityVelocity > 0) //gravityVelocity���� 0���� Ŭ ��� �Ʒ� �ڵ尡 ������ ����
        {
            return;
        }

        hit = Physics2D.BoxCast(petBoxColl2D.bounds.center, petBoxColl2D.bounds.size,
            0, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }
    }

    private void petPos()
    {
        if (getPet == false)
        {
            return;
        }

        if (playerIn == false)
        {
            Vector3 playerPos = playerTrs.position;
            playerPos.x = playerTrs.position.x - 0.5f;
            transform.position = playerPos;
            gravityVelocity = 0;
        }
    }

    private void petMove()
    {
        if (getPet == false)
        {
            return;
        }

        if (Input.GetKey(keyManager.PlayerLeftKey()) == true) //���� Ű�� ������ ��� ��������
        {
            moveVec.x = -1;
            isIdle = false;
            motionTimer = 0;
            motionOn = false;
        }
        else if (Input.GetKey(keyManager.PlayerRightKey()) == true) //������ Ű�� ������ ��� ����������
        {
            moveVec.x = 1;
            isIdle = false;
            motionTimer = 0;
            motionOn = false;
        }
        else
        {
            moveVec.x = 0;
            isIdle = true;
        }

        moveVec.x *= speed;
        rigid.velocity = moveVec;
    }

    private void petGravity()
    {
        if (isGround == false)
        {
            gravityVelocity -= gravity * Time.deltaTime;
        }
        else if (isGround == true)
        {
            gravityVelocity = -1;
        }

        moveVec.y = gravityVelocity;
        rigid.velocity = moveVec;

        if (playerSc != null)
        {
            moveVec.y = playerSc.GetGravityVelocity() - 1;
            rigid.velocity = moveVec;
        }
    }

    private void petAni()
    {
        anim.SetInteger("isWalk", walkValue);
        anim.SetBool("isSleep", motionOn);

        if (petRun == true)
        {
            anim.SetTrigger("isRun");
            petRun = false;
        }
    }

    public void PetWalkValue(float _speed)
    {
        speed = _speed;
    }

    public void PetWalkAniValue(int _value)
    {
        walkValue = _value;
    }

    public void GetPetCheck(bool _get)
    {
        getPet = _get;
    }

    public void PetRunCheck(bool _run)
    {
        petRun = _run;
    }
}
