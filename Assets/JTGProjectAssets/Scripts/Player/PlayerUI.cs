using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("�÷��̾� UI ���� ����")]
    [SerializeField, Tooltip("�÷��̾� ��ų �� �ǳ�")] private GameObject playerSkillCool;
    [SerializeField, Tooltip("�÷��̾� ��ų �� �ؽ�Ʈ")] private GameObject playerSkillCoolText;
    [SerializeField, Tooltip("�÷��̾� �뽬 �� �ǳ�")] private GameObject playerDashCool;
    [SerializeField, Tooltip("�÷��̾� ��ų �� �ؽ�Ʈ")] private GameObject playerDashCoolText;
    [SerializeField, Tooltip("�÷��̾� ü�� �����̴�")] private Slider playerHp;
    [SerializeField, Tooltip("�÷��̾� ü�� �ؽ�Ʈ")] private TMP_Text playerHpText;
    [SerializeField, Tooltip("�ɼ�â")] private GameObject option;

    /// <summary>
    /// �÷��̾� ��ų�� ���õ� ������Ʈ�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�� ���
    /// </summary>
    /// <param name="_objOn"></param>
    public void PlayerSkiilCool(bool _objOn)
    {
        playerSkillCool.SetActive(_objOn);
        playerSkillCoolText.SetActive(_objOn);
    }

    /// <summary>
    /// �÷��̾� ��ų ��Ÿ�� ���� �ؽ�Ʈ ���� �޾ƿ� ȭ�鿡 ������
    /// </summary>
    /// <param name="_imageFill"></param>
    /// <param name="_textValue"></param>
    public void SetPlayerSkillCool(float _imageFill, string _textValue)
    {
        Image skillCoolImage = playerSkillCool.GetComponent<Image>();
        skillCoolImage.fillAmount = _imageFill;

        TMP_Text skillCoolText = playerSkillCoolText.GetComponent<TMP_Text>();
        skillCoolText.text = _textValue;
    }

    /// <summary>
    /// �÷��̾� �뽬�� ���õ� ������Ʈ�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�� ���
    /// </summary>
    /// <param name="_objOn"></param>
    public void PlayerDashCool(bool _objOn)
    {
        playerDashCool.SetActive(_objOn);
        playerDashCoolText.SetActive(_objOn);
    }

    /// <summary>
    /// �÷��̾� �뽬 ��Ÿ�� ���� �ؽ�Ʈ ���� �޾ƿ� ȭ�鿡 ������
    /// </summary>
    /// <param name="_imageFill"></param>
    /// <param name="_textValue"></param>
    public void SetPlayerDashCool(float _imageFill, string _textValue)
    {
        Image skillDashImage = playerDashCool.GetComponent<Image>();
        skillDashImage.fillAmount = _imageFill;

        TMP_Text skillDashText = playerDashCoolText.GetComponent<TMP_Text>();
        skillDashText.text = _textValue;
    }

    /// <summary>
    /// �÷��̾� ü�°� ���õ� ���� �޾ƿ� �����̴��� �ؽ�Ʈ�� �־� ȭ�鿡 ������
    /// </summary>
    /// <param name="_hpValue"></param>
    /// <param name="_textValue"></param>
    public void SetPlayerHp(int _hpValue, int _hpMaxValue, string _textValue)
    {
        Slider hpSlider = playerHp.GetComponent<Slider>();
        hpSlider.value = _hpValue;
        hpSlider.maxValue = _hpMaxValue;

        TMP_Text HpText = playerHpText.GetComponent<TMP_Text>();
        HpText.text = _textValue;
    }

    public void OptionOn(bool _on)
    {
        option.SetActive(_on);
    }
}
