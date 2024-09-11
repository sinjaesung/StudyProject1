using UnityEngine;

// 총알을 충전하는 아이템
public class AmmoPack : MonoBehaviour, IItem {
    public int ammo = 30; // 충전할 총알 수

    public Inventory inventory;

    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void Use(GameObject target) {
        //플레이어의 전체 탄창 갯수를 증가시킨다.,
        //프렐이어의 gun을 갖고와서 사용 가능

        //target에 들어있는 정보 : 플레이어의 오브젝트
        //플레이어의 오브젝트로부터 gun클래스를 어떻게 참조해올 것인가??
        //PlayerShooter의 gun 변수를 활용

        var shooter = target.GetComponent<PlayerShooter>();//기본건
        var shooter2 = target.GetComponent<PlayerShooter2>();//샷건
        var shooter3 = target.GetComponent<PlayerShooter3>();//바주카

        if (inventory.isWeapon1Active)
        {
            shooter = target.GetComponent<PlayerShooter>();

            if (shooter != null && shooter.gun != null)
            {
                //참조에 성공하면
                //탄창 갯수 증가
                shooter.gun.ammoRemain += ammo;
                //이 오브젝트 파괴

                UIManager.instance.UpdateAmmoText(shooter.gun.magAmmo, shooter.gun.ammoRemain);

                Destroy(this.gameObject);
            }
        }
        else if (inventory.isWeapon2Active)
        {
            shooter2 = target.GetComponent<PlayerShooter2>();

            if (shooter2 != null && shooter2.gun != null)
            {
                //참조에 성공하면
                //탄창 갯수 증가
                shooter2.gun.ammoRemain += ammo;
                //이 오브젝트 파괴

                UIManager.instance.UpdateAmmoText(shooter2.gun.magAmmo, shooter2.gun.ammoRemain);

                Destroy(this.gameObject);
            }
        }
        else if (inventory.isWeapon3Active)
        {
            shooter3 = target.GetComponent<PlayerShooter3>();

            if (shooter3 != null && shooter3.gun != null)
            {
                //참조에 성공하면
                //탄창 갯수 증가
                shooter3.gun.ammoRemain += ammo;
                //이 오브젝트 파괴

                UIManager.instance.UpdateAmmoText(shooter3.gun.magAmmo, shooter3.gun.ammoRemain);

                Destroy(this.gameObject);
            }
        }   
    }
}