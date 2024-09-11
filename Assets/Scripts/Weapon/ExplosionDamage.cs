using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] float damage;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();

        //hit�� ��ü�� IDamagable�� ���� ������ � ������ ����� ���̰�,
        //���� ���� �ʴٸ� target�� ������ null�� �� ���̴�.

        if (target != null)
        {
            Debug.Log("ExplosionDamage Damaged Target>>" + other.transform.name);
            //�ε��� ��ü�� �������� ���� �� �ִ� ����� ��
            //IDamagable�� ���� �ִ� ��ü�� ������ OnDamage �Լ��� ���� �ִ�.

            //���� ������: ���� �����
            //���� ����: �Ѿ��� ���� ����
            //���� ȸ����: �ε��� ����� ȸ���� - hit.normal
            target.OnDamage(damage);
        }
    }
}
