using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SaveObject;

public class Player : MonoBehaviour
{
    public class PlayerData
    {
        public float playerDamage;
        public int playerArmor;
        public int playerHp;
        public int playerCurHp;
        public float playerCritical;
        public float playerCriDamage;
    }

    private PlayerData playerData = new PlayerData();
    private SaveObject saveObject;
    private bool dataSave = false;

    public enum PlayerSkillType
    {
        skillTypeA,
        skillTypeB,
        skillTypeC,
        skillTypeD,
    }

    [SerializeField] private PlayerSkillType skillType;

    private Rigidbody2D rigid; //�÷��̾��� ������ٵ�
    private RaycastHit2D hit2D;
    private BoxCollider2D playerBoxColl2D; //�÷��̾��� �ڽ� �ݶ��̴�
    private Camera mainCam; //���� ī�޶�
    private Animator anim;
    private SpriteRenderer playerRen;

    private PlayerUI playerUI;

    //�÷��̾ ������ �Ŵ���
    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    [SerializeField] private float gravity; //���ӸŴ������� ������ �߷°��� ������ ����

    [Header("ĳ������ �⺻ ����")]
    [SerializeField] private float playerDamage;
    [SerializeField] private float playerBuffDamageUp;
    [SerializeField, Range(0.0f, 100.0f)] private float playerCritical;
    [SerializeField, Range(2.0f, 3.5f)] private float playerCriDamage;
    private bool buffDamageUp = false;
    private float buffDuration = 0.0f;
    [SerializeField] private int playerMaxHealth = 100;
    [SerializeField] private int playerCurHealth = 0;
    [SerializeField] private int playerArmor = 0;
    private bool playerHitDamage = false;
    private float hitDamageTimer;

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
    private bool dashInvincibleOn = false; //�뽬�� ����� �� ������ ���� ����

    [Header("�� Ÿ�� �� �� �����̵�")]
    [SerializeField, Tooltip("�� ������ ���� ��")] private float wallJumpPower = 0.5f; //�� ������ ���� ��
    [SerializeField, Tooltip("�� ���� �� ���߿� �ִ� �ð�")] private float wallJumpSky = 0.3f;
    [SerializeField, Tooltip("�� �����̵� �ӵ�")] private float wallSlidingSpeed = 5.0f;
    private bool isWall = false; //���� ��Ҵ��� Ȯ���� ��
    private bool wallJumpTimerOn = false;
    private float wallJumpTimer = 0.0f; //�� ����
    private bool useWallSliding = false; //�� �����̵��� �ϱ� ���� ���ǽ�


    [Header("���� ���� ����")]
    [SerializeField, Tooltip("���� ���� ������")] private float weaponsChangeCoolTime = 1.0f; //���� ������ ��� �ð�
    private float weaponsChangeCoolTimer = 0.0f; //���� ������ ���� Ÿ�̸�
    private bool weaponsChangeCoolOn = false; //���Ⱑ ������ �����ϸ� false
    [SerializeField] private List<GameObject> weaponPrefabs = new List<GameObject>(); //���⸦ ���� �κ��丮 ����
    private GameObject getWeapon; //�ݶ��̴��� ���� ������Ʈ�� ��� �� ����
    private bool weaponSwap = false; //���� ������ ����ϴ� ����
    private bool weaponDrop = false;
    private float weaponDropTime = 0.0f;
    private Vector3 weaponLocalScale;

    [Header("�� ���� ����")]
    [SerializeField, Tooltip("���� ������ ����")] private List<GameObject> petPrefabs = new List<GameObject>();
    private Pet petSc; //���� ��ũ��Ʈ ������ ����

    private bool optionOn = false; //�÷��̾ �ɼ��� �״��� �� �״��� Ȯ���ϱ� ���� ����

    private void OnDrawGizmos() //�ڽ�ĳ��Ʈ�� ���信�� ������ Ȯ���� �����ϰ� ������
    {
        if (playerBoxColl2D != null) //�ݶ��̴��� null�� �ƴ϶�� �ڽ����� ������ ���信�� Ȯ���� �� �ְ�
        {
            Gizmos.color = Color.red;
            Vector3 pos = playerBoxColl2D.bounds.center - new Vector3(0, 0.1f, 0);
            Gizmos.DrawWireCube(pos, playerBoxColl2D.bounds.size);
        }
    }

    private void pickUpItem(Collider2D _collision) //�������� �ݱ� ���� �Լ�
    {
        if (_collision.gameObject.tag == "Item" || _collision.gameObject.tag == "Weapon") //�÷��̾ ���� ������Ʈ �±װ� ������ �Ǵ� �������� üũ
        {
            ItemPickUp itemPickUpSc = _collision.gameObject.GetComponent<ItemPickUp>();  //�÷��̾ ���� ������Ʈ�� ������ ��ũ��Ʈ�� ������
            ItemPickUp.ItemType itemPickUpType = itemPickUpSc.GetItemType();

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (itemPickUpType == ItemPickUp.ItemType.Weapon) //������ ������ Ÿ�԰� ��ũ��Ʈ�� ������ Ÿ���� ��ġ�ϸ� �۵�
                {
                    if (weaponPrefabs.Count > 1)
                    {
                        return;
                    }

                    int count = weaponPrefabs.Count;

                    if (weaponPrefabs.Count > 0) //���� ī��Ʈ�� 0���� ũ�� �ڽ��� ������ ������ ������Ʈ�� ��Ȱ��ȭ ���� ��
                    {
                        for (int i = 0; i < count; i++)
                        {
                            SpriteRenderer weaponRen = weaponPrefabs[i].GetComponent<SpriteRenderer>();
                            weaponRen.enabled = false;
                            Weapons weaponSc = weaponPrefabs[i].GetComponent<Weapons>();
                            weaponSc.ShootingOn(false);
                        }
                    }

                    getWeapon = _collision.gameObject;
                    getWeapon.transform.SetParent(playerHand); //�ڽ� ������Ʈ�� �ִ� �ڵ�
                    getWeapon.transform.position = playerHand.transform.position;
                    getWeapon.transform.rotation = playerHand.transform.rotation;
                    weaponLocalScale = getWeapon.transform.localScale;
                    if (getWeapon.transform.localScale.x < 0)
                    {
                        weaponLocalScale.x *= -1;
                        getWeapon.transform.localScale = weaponLocalScale;
                    }

                    getWeapon.GetComponent<BoxCollider2D>().enabled = false;

                    Weapons getWeaponSc = getWeapon.GetComponent<Weapons>();
                    getWeaponSc.ShootingOn(true);
                    getWeaponSc.PickUpImageOff(true);
                    getWeaponSc.WeaponGravityOff(true);
                    getWeaponSc.WeaponUseShooting(false);
                    SpriteRenderer getWeaponRen = getWeapon.GetComponent<SpriteRenderer>();
                    getWeaponRen.sortingOrder = 2;

                    weaponPrefabs.Add(getWeapon); //���⸦ �κ��丮 ������ �ϴ� �迭�� �߰���
                }
                else if (itemPickUpType == ItemPickUp.ItemType.Pet)
                {
                    if (petPrefabs.Count > 0)
                    {
                        return;
                    }

                    petSc = _collision.gameObject.GetComponent<Pet>();
                    petSc.GetPetCheck(true);
                    petPrefabs.Add(_collision.gameObject);
                }
            }
            else if (itemPickUpType == ItemPickUp.ItemType.Buff)
            {
                GameObject buffObj = _collision.gameObject;
                Buff buffSc = buffObj.GetComponent<Buff>();
                Buff.BuffType buffType = buffSc.GetBuffType();
                if (buffType == Buff.BuffType.damage)
                {
                    playerBuffDamageUp = buffSc.BuffTypeValue();
                    buffDuration = 10;
                    buffDamageUp = true;
                }
                else if (buffType == Buff.BuffType.armor)
                {
                    playerArmor = (int)buffSc.BuffTypeValue();
                }
                else if (buffType == Buff.BuffType.heal)
                {
                    playerCurHealth += (int)buffSc.BuffTypeValue();
                }
                DestroyImmediate(_collision.gameObject);
            }
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); //�÷��̾� �ڽ��� ������ٵ� ������
        playerBoxColl2D = GetComponent<BoxCollider2D>(); //�÷��̾� �ڽ��� �ڽ��ݶ��̴�2D�� ������
        anim = GetComponent<Animator>(); //�÷��̾��� �ִϸ��̼��� ������
        playerHand = transform.Find("PlayerHand");
        playerRen = GetComponent<SpriteRenderer>();
        playerUI = GetComponent<PlayerUI>();
        saveObject = GetComponent<SaveObject>();

        playerCurHealth = playerMaxHealth;

        dashCoolTimer = dashCoolTime;

        weaponDropTime = 0.5f;

        hitDamageTimer = 0.1f;

        mouseAimRight = true;
    }

    private void Start()
    {
        gameManager = GameManager.Instance; //���� �Ŵ����� ������ gameManager�� ��� ��
        keyManager = KeyManager.instance; //Ű�Ŵ����� ������ keyManager ��� ��

        mainCam = Camera.main; //���� ī�޶� ������ mainCam�� ��� ��

        gravity = gameManager.GravityScale();

        playerUI.OptionOn(false);

        playerUI.SetPlayerHp(playerCurHealth, playerMaxHealth, "");

        saveObject.PlayerObjectDataLoad();
    }

    private void Update()
    {
        if (playerCurHealth > playerMaxHealth)
        {
            playerCurHealth = playerMaxHealth;
        }

        setPlayerData();
        timers();
        itmeColliderCheck();
        playerCheckGround();
        playerAim();
        playerMove();
        playerGravity();
        playerDash();
        playerWallSkill();
        playerWeaponChange();
        playerWeaponDrop();
        playerBuffDamage();
        playerDamageHit();
        playerDead();
        playerWeaponCritical();
        petPassiveBuff();
        playerOption();
        playerAni();
    }

    /// <summary>
    /// �÷��̾��� �����͸� ���̺��ϱ� ���� �Լ�
    /// </summary>
    private void setPlayerData()
    {
        if (dataSave == true)
        {
            playerData.playerDamage = playerDamage;
            playerData.playerArmor = playerArmor;
            playerData.playerHp = playerMaxHealth;
            playerData.playerCurHp = playerCurHealth;
            playerData.playerCritical = playerCritical;
            playerData.playerCriDamage = playerCriDamage;

            saveObject.PlayerObjectSaveData(playerData);

            dataSave = false;
        }
    }

    /// <summary>
    /// �Լ����� ����� Ÿ�̸� ����
    /// </summary>
    private void timers()
    {
        if (isGround == false && isJump == true) //isGround�� false��, isJump�� true ���� ������ �ϱ� ���� �ð��� �۵�
        {
            doubleJumpTimer += Time.deltaTime;
        }

        if (isDash == true) //�뽬�� ���ӽð�
        {
            dashRangeTimer += Time.deltaTime;
        }

        if (dashCoolOn == true) //�뽬�� �ϱ� ���� ��Ÿ��
        {
            dashCoolTimer -= Time.deltaTime;
            if (dashCoolTimer >= 1)
            {
                string timerTextInt = $"{(int)dashCoolTimer}";
                playerUI.SetPlayerDashCool(dashCoolTimer / dashCoolTime, timerTextInt);
            }
            else if (dashCoolTimer < 1)
            {
                string timerTextInt = $"{dashCoolTimer.ToString("F1")}";
                playerUI.SetPlayerDashCool(dashCoolTimer / dashCoolTime, timerTextInt);
            }
        }

        if (wallJumpTimerOn == true) //�� ������ �����̵Ǹ� �ٽ� �߷��� �ޱ� ���� Ÿ�̸�
        {
            wallJumpTimer += Time.deltaTime;
        }

        if (weaponsChangeCoolOn == true) //���� ���� Ÿ�̸Ӱ� true�� �۵�
        {
            weaponsChangeCoolTimer += Time.deltaTime;
        }

        if (animIsJump == true) //���� �ִϸ��̼��� ���۵Ǵ� �ð�
        {
            animTimer += Time.deltaTime;
        }

        if (weaponDrop == true) //���⸦ ������ �� �ٽ� ���� �� �ִ� ��� �ð�
        {
            weaponDropTime -= Time.deltaTime;
        }

        if (buffDamageUp == true)
        {
            buffDuration -= Time.deltaTime;
            if (buffDuration < 0)
            {
                buffDamageUp = false;
                playerBuffDamageUp = 0;
            }
        }

        if (playerHitDamage == true)
        {
            hitDamageTimer -= Time.deltaTime;
            if (hitDamageTimer < 0)
            {
                hitDamageTimer = 0.1f;
                playerHitDamage = false;
                playerRen.color = Color.white;
            }
        }
    }

    /// <summary>
    /// ������ �ݶ��̴� üũ�� ����ϴ� �Լ�
    /// </summary>
    private void itmeColliderCheck()
    {
        Collider2D weaponColl = Physics2D.OverlapBox(playerBoxColl2D.bounds.center,
                playerBoxColl2D.bounds.size, 0f, LayerMask.GetMask("Weapon")); //�÷��̾� �ݶ��̴��� ���� ���̾ Ȯ����  weaponColl�� �ִ´�

        Collider2D itemColl = Physics2D.OverlapBox(playerBoxColl2D.bounds.center,
                playerBoxColl2D.bounds.size, 0f, LayerMask.GetMask("Item")); //�÷��̾� �ݶ��̴��� ���� ���̾ Ȯ���� itemColl�� �ִ´�

        Collider2D petColl = Physics2D.OverlapBox(playerBoxColl2D.bounds.center,
                playerBoxColl2D.bounds.size, 0f, LayerMask.GetMask("Pet")); //�÷��̾� �ݶ��̴��� ���� ���̾ Ȯ���� petColl�� �ִ´�

        if (weaponColl != null)
        {
            pickUpItem(weaponColl);
        }

        if (itemColl != null)
        {
            pickUpItem(itemColl);
        }

        if (petColl != null)
        {
            pickUpItem(petColl);
        }

        //Collider2D[] colls = Physics2D.OverlapBoxAll(playerBoxColl2D.bounds.center, 
        //    playerBoxColl2D.bounds.size, 0f, LayerMask.GetMask("Weapon"));
        //int count = colls.Length;
        //for (int iNum = 0; iNum < count; ++iNum)
        //{
        //    Collider2D coll = colls[iNum];
        //    pickUpItem(coll);
        //    colls[iNum] = null;
        //}     
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
        if (isDash == true || wallJumpTimerOn == true)
        {
            return;
        }

        leftKey = Input.GetKey(keyManager.PlayerLeftKey()); //Ű �Ŵ������� ���� Ű�� �޾ƿͼ� ��� 
        rightKey = Input.GetKey(keyManager.PlayerRightKey()); //Ű �Ŵ������� ������ Ű�� �޾ƿͼ� ���        

        if (leftKey == true) //���� Ű�� ������ ��� ��������
        {
            moveVec.x = -1;
        }
        else if (rightKey == true) //������ Ű�� ������ ��� ����������
        {
            moveVec.x = 1;
        }
        else
        {
            moveVec.x = 0;
        }

        moveVec.x *= speed;
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

        if (jumpKey == true && isGround == true && isJump == false)
        {
            gravityVelocity = jumpPower;
            animIsJump = true;
            isJump = true;
            useWallSliding = true;
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
            gravityVelocity -= gravity * Time.deltaTime; //���������� �޴� �߷� 
            if (gravityVelocity > gravity)
            {
                gravityVelocity = gravity;
            }
        }
        else
        {
            gravityVelocity = -1; //isGround�� true�� ��� �߷��� -1�� ����� ���� �ٰ� ����
        }

        moveVec.y = gravityVelocity; //moveVec�� �߷��� ����
        rigid.velocity = moveVec;


        playerJump();
    }

    /// <summary>
    /// �÷��̾��� �뽬�� ����ϴ� �Լ�
    /// </summary>
    private void playerDash()
    {
        dashKey = Input.GetKeyDown(keyManager.PlayerDashKey());

        if (dashRangeTimer >= dashRange)
        {
            isDash = false;
            dashInvincibleOn = false;
            dashRangeTimer = 0.0f;
            playerRen.color = Color.white;
        }

        if (dashCoolTimer < 0)
        {
            dashCoolOn = false;
            dashCoolTimer = dashCoolTime;
            playerUI.PlayerDashCool(false);
        }

        if (dashKey == true && isDash == false && dashCoolOn == false)
        {
            isDash = true;

            dashCoolOn = true;

            dashInvincibleOn = true;

            playerUI.PlayerDashCool(true);

            gravityVelocity = 0.0f;

            playerRen.color = Color.black;

            if (moveVec.x > 0 && isWall == false) //moveVec.x �� 0���� ũ�� ���������� �뽬
            {
                moveVec.x = dashPower;
            }
            else if (moveVec.x < 0 && isWall == false) //moveVec.x �� 0���� ������ �������� �뽬
            {
                moveVec.x = -dashPower;
            }
            else if (moveVec.x == 0 && isWall == false) //moveVec.x �� 0�̸� ���콺 ������ �ִ� ������ �뽬
            {
                moveVec.x = dashPower;
                if (mouseAimRight == false)
                {
                    moveVec.x = -dashPower;
                }
            }

            rigid.velocity = moveVec;
        }
    }

    /// <summary>
    /// ���� ���õ� ��ȣ�ۿ� �� ����� �ϱ� ���� �Լ�
    /// </summary>
    private void playerWallSkill()
    {
        if (wallJumpTimer >= wallJumpSky)
        {
            wallJumpTimerOn = false;
            wallJumpTimer = 0.0f;
        }

        if (jumpKey == true && isWall == true && isGround == false && moveVec.x != 0) //�� ������ �ϱ� ���� ���ǽ�
        {
            gravityVelocity = wallJumpPower;
            moveVec.x *= -0.8f;
            rigid.velocity = moveVec;
            wallJumpTimerOn = true;
            useWallSliding = true;
            animIsJump = true;
        }
        else if (jumpKey == false && isWall == true && isGround == false && moveVec.x != 0 && useWallSliding == false)  //�� �����̵��� �ϱ� ���� ���ǽ�
        {
            gravityVelocity = -wallSlidingSpeed;
        }
        else if ((moveVec.x == 0 && isWall == false) || isWall == false || isGround == true)
        {
            if (moveVec.x != 0 && isGround == true)
            {
                return;
            }

            useWallSliding = false;
        }
    }

    /// <summary>
    /// �÷��̾ ������ �ִ� ������ �ϳ��� ������ ����ϴ� �Լ�
    /// </summary>
    private void playerWeaponChange()
    {
        if (weaponPrefabs.Count < 2) //���� �迭�� �ƹ��͵� ���ų�, 2���� ������ ������ ����
        {
            return;
        }

        if (weaponPrefabs.Count > 2) //���� �迭�� ���̰� 2�� �Ѿ�� �� �Ŀ� ����� �迭�� ����
        {
            weaponPrefabs.RemoveAt(2);
        }

        if (weaponsChangeCoolTimer >= weaponsChangeCoolTime)
        {
            weaponsChangeCoolOn = false;
            weaponsChangeCoolTimer = 0.0f;
        }

        if (Input.GetKeyDown(keyManager.WeaponChangeKey()))
        {
            int count = weaponPrefabs.Count;

            if (weaponSwap == false && weaponsChangeCoolOn == false)
            {
                for (int i = 0; i < count; i++)
                {
                    SpriteRenderer weaponRen = weaponPrefabs[i].GetComponent<SpriteRenderer>();
                    Weapons weaponSc = weaponPrefabs[i].GetComponent<Weapons>();

                    if (i == 0)
                    {
                        weaponRen.enabled = true;
                        weaponSc.ShootingOn(true);
                    }
                    else if (i == 1)
                    {
                        weaponRen.enabled = false;
                        weaponSc.ShootingOn(false);
                    }
                }

                weaponSwap = true;
                weaponsChangeCoolOn = true;
                gameManager.ReloadingObj().SetActive(false);
            }
            else if (weaponSwap == true && weaponsChangeCoolOn == false)
            {
                for (int i = 0; i < count; i++)
                {
                    SpriteRenderer weaponRen = weaponPrefabs[i].GetComponent<SpriteRenderer>();
                    Weapons weaponSc = weaponPrefabs[i].GetComponent<Weapons>();

                    if (i == 0)
                    {
                        weaponRen.enabled = false;
                        weaponSc.ShootingOn(false);
                    }
                    else if (i == 1)
                    {
                        weaponRen.enabled = true;
                        weaponSc.ShootingOn(true);
                    }
                }

                weaponSwap = false;
                weaponsChangeCoolOn = true;
                gameManager.ReloadingObj().SetActive(false);
            }
        }
    }

    /// <summary>
    /// �÷��̾ ���⸦ ���� �� �ְ� ����ϴ� �Լ�
    /// </summary>
    private void playerWeaponDrop()
    {
        if (weaponPrefabs.Count < 1 || weaponPrefabs == null) //���� �迭�� �ƹ��͵� ���ų�, 1���� ������ ������ ����
        {
            return;
        }

        if (weaponDropTime < 0.0f)
        {
            weaponDrop = false;
            weaponDropTime = 0.5f;
        }

        if (Input.GetKeyDown(keyManager.DropItemKey()) && weaponDrop == false)
        {
            int count = weaponPrefabs.Count;
            int removeIndex = 0;
            for (int i = 0; i < count; i++)
            {
                SpriteRenderer weaponRen = weaponPrefabs[i].GetComponent<SpriteRenderer>();
                BoxCollider2D weaponBoxColl = weaponPrefabs[i].GetComponent<BoxCollider2D>();
                Weapons weaponSc = weaponPrefabs[i].GetComponent<Weapons>();
                WeaponSkill weaponSkillSc = weaponPrefabs[i].GetComponent<WeaponSkill>();

                if (weaponRen.enabled == true)
                {
                    weaponDrop = true; //����߷ȴٴ� ���� Ȯ��
                    weaponSc.ShootingOn(false); //������ ����
                    weaponSc.PickUpImageOff(false); //������ �ݱ� Ű �̹��� ����
                    weaponSc.WeaponGravityOff(false); //���� �������� �߷��� Ȱ��ȭ
                    weaponSc.PassiveDamageUp(0); //���⿡ ����� �нú�ȿ�� ����
                    weaponSc.ReturnDamage(); //���⸦ �⺻������ �ǵ���
                    weaponSc.GetPlayerCriticalPcent(0, 1);
                    weaponSkillSc.WeaponSkillOff(false); //��ų �̹��� ��Ȱ��ȭ
                    weaponBoxColl.enabled = true; //������ �ڽ��ݶ��̴��� Ȱ��ȭ
                    weaponRen.sortingOrder = -2;  //��������Ʈ�� ���� �� ���̾ -2�� ����
                    gameManager.ReloadingObj().SetActive(false);  //���ε� UI ��Ȱ��ȭ
                    weaponPrefabs[i].transform.SetParent(gameManager.ItemDropTrs());   //���⸦ ������ ��ġ�� �ڽ����� �־���
                    weaponPrefabs[i].transform.position = gameObject.transform.position;  //������ �������� �÷��̾��� ������
                    weaponPrefabs[i].transform.rotation = gameManager.ItemDropTrs().rotation;  //ȸ�� ���� ������ ��ġ�� ȸ�� ���� �޾ƿ�
                    weaponPrefabs[i].transform.localScale = weaponLocalScale;  //������ ������ ��ġ��ŭ ����
                    removeIndex = i;
                }
            }

            if (weaponPrefabs.Count == 1)
            {
                Weapons weaponScA = weaponPrefabs[0].GetComponent<Weapons>();

                weaponScA.ShootingOn(false);
                weaponScA.BuffDamage(0);
            }
            else if (weaponPrefabs.Count == 2)
            {
                SpriteRenderer weaponRenA = weaponPrefabs[0].GetComponent<SpriteRenderer>();
                Weapons weaponScA = weaponPrefabs[0].GetComponent<Weapons>();
                SpriteRenderer weaponRenB = weaponPrefabs[1].GetComponent<SpriteRenderer>();
                Weapons weaponScB = weaponPrefabs[1].GetComponent<Weapons>();

                if (weaponRenA.enabled == true)
                {
                    weaponRenB.enabled = true;
                    weaponScA.ShootingOn(false);
                    weaponScB.ShootingOn(true);
                    weaponSwap = true;
                    weaponScA.BuffDamage(0);
                }
                else if (weaponRenB.enabled == true)
                {
                    weaponRenA.enabled = true;
                    weaponScA.ShootingOn(true);
                    weaponScB.ShootingOn(false);
                    weaponSwap = false;
                    weaponScB.BuffDamage(0);
                }
            }
            weaponPrefabs.RemoveAt(removeIndex);
        }
    }

    /// <summary>
    /// �÷��̾ ���� ���� ���ݷ��� ���⿡ �����Ű�� ���� �Լ�
    /// </summary>
    private void playerBuffDamage()
    {
        if (weaponPrefabs.Count == 1)
        {
            Weapons weaponSc = weaponPrefabs[0].GetComponent<Weapons>();

            if (buffDamageUp == true)
            {
                weaponSc.BuffDamage(playerBuffDamageUp);
            }
            if (buffDamageUp == false)
            {
                weaponSc.BuffDamage(0);
            }
        }
        else if (weaponPrefabs.Count == 2)
        {
            int count = weaponPrefabs.Count;
            for (int i = 0; i < count; i++)
            {
                Weapons weaponSc = weaponPrefabs[i].GetComponent<Weapons>();
                weaponSc.BuffDamage(playerBuffDamageUp);

                if (buffDamageUp == true)
                {
                    weaponSc.BuffDamage(playerBuffDamageUp);
                }
                if (buffDamageUp == false)
                {
                    weaponSc.BuffDamage(0);
                }
            }
        }
    }

    /// <summary>
    /// �÷��̾ �ǰ��� �Ծ��� �� Ÿ�ݰ��� �ִ� �Լ�
    /// </summary>
    private void playerDamageHit()
    {
        if (playerHitDamage == true)
        {
            playerRen.color = Color.red;
        }
    }

    /// <summary>
    /// �÷��̾��� �ǰ� 0�̵Ǹ� �۵��ϴ� �Լ�
    /// </summary>
    private void playerDead()
    {
        if (playerCurHealth <= 0)
        {
            string hpText = $"0 / {playerMaxHealth}";
            playerUI.SetPlayerHp(0, playerMaxHealth, hpText);
            SceneManager.LoadSceneAsync("DeadScene");
        }
        else if (playerCurHealth > 0 && transform.gameObject != null)
        {
            string hpText = $"{playerCurHealth} / {playerMaxHealth}";
            playerUI.SetPlayerHp(playerCurHealth, playerMaxHealth, hpText);
        }
    }

    /// <summary>
    /// �÷��̾��� ũ��Ƽ���� ����ϴ� �Լ�
    /// </summary>
    private void playerWeaponCritical()
    {
        if (weaponPrefabs != null)
        {
            int count = weaponPrefabs.Count;
            for (int i = 0; i < count; i++)
            {
                SpriteRenderer weaponRen = weaponPrefabs[i].GetComponent<SpriteRenderer>();
                Weapons weaponSc = weaponPrefabs[i].GetComponent<Weapons>();

                if (weaponRen.enabled == true)
                {
                    weaponSc.GetPlayerCriticalPcent(playerCritical, playerCriDamage);
                }
            }
        }
    }

    /// <summary>
    /// ���� �ִ� ȿ�� ����
    /// </summary>
    private void petPassiveBuff()
    {
        if (weaponPrefabs != null || petSc != null)
        {
            int count = weaponPrefabs.Count;
            for (int i = 0; i < count; i++)
            {
                Weapons weaponSc = weaponPrefabs[i].GetComponent<Weapons>();
                weaponSc.PassiveDamageUp(playerDamage);
            }
        }
    }

    /// <summary>
    /// �÷��̾ �ɼ�â�� �Ѱ� �� �� �ְ� �����ϵ��� �����ִ� �Լ�
    /// </summary>
    private void playerOption()
    {
        if (Input.GetKeyDown(keyManager.OptionKey()) && optionOn == false)
        {
            optionOn = true;
            playerUI.OptionOn(optionOn);
        }
        else if (Input.GetKeyDown(keyManager.OptionKey()) && optionOn == true)
        {
            optionOn = false;
            playerUI.OptionOn(optionOn);
        }

        gameManager.GamePause(optionOn);
    }

    /// <summary>
    /// �÷��̾��� �ִϸ��̼��� ����ϴ� �Լ�
    /// </summary>
    private void playerAni()
    {
        anim.SetInteger("isWalk", (int)moveVec.x);
        anim.SetBool("isJump", animIsJump);
        anim.SetBool("isGround", isGround);

        if (animTimer >= animJumpTime)
        {
            animIsJump = false;
            animTimer = 0.0f;
        }
    }

    /// <summary>
    ///  �÷��̾ �� ������ �� �� �ִ� ��Ȳ���� �ƴ��� üũ�� �ϱ� ���� �Լ�
    /// </summary>
    public void playerWallCheck(bool _wallHit, Collider2D _collision)
    {
        if (_wallHit == true && _collision.gameObject.layer == LayerMask.NameToLayer("Ground")
            && _collision.gameObject.tag == "JumpWall")
        {
            isWall = true;
        }
        else if (_wallHit == false && _collision.gameObject.layer == LayerMask.NameToLayer("Ground")
            && _collision.gameObject.tag == "JumpWall")
        {
            isWall = false;
        }
    }

    /// <summary>
    /// �÷��̾ų ��ũ��Ʈ���� �޾ƿ� ��ų Ÿ��
    /// </summary>
    /// <returns></returns>
    public PlayerSkillType SkillType()
    {
        return skillType;
    }

    /// <summary>
    /// ��ų ������ Ȯ���ϱ� ���� ���콺 ������ ������ ��ȯ
    /// </summary>
    /// <returns></returns>
    public bool PlayerMouseAimRight()
    {
        return mouseAimRight;
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� �÷��̾� Hp�� �־��� ���� �޾ƿ��� ���� �Լ�
    /// </summary>
    public void PlayerCurHp(int _damage, bool _hit, bool _trueDmg)
    {
        if (_trueDmg == true)
        {
            if (dashInvincibleOn == false)
            {
                playerCurHealth -= _damage;
                playerHitDamage = _hit;
            }
        }
        else if (_trueDmg == false)
        {
            if (dashInvincibleOn == false)
            {
                int dmgReduction = _damage - playerArmor;
                if (dmgReduction <= 0)
                {
                    playerCurHealth -= 1;
                    playerHitDamage = _hit;
                }
                else if (dmgReduction > 0)
                {
                    playerCurHealth -= _damage - playerArmor;
                    playerHitDamage = _hit;
                }
            }
        }
    }

    public void GravityVelocityValue(float _gravityVelocity)
    {
        gravityVelocity = _gravityVelocity;
    }

    public float PlayerBuffDamage()
    {
        return playerBuffDamageUp;
    }

    public Vector3 PlayerMoveVec()
    {
        return moveVec;
    }

    /// <summary>
    /// �ٸ� ��ũ��Ʈ���� �ɼ�â�� ���� ���� �Լ�
    /// </summary>
    public void OptionOff(bool _off)
    {
        optionOn = _off;
    }

    /// <summary>
    /// �÷��̾� ���ݷ��� ��½�Ű�� �Լ�
    /// </summary>
    /// <param name="_damageUp"></param>
    /// <param name="_armorUp"></param>
    /// <param name="_hpUp"></param>
    public void PlayerStatusDamage(int _damageUp, float _damageUpPercent)
    {
        playerDamage += (_damageUp + (playerDamage * _damageUpPercent));
    }

    /// <summary>
    /// �÷��̾� ������ ��½�Ű�� �Լ�
    /// </summary>
    /// <param name="_armorUp"></param>
    /// <param name="_armorUpPercent"></param>
    public void PlayerStatusArmor(int _armorUp, float _armorUpPercent)
    {
        playerArmor += (_armorUp + (int)(playerArmor * _armorUpPercent));
    }

    /// <summary>
    /// �÷��̾� ü���� ��½�Ű�� �Լ�
    /// </summary>
    /// <param name="_hpUp"></param>
    /// <param name="_hpUpPercent"></param>
    public void PlayerStatusHp(int _hpUp, float _hpUpPercent)
    {
        playerMaxHealth += (_hpUp + (int)(playerMaxHealth * _hpUpPercent));
        playerCurHealth = playerMaxHealth;
    }

    /// <summary>
    /// �÷��̾� ũ��Ƽ��Ȯ���� �������� ��½�Ű�� �Լ�
    /// </summary>
    /// <param name="_criticalPercentage"></param>
    /// <param name="_criDamage"></param>
    public void PlayerStatusCritical(float _criticalPercentage, float _criDamage)
    {
        playerCritical += _criticalPercentage;
        playerCriDamage += _criDamage;
    }

    public void PlayerSavedData(float _damage, int _armor, int _hp, float _critical, float _cridmg)
    {
        playerDamage = _damage;
        playerArmor = _armor;
        playerMaxHealth = _hp;
        playerCritical = _critical;
        playerCriDamage = _cridmg;
    }

    public void PlayerSavedData(SavedObjectData _savedObject)
    {
        playerDamage = _savedObject.playerDamage;
        playerArmor = _savedObject.playerArmor;
        playerMaxHealth = _savedObject.playerHp;
        playerCurHealth = _savedObject.playerCurHp;
        playerCritical = _savedObject.playerCritical;
        playerCriDamage = _savedObject.playerCriDamage;
    }

    public void PlayerSaveOn(bool _save)
    {
        dataSave = _save;
    }
}
