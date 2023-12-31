using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        playerBullet,
        enemyBullet
    }

    [SerializeField] BulletType bulletType;

    [Header("�Ѿ� ����")]
    [SerializeField, Tooltip("�Ѿ��� ���ư��� �ӵ�")] private float bulletSpeed = 1.0f;
    [SerializeField, Tooltip("�Ѿ��� ���ݷ�")] private float bulletDamage = 1.0f;

    private bool damageUpOn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        shootBullet();
        Destroy(gameObject, 1f);
    }

    private void shootBullet()
    {
        Vector3 bulletMove = transform.up * bulletSpeed * Time.deltaTime;
        transform.position += bulletMove;
    }

    public void BulletDamage(float _bulletDamage, float _bulletUpDamage, bool _damageUpOn)
    {
        damageUpOn = _damageUpOn;

        if (damageUpOn == false)
        {
            bulletDamage = _bulletDamage;
        }
        else if (damageUpOn == true)
        {
            bulletDamage = _bulletDamage * _bulletUpDamage;
        }
    }
}
