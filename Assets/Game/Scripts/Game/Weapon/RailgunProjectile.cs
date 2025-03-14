using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class RailgunProjectile : AbstractProjectile
    {

        [SerializeField]
        private int _damage;
        [SerializeField]
        private int _speed;
        [SerializeField]
        private GameObject _destroyEffect;

        private void Update()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Health>(out var health))
            {
                health.DoDamage(new Health.Damage() { value = _damage });
            }
            Instantiate(_destroyEffect, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        //����
        //�������� 

        //����� ������ � ��� ������������ ������� ���� �����
        //����� GetComponent<Health> �������� ���� �� �������� ���� DoDamage(new Health.Damage(){value = _damage})
        //������ ������ ������������ ��� ����� ������������ 
    }
}