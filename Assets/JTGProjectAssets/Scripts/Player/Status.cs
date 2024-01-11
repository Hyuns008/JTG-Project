using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("�÷��̾��� �ɷ�ġ�� �ø� ��ư")]
    [SerializeField] private Button damageUp; //����ġ ����Ʈ�� �̿��Ͽ� ���ݷ��� ��� ��Ű�� ��ư
    [SerializeField] private Button armorUp; //����ġ ����Ʈ�� �̿��Ͽ� ������ ��� ��Ű�� ��ư
    [SerializeField] private Button hpUp; //����ġ ����Ʈ�� �̿��Ͽ� ü���� ��� ��Ű�� ��ư
    [SerializeField] private Button criticalUp; //����ġ ����Ʈ�� �̿��Ͽ� ġ��Ÿ Ȯ���� ��� ��Ű�� ��ư

    [Header("�÷��̾��� ������ �־��� UI��")]
    [SerializeField] private TMP_Text playerLevel;
    [SerializeField] private TMP_Text playerExp;
    [SerializeField] private TMP_Text playerLevelPoint;
    [SerializeField] private TMP_Text playerDamage;
    [SerializeField] private TMP_Text playerArmor;
    [SerializeField] private TMP_Text playerCurHp;
    [SerializeField] private TMP_Text playerCritical;
    [SerializeField] private TMP_Text playerCriDamage;

    private void Awake()
    {
        damageUp.onClick.AddListener(() => 
        {
            player.playerPointDamageUp();
        });

        armorUp.onClick.AddListener(() =>
        {
            player.playerPointArmorUp();
        });

        hpUp.onClick.AddListener(() =>
        {
            player.playerPointHpUp();
        });

        criticalUp.onClick.AddListener(() =>
        {
            player.playerPointCriticalUp();
        });
    }

    private void Update()
    {
        playerStatus();
    }

    private void playerStatus()
    {
        if (player.PlayerStatusOpen() == true)
        {
            playerLevel.text = $"LV: {player.PlayerLevelReturn()}";
            playerExp.text = $"Exp: {player.PlayerExpReturn()} / {player.PlayerMaxExpReturn()}";
            playerLevelPoint.text = $"����Ʈ: {player.PlayerLevelPointReturn()}";
            playerDamage.text = $"���ݷ�: {player.PlayerDamageReturn()}";
            if (player.PlayerBuffDamageReturn() <= 0)
            {
                playerDamage.text = $"���ݷ�: {player.PlayerDamageReturn()}";
            }
            else if (player.PlayerBuffDamageReturn() > 0)
            {
                playerDamage.text = $"���ݷ�: {player.PlayerDamageReturn()} <color=red>+{player.PlayerBuffDamageReturn()}</color>"; //��ġ�ؽ�Ʈ ����ϱ�
            }
            playerArmor.text = $"����: {player.PlayerArmorReturn()}";
            playerCurHp.text = $"ü��: {player.PlayerCrHpReturn()} / {player.PlayerMaxHpReturn()}";
            playerCritical.text = $"ġ��ŸȮ��: {player.PlayerCriticalReturn().ToString("F1")}%";
            playerCriDamage.text = $"ġ��Ÿ������: {(int)player.PlayerCriDamageReturn() * 100}%";
        }
    }
}
