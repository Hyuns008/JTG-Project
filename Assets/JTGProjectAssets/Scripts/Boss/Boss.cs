using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private BoxCollider2D boxColl2D;
    private RaycastHit2D hit;
    private Rigidbody2D rigid;
    private Vector3 moveVec;
    private Animator anim;
    private SpriteRenderer bossRen;

    private GameManager gameManager;

    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("������ �̵��ӵ�")] private float bossSpeed;
    [SerializeField, Tooltip("������ ����ü��")] private int bossCurHp;
    [SerializeField, Tooltip("������ ���Ϻ� ü��")] private List<int> bossPhaseHp;
    private int curPhase; //���� ������
    private bool phaseChange = false; //������ ������ Ȯ���ϱ� ���� ����
    private float changeTimer; //������ ���� �ð�
    private bool isGround = false; //������ üũ�ϱ� ���� ����
    private float gravity; //�߷°��� �޾ƿ� ����
    private float gravityVelocity; //�߷°��� ����� ������ ����
    private bool isRight = false;
    private bool moveStop = false;

    [Header("���� ���� ����")]
    [SerializeField, Tooltip("������ �÷��̾ �߰��ϱ� ���� �ݶ��̴�")] private BoxCollider2D playerChase;
    [SerializeField, Tooltip("������ �⺻ ������ ���� ����")] private BoxCollider2D AttackCheck;
    [SerializeField, Tooltip("������ ����2 ������ ���� ����")] private BoxCollider2D telpoArea;
    [SerializeField, Tooltip("������ ����3 ������ ���� ����")] private BoxCollider2D darkHandArea;
    [SerializeField, Tooltip("������ �⺻ ���� ����")] private BoxCollider2D bossAttack;
    [SerializeField, Tooltip("������ ����3�� ���� ������")] private GameObject darkHandPrefab;
    private bool isAttack = false; //�÷��̾ �����ߴ��� Ȯ���ϱ� ���� ����
    private bool attackOn = false; //�÷��̾ �ٽ� �����ϱ� ���� ����
    private bool patternAttack = false; //�⺻ ���� �� �������� ���� �ϴ� ����
    private int randomPattern; //�������� ������ �����ų ����
    private float attackDelayTimer; //�⺻ ������ ������ Ÿ�̸�
    private float telpoDelayTimer; //�ڷ���Ʈ�� ������ Ÿ�̸�
    private float darkHandDelayTimer; //��ũ�ڵ��� ������ Ÿ�̸�
    private bool attackDelayOn = false; //�⺻ ���� �����̸� ���ִ� ����
    private bool telpoDelayOn = false; //�ڷ���Ʈ �����̸� ���ִ� ����
    private bool darkHandDelayOn = false; //��ũ�ڵ� �����̸� ���ִ� ����
    [Space]
    [SerializeField, Tooltip("������ �� ��° ������ ��� �� ���� ��� �ð�")] private float bossTelpoCoolTime;
    [SerializeField, Tooltip("������ �� ��° ������ ��� �� ���� ��� �ð�")] private float darkHandCoolTime;
    private bool useTelpo = false; //������ ����ߴ��� �˸��� ���� ����
    private bool useDarkHand = false; //Ÿũ�ڵ带 ����ߴ��� �˸��� ���� ����
    private float bossTelpoCoolTimer; //�ڷ���Ʈ�� ��Ÿ�̸�
    private float darkHandCoolTimer; //��ũ�ڵ��� ��Ÿ�̸�
    private Transform playerTrs; //��ų�� ����ϱ� ���� �÷��̾��� Transform

    private void OnDrawGizmos() //�ڽ�ĳ��Ʈ�� ���信�� ������ Ȯ���� �����ϰ� ������
    {
        if (boxColl2D != null) //�ݶ��̴��� null�� �ƴ϶�� �ڽ����� ������ ���信�� Ȯ���� �� �ְ�
        {
            Gizmos.color = Color.red;
            Vector3 pos = boxColl2D.bounds.center - new Vector3(0, 0.1f, 0);
            Gizmos.DrawWireCube(pos, boxColl2D.bounds.size);
        }
    }

    private void playerCheck(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector3 playerPos = collision.transform.position - transform.position;

            Vector2 scale = transform.localScale;
            scale.x *= -1;
            if (playerPos.x > 2 && transform.localScale.x != -1)
            {
                transform.localScale = scale;
                bossSpeed *= -1;
                isRight = true;
            }
            else if (playerPos.x < -2 && transform.localScale.x != 1)
            {
                transform.localScale = scale;
                bossSpeed *= -1;
                isRight = false;
            }
        }
    }

    /// <summary>
    /// �⺻ ������ ���� �÷��̾ Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <param name="collision"></param>
    private void attackCheck(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isAttack == false && attackOn == false)
            {
                anim.SetTrigger("isPatternA");
                attackDelayOn = true;
                isAttack = true;
                attackOn = true;
                moveStop = true;
            }
        }
    }

    /// <summary>
    /// �ڷ���Ʈ�� ���� �÷��̾ Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <param name="collision"></param>
    private void telpoAreaCheck(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isAttack == false && useTelpo == false &&
                attackOn == false && curPhase >= 1 && randomPattern == 1)
            {
                anim.SetBool("isPatternB_bool", true);
                playerTrs = collision.transform;
                telpoDelayOn = true;
                useTelpo = true;
                isAttack = true;
                attackOn = true;
                moveStop = true;
            }
        }
    }

    /// <summary>
    /// ��ũ�ڵ带 ���� �÷��̾ Ȯ���ϱ� ���� �Լ�
    /// </summary>
    /// <param name="collision"></param>
    private void darkHandCheck(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isAttack == false && useDarkHand == false && attackOn == false 
                && curPhase == 2 && randomPattern == 2)
            {
                anim.SetTrigger("isPatternC");
                darkHandDelayOn = true;
                playerTrs = collision.transform;
                useDarkHand = true;
                isAttack = true;
                attackOn = true;
                moveStop = true;
            }
        }
    }

    /// <summary>
    /// �⺻ ������ �������� �÷��̾ ������ �������� �־��ִ� �Լ�
    /// </summary>
    /// <param name="collision"></param>
    private void bossAttackArea(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (patternAttack == true)
            {
                Player playerSc = collision.gameObject.GetComponent<Player>();
                playerSc.PlayerCurHp(10, true, false);
                patternAttack = false;
                moveStop = false;
            }
        }        
    }

    private void Awake()
    {
        boxColl2D = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();

        bossCurHp = bossPhaseHp[0];

        changeTimer = 0;

        moveVec.x = 1;
    }

    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        bossRen = transform.GetChild(0).GetComponent<SpriteRenderer>();

        gameManager = GameManager.Instance;

        gravity = gameManager.GravityScale();
    }

    private void Update()
    {
        if (gameManager.GetGamePause() == true)
        {
            return;
        }

        playerCollCheck();
        timers();
        bossRightCheck();
        bossGroundCheck();
        bossMove();
        bossGravity();
        bossDeadCheck();
        bossAni();
    }

    /// <summary>
    /// ������ �÷��̾ üũ�Ͽ� ������ �� �ְ� ����� �ϴ� �Լ�
    /// </summary>
    private void playerCollCheck()
    {
        Collider2D playerChaseCheck = Physics2D.OverlapBox(playerChase.bounds.center,
            playerChase.bounds.size, 0, LayerMask.GetMask("Player"));

        Collider2D patternAttackCheck = Physics2D.OverlapBox(AttackCheck.bounds.center,
            AttackCheck.bounds.size, 0, LayerMask.GetMask("Player"));

        Collider2D patternTelpoCheck = Physics2D.OverlapBox(telpoArea.bounds.center,
            telpoArea.bounds.size, 0, LayerMask.GetMask("Player"));

        Collider2D patternDarkHandCheck = Physics2D.OverlapBox(darkHandArea.bounds.center,
            darkHandArea.bounds.size, 0, LayerMask.GetMask("Player"));

        Collider2D patternBossAttack = Physics2D.OverlapBox(bossAttack.bounds.center,
            bossAttack.bounds.size, 0, LayerMask.GetMask("Player"));

        if (playerChaseCheck != null)
        {
            playerCheck(playerChaseCheck);
        }

        if (patternAttackCheck != null)
        {
            attackCheck(patternAttackCheck);
        }

        if (patternTelpoCheck != null)
        {
            telpoAreaCheck(patternTelpoCheck);
        }

        if (patternDarkHandCheck != null)
        {
            darkHandCheck(patternDarkHandCheck);
        }

        if (patternBossAttack != null)
        {
            bossAttackArea(patternBossAttack);
        }
    }

    /// <summary>
    /// ������ ���õ� Ÿ�̸� ����
    /// </summary>
    private void timers()
    {
        if (phaseChange == true)
        {
            if (curPhase == 2)
            {
                ++curPhase;
            }

            changeTimer += Time.deltaTime;
            if (changeTimer > 2)
            {
                ++curPhase;
                patternHpChange();
                changeTimer = 0;
                phaseChange = false;
            }
        }

        if (isAttack == true) //������ �ϸ� �ٷ� ������ �� �� ���� ����� �ڵ�
        {
            attackDelayTimer += Time.deltaTime;
            if (attackDelayTimer > 3)
            {
                attackDelayTimer = 0;
                if (curPhase == 1) //2������� ����Ǵ� �ڵ�
                {
                    if (useTelpo == false)
                    {
                        randomPattern = 1;
                    }
                }
                else if (curPhase == 2) //3������� ����Ǵ� �ڵ�
                {
                    if (useTelpo == false && useDarkHand == false)
                    {
                        int randomPat = Random.Range(1, 3);
                        randomPattern = randomPat;
                    }
                    else if (useTelpo == true && useDarkHand == false)
                    {
                        randomPattern = 2;
                    }
                    else if (useTelpo == false && useDarkHand == true)
                    {
                        randomPattern = 1;
                    }
                }
                isAttack = false;
            }
        }

        if (attackDelayOn == true) //������ �⺻ ���� ������
        {
            attackDelayTimer += Time.deltaTime;
            if (attackDelayTimer > 1f)
            {
                attackDelayTimer = 0;
                attackDelayOn = false;
                patternAttack = true;
                attackOn = false;
            }
        }

        if (telpoDelayOn == true) //������ �ڷ���Ʈ ������
        {
            telpoDelayTimer += Time.deltaTime;
            if (telpoDelayTimer > 0.8f)
            {
                transform.position = playerTrs.position + new Vector3(0, 1, 0);
                anim.SetBool("isPatternB_bool", false);
                anim.SetTrigger("isPatternB_trigger");
                telpoDelayTimer = 0;
                telpoDelayOn = false;
                attackOn = false;
                moveStop = false;
            }
        }

        if (darkHandDelayOn == true) //������ ��ũ�ڵ� ���� ������
        {
            darkHandDelayTimer += Time.deltaTime;
            if (darkHandDelayTimer > 0.8f)
            {
                Instantiate(darkHandPrefab, playerTrs.position + new Vector3(0, 2.5f, 0), Quaternion.identity, transform);
                darkHandDelayTimer = 0;
                darkHandDelayOn = false;
                attackOn = false;
                moveStop = false;
            }
        }

        if (useTelpo == true) //������ �ڷ���Ʈ ��Ÿ��
        {
            bossTelpoCoolTimer += Time.deltaTime;
            if (bossTelpoCoolTimer >= bossTelpoCoolTime)
            {
                bossTelpoCoolTimer = 0;
                useTelpo = false;
            }
        }

        if (useDarkHand == true) //������ ��ũ�ڵ� ������ ��Ÿ��
        {
            darkHandCoolTimer += Time.deltaTime;
            if (darkHandCoolTimer >= darkHandCoolTime)
            {
                darkHandCoolTimer = 0;
                useDarkHand = false;
            }
        }
    }

    private void bossRightCheck()
    {
        if (isRight == true)
        {

        }
    }

    /// <summary>
    /// ������ ���� üũ�� �� �ְ� ����ϴ� �Լ�
    /// </summary>
    private void bossGroundCheck()
    {
        isGround = false;

        hit = Physics2D.BoxCast(boxColl2D.bounds.center, boxColl2D.bounds.size, 0,
            Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        if (hit.transform != null && hit.transform.gameObject.layer
            == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }
    }

    /// <summary>
    /// ������ �⺻ ������
    /// </summary>
    private void bossMove()
    {
        if (moveStop == true)
        {
            moveVec.x = 0;
            return;
        }

        moveVec.x = -bossSpeed;
        rigid.velocity = moveVec;
    }

    /// <summary>
    /// ������ �߷�
    /// </summary>
    private void bossGravity()
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
    }

    /// <summary>
    /// ������ ������ ����� �� ü�µ� ����
    /// </summary>
    private void patternHpChange()
    {
        if (curPhase > 2)
        {
            return;
        }

        if (curPhase == 1)
        {
            bossRen.color = Color.magenta;
            bossCurHp = bossPhaseHp[1];
        }
        else if (curPhase == 2)
        {
            bossRen.color = Color.red;
            bossCurHp = bossPhaseHp[2];
        }
    }

    /// <summary>
    /// ������ ��� ����� ��� �� �׾��� �� ����
    /// </summary>
    private void bossDeadCheck()
    {
        if (bossCurHp <= 0 && curPhase > 2)
        {
            Destroy(gameObject);
        }
        else if (bossCurHp <= 0)
        {
            phaseChange = true;

            //Color curColor = bossRen.color; ���İ� ���� ���
            //curColor.a = 0.5f;
            //bossRen.color = curColor;
        }
    }

    /// <summary>
    /// ������ �⺻���� �ִϸ��̼��� ����ϴ� �Լ�
    /// </summary>
    private void bossAni()
    {
        anim.SetInteger("isWalk", (int)moveVec.x);
    }
}
