using UnityEngine;

// 체력을 회복하는 아이템
public class HealthPack : MonoBehaviour, IItem {
    public float health = 50; // 체력을 회복할 수치

    public void Use(GameObject target) {
        //target : 플레이어 오브젝트

        //PlayerHealth (혹은 LivingEntity)의 health 값을 상승시킨다.

        //클래스 참조 받아오기
        LivingEntity life = target.GetComponent<LivingEntity>();

        //참조에 성공하면
        //체력 회복
        if(life != null)
        {
            //체력 회복
            life.RestoreHealth(health);

            //아이템 오브젝트 파괴
            Destroy(this.gameObject);
        }
    }
}