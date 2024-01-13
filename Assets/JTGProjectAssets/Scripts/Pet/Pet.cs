using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public enum PetType
    {
        petTypeA = 1,
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

    private TrashPreFab trashPreFab;

    private float gravity; //���� �߷�
    private bool isGround = false; //������ �ƴ��� Ȯ���� ���� ����
    private float gravityVelocity; //������ٵ��� Y���� �����ϱ� ���� ����

    private Player playerSc; //�÷��̾� ��ũ��Ʈ
    private Transform playerTrs; //�÷��̾��� Ʈ������
    private Vector3 playerPos; //�÷��̾��� ������
    private float followPosX; //���� ���� x������
    private float followPosY; //���� Ȯ���� Y������
    private Enemy enemySc;

    [SerializeField] private GameObject pickUpKeyImage;

    [Header("���� �⺻ ����")]
    [SerializeField, Tooltip("���� �̵��ӵ�")] private float speed;
    [SerializeField, Tooltip("�÷��̾ ���� ������� üũ���ִ� ����")] private bool getPet = false;
    private bool playerIn = false; //�÷��̾ �ڽ��� �����ȿ� �ִ��� üũ
    private bool petRun = false; //���� �޸����� �� �޸������� üũ
    private bool isIdle = false; //���� ������ �ִ����� üũ
    private float motionTimer; //����� ����ϱ� ���� Ÿ�̸�
    private bool motionOn = false; //����� ����Ǿ������� Ȯ���� ����
    private bool moveOff = false; //����� ����Ǿ��� �� �������� ������ ����
    private float moveOnTimer; //�ٽ� �����̰� ������ִ� Ÿ�̸�
    private bool runStop = false; //�޸��� �ִϸ��̼��� �������� ����
    private float runStopTimer; //�޸��� ����� ���߰� �ϴ� Ÿ�̸�

    [Header("���� �ɷ�ġ ���� �нú�")]
    [SerializeField, Tooltip("�нú� ȿ�� ���ݷ� ����")] private int petDamageEffect;
    [Space]
    [SerializeField, Tooltip("�нú� ȿ�� ���� ����")] private int petArmorEffect;
    [Space]
    [SerializeField, Tooltip("�нú� ȿ�� ü�� ����")] private int petHpEffect;
    [Space]
    [SerializeField, Tooltip("�нú� ȿ�� ġ��ŸȮ�� ����")] private float petCriPcentEffect;
    [SerializeField, Tooltip("�нú� ȿ�� ġ��Ÿ������ ����")] private float petCriDamageEffect;
    private bool petPassiveOn = false;

    [Header("���� �ɷ�")]
    [SerializeField, Tooltip("���� ���ݷ�")] private float petDamage;
    [SerializeField, Tooltip("���� ���� ������")] private GameObject petAttackpreFab;
    [SerializeField, Tooltip("���� ���ݽ� �꿡�� �����Ǵ� ����Ʈ")] private GameObject petSkillEffect;
    [SerializeField, Tooltip("���� ��ų��Ÿ��")] private float petSkillTime;
    [SerializeField, Tooltip("���� ���ݽ� �������� ���� ������")] private float attackDelayTime;
    private bool uesPetSkill = false;
    private float petSkillTimer;
    private float attackDelayTimer;
    private bool isAttack;
    private GameObject petSkilAttacklObj;
    private GameObject petSkillEfObj;

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
            playerPos = _collision.gameObject.transform.position;
            followPosX = playerPos.x - transform.position.x;
            followPosY = playerPos.y - transform.position.y;

            Vector3 scale = transform.localScale;
            scale.x *= -1;

            if (transform.localScale.x != 1 && followPosX > 0 && moveOff == false && motionOn == false)
            {
                transform.localScale = scale;
            }
            else if (transform.localScale.x != -1 && followPosX < 0 && moveOff == false && motionOn == false)
            {
                transform.localScale = scale;
            }
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (petType.ToString() == "petTypeA")
            {
                enemySc = _collision.gameObject.GetComponent<Enemy>();
                if (uesPetSkill == false)
                {
                    petSkilAttacklObj = Instantiate(petAttackpreFab, _collision.gameObject.transform.position, Quaternion.identity, trashPreFab.transform);
                    petSkilAttacklObj.transform.SetParent(_collision.gameObject.transform);

                    Vector3 petPos = transform.position;
                    petPos.y += 0.8f;

                    petSkillEfObj = Instantiate(petSkillEffect, petPos, Quaternion.identity, trashPreFab.transform);
                    petSkillEfObj.transform.SetParent(gameObject.transform);
                    uesPetSkill = true;
                    isAttack = true;
                }
            }
            else if (petType.ToString() == "petTypeB")
            {

            }
            else if (petType.ToString() == "petTypeC")
            {

            }
            else if (petType.ToString() == "petTypeD")
            {

            }
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        petBoxColl2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        motionTimer = 0;

        petSkillTimer = petSkillTime;

        attackDelayTimer = attackDelayTime;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        trashPreFab = TrashPreFab.instance;

        gravity = gameManager.GravityScale();
    }

    private void Update()
    {
        if (gameManager.GetGamePause() == true)
        {
            return;
        }

        petTimer();
        colliderCheck();
        petGroundCheck();
        petPos();
        petMove();
        petGravity();
        petPassiveEffect();
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
                moveVec.x = 0;
            }
        }
        else if (isIdle == false && motionOn == true)
        {
            motionOn = false;
            moveOff = true;
        }

        if (moveOff == true)
        {
            moveOnTimer += Time.deltaTime;
            if (moveOnTimer > 1)
            {
                moveOff = false;
                moveOnTimer = 0;
            }
        }

        if (runStop == true)
        {
            runStopTimer += Time.deltaTime;
            if (runStopTimer > 0.3f)
            {
                petRun = false;
            }
        }

        if (uesPetSkill == true)
        {
            petSkillTimer -= Time.deltaTime;
            if (petSkillTimer < 0)
            {
                uesPetSkill = false;
                petSkillTimer = petSkillTime;
            }
        }

        if (isAttack == true)
        {
            attackDelayTimer -= Time.deltaTime;
            if (attackDelayTimer < 0)
            {
                enemySc.EnemyHp((int)petDamage, true, true, false);
                Destroy(petSkillEfObj);
                attackDelayTimer = attackDelayTime;
                isAttack = false;
            }
        }
    }

    private void colliderCheck()
    {
        if (getPet == false)
        {
            pickUpKeyImage.SetActive(true);
            return;
        }
        else if (getPet == true)
        {
            pickUpKeyImage.SetActive(false);
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
            Vector3 telposPos = playerTrs.position;
            telposPos.x = playerTrs.position.x - 1f;
            transform.position = telposPos;
            gravityVelocity = 0;
        }
    }

    private void petMove()
    {
        if (getPet == false)
        {
            return;
        }

        if (moveOff == true)
        {
            moveVec.x = 0;
            return;
        }

        if (playerSc != null)
        {
            if (followPosX > 1)
            {
                isIdle = false;
                motionTimer = 0;
                moveVec.x = 1;
            }
            else if (followPosX < -1)
            {
                isIdle = false;
                motionTimer = 0;
                moveVec.x = -1;
            }
            else if (followPosX <= 1 || followPosX >= -1)
            {
                isIdle = true;
                moveVec.x = 0;
                moveOnTimer = 0;
            }

            if (followPosX > 3)
            {
                speed = 8;
                petRun = true;
                runStop = false;
                runStopTimer = 0;
            }
            else if (followPosX < -3)
            {
                speed = 8;
                petRun = true;
                runStop = false;
                runStopTimer = 0;
            }
            else if (followPosX <= 3 || followPosX >= -3)
            {
                speed = 2;
                runStop = true;
            }

            moveVec.x *= speed;
            rigid.velocity = moveVec;
        }
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
    }

    private void petPassiveEffect()
    {
        if (playerSc != null)
        {
            if (petPassiveOn == false)
            {
                if (petType.ToString() == "petTypeA")
                {
                    playerSc.PlayerStatusHp(petHpEffect);                 
                    petPassiveOn = true;
                }
                else if (petType.ToString() == "petTypeB")
                {
                    playerSc.PlayerStatusArmor(petArmorEffect);
                    petPassiveOn = true;
                }
                else if (petType.ToString() == "petTypeC")
                {
                    playerSc.PlayerStatusDamage(petDamageEffect);
                    petPassiveOn = true;
                }
                else if (petType.ToString() == "petTypeD")
                {
                    playerSc.PlayerStatusCritical(petCriPcentEffect, petCriDamageEffect);
                    petPassiveOn = true;
                }
            }            
        }
    }

    private void petAni()
    {
        if (followPosY > 1)
        {
            isIdle = false;
            motionTimer = 0;
        }
        else if (followPosY < -1)
        {
            isIdle = false;
            motionTimer = 0;
        }

        anim.SetInteger("isWalk", (int)moveVec.x);
        anim.SetBool("isSleep", motionOn);
        anim.SetBool("isRun", petRun);
    }

    /// <summary>
    /// ���� ������� üũ�ϱ� ���� �Լ�
    /// </summary>
    /// <param name="_get"></param>
    public void GetPetCheck(bool _get)
    {
        getPet = _get;
    }

    /// <summary>
    /// ���� ������ ���ϱ� ���� Ÿ���� �÷��̾�� ������ �Լ�
    /// </summary> 
    /// <returns></returns>
    public PetType GetPetType()
    {
        return petType;
    }

    public void SetPetPassiveOn(bool _on)
    {
        petPassiveOn = _on;
    }
}
