using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private KeyManager keyManager;

    [Header("Ʃ�丮�� ������ ��ǳ�� �� �̵� ����")]
    [SerializeField, Tooltip("��ǳ��")] private List<GameObject> conversations;
    [SerializeField, Tooltip("�ڽ��� �����ϸ� �ߴ� ��ǳ��")] private GameObject dontConversations;
    [SerializeField, Tooltip("�÷��̾� ������ ���� ��ǳ��")] private GameObject Barricade;
    [SerializeField, Tooltip("��ȣ�ۿ��� Ű �̹���")] private GameObject interactionKeyImage;
    [SerializeField, Tooltip("���� ��ǳ���� �߰� �ϱ� ���� üũ�ݶ��̴�")] private BoxCollider2D dontConversationsCheckColl;

    private int conversationsCount = 0;
    private bool conversationing = false;
    private bool conversationsEnd = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            conversationing = true;
            if (conversationing == true && conversationsEnd == false)
            {
                dontConversations.SetActive(false);
                interactionKeyImage.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (conversationing == true && conversationsEnd == false)
            {
                interactionKeyImage.SetActive(false);
            }
            else if (conversationsEnd == true)
            {
                int count = conversations.Count;
                for (int i = 0; i < count; i++)
                {
                    conversations[i].SetActive(false);
                }

                Destroy(gameObject);
            }
        }
    }

    private void onTriggerCheck(Collider2D _collision)
    {
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Barricade.activeSelf == true)
            {
                dontConversations.SetActive(true);
            }
        }
    }

    private void Start()
    {
        keyManager = KeyManager.instance;

        interactionKeyImage.SetActive(false);
    }

    private void Update()
    {
        collCheck();
        interaction();
    }

    private void collCheck()
    {
        Collider2D collCheck = Physics2D.OverlapBox(dontConversationsCheckColl.bounds.center,
            dontConversationsCheckColl.bounds.size, 0f, LayerMask.GetMask("Player"));

        if (collCheck != null)
        {
            onTriggerCheck(collCheck);
        }
        else
        {
            dontConversations.SetActive(false);
        }
    }

    private void interaction()
    {
        if (conversationsEnd == false)
        {
            if (conversationing == true)
            {
                if (Input.GetKeyDown(keyManager.InteractionKey()))
                {
                    if (conversations.Count - 1 < conversationsCount)
                    {
                        Barricade.SetActive(false);
                        conversationsEnd = true;
                        return;
                    }
                    else if (conversationsCount - 1 > -1)
                    {
                        conversations[conversationsCount - 1].SetActive(false);
                    }

                    interactionKeyImage.SetActive(false);
                    conversations[conversationsCount].SetActive(true);
                    conversationsCount++;
                }
            }
        }
    }
}
