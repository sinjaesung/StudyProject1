﻿using System;
using UnityEngine;

// 생명체로서 동작할 게임 오브젝트들을 위한 뼈대를 제공
// 체력, 데미지 받아들이기, 사망 기능, 사망 이벤트를 제공
public class LivingEntity : MonoBehaviour, IDamageable {
    public float startingHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 상태
    public event Action onDeath; // 사망시 발동할 이벤트

    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable() {
        //현재 체력을 초기 체력으로 돌린다.
        health = startingHealth;
        //'사망 상태가 아님'으로 캐릭터를 설정
        dead = false;
    }

    // 데미지를 입는 기능
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal) {
        //데미지를 입은 만큼 체력이 감소
        //체력이 0이 되었을 때 아직 죽지 않았을 떄 사망 처리 진행
        health = health - damage;

        if(health <=0 && dead == false){
            Die();
        }
    }
    public virtual void OnDamage(float damage)
    {
        //데미지를 입은 만큼 체력이 감소
        //체력이 0이 되었을 때 아직 죽지 않았을 떄 사망 처리 진행
        health = health - damage;

        if (health <= 0 && dead == false)
        {
            Die();
        }
    }

    // 체력을 회복하는 기능
    public virtual void RestoreHealth(float newHealth) {
        if(dead == false)
        {
            return;
        }

        health += newHealth;
    }

    // 사망 처리
    public virtual void Die() {

        if(onDeath != null)
        {
            onDeath();
        }

        dead = true;
    }
}