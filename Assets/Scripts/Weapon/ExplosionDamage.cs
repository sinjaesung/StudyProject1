using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] float damage;
    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();

        //hit의 물체가 IDamagable을 갖고 있으면 어떤 정보가 저장될 것이고,
        //갖고 있지 않다면 target의 정보는 null이 될 것이다.

        if (target != null)
        {
            Debug.Log("ExplosionDamage Damaged Target>>" + other.transform.name);
            //부딪힌 물체가 데미지를 입을 수 있는 대상일 때
            //IDamagable을 갖고 있는 물체는 무조건 OnDamage 함수를 갖고 있다.

            //맞은 데미지: 총의 대미지
            //맞은 지점: 총알이 다은 지점
            //맞은 회전값: 부딪힌 장소의 회전값 - hit.normal
            target.OnDamage(damage);
        }
    }
}
