using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    //�÷��̾ų�� ������ �Ŵ���
    private GameManager gameManager; //���ӸŴ���
    private KeyManager keyManager; //Ű�Ŵ���

    private TrashPreFab trashPreFab;

    private Player player;

    [Header("��ų ������")]
    [SerializeField] private List<GameObject> skillPrefabs = new List<GameObject>();

    [Header("��ų�� ��Ÿ�� ��ġ �� ȸ�� ��")]
    [SerializeField] private Transform skillPos;
    [SerializeField] private Transform skillRot;

    [Header("��ų ����")]
    [SerializeField] private float skillCoolTime = 1.0f;
    private float skillCoolTimer = 0.0f;
    private bool skillCoolOn = false;
    private float coolTime;


    private void Start()
    {
        gameManager = GameManager.Instance;
        keyManager = KeyManager.instance;

        trashPreFab = TrashPreFab.instance;

        player = GetComponent<Player>();

        skillCoolTimer = skillCoolTime;

        gameManager.PlayerSkillOn(true);
        gameManager.PlayerSkillCoolTimePanel(false);

        coolTime = gameManager.PlayerCoolTimePanelImage().fillAmount;
    }

    private void Update()
    {
        skillCool();
        playerSpecialAttack();
    }

    /// <summary>
    /// ��ų ��� �� ��Ÿ�ӵ�
    /// </summary>
    private void skillCool()
    {
        if (skillCoolOn == true)
        {
            string timerText = $"{(int)skillCoolTimer + 1}";
            gameManager.PlayerCoolTimeText().text = timerText;
            gameManager.PlayerSkillCoolTimePanel(true);
            skillCoolTimer -= Time.deltaTime;
            gameManager.PlayerCoolTimePanelImage().fillAmount = skillCoolTimer;
            if (skillCoolTimer < 0.0f)
            {
                skillCoolOn = false;
                gameManager.PlayerSkillCoolTimePanel(false);
                gameManager.PlayerCoolTimePanelImage().fillAmount = coolTime;
                skillCoolTimer = skillCoolTime;
            }
        }
    }

    /// <summary>
    /// �÷��̾���� Ư�� ��ų���� ����ϴ� �Լ�
    /// </summary>
    private void playerSpecialAttack()
    {
        if (Input.GetKeyDown(keyManager.PlayerSpecialAttackKey()) && skillCoolOn == false)
        {
            skillCoolOn = true;          

            Player.PlayerSkillType skillType = player.SkillType();
            if (skillType.ToString() == "skillTypeA")
            {
                GameObject skillObj = Instantiate(skillPrefabs[0], skillPos.position,
                    skillRot.rotation, trashPreFab.transform);
                Knife knifeSc = skillObj.GetComponent<Knife>();

                knifeSc.KnifeForce(skillRot.rotation * (new Vector2(15.0f, 0f)), player.playerMouseAimRight());
            }
            else if (skillType.ToString() == "skillTypeB")
            {

            }
            else if (skillType.ToString() == "skillTypeC")
            {

            }
            else if (skillType.ToString() == "skillTypeD")
            {

            }
        }
    }
}
