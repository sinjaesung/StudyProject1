using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Item slots")]
    public GameObject Weapon1;
    public bool isWeapon1Active = true;//기본 권총

    public GameObject Weapon2;//서브머신건(샷건)
    public bool isWeapon2Active = false;

    public GameObject Weapon3;//바주카
    public bool isWeapon3Active = false;//gun상속

    public GameObject Weapon4;//스노우완드
    public bool isWeapon4Active = false;//wand

    public GameObject Weapon5;//파이어스워드
    public bool isWeapon5Active = false;//wand

    [Header("Weapons to Use")]
    public GameObject BaseGun;//기본 권총
    public GameObject ShotGun;//샷건 무기2
    public GameObject Bazooka;//바주카 무기3
    public GameObject SnowWand;//스노우 완드
    public GameObject FireSword;//파이어 스워드

    [Header("Scripts")]
    public PlayerShooter playershooterScript;//기본무기
    public PlayerShooter2 playershooter2Script;//샷건무기2
    public PlayerShooter3 playershooter3Script;//바주카무기3
    public WandShooter1 wandshooter1Script;//스노우완드
    public WandShooter2 wandshooter2Script;//파이어스워드

    private void Update()
    {
        if(playershooterScript.gun.state != Gun.State.Reloading &&
            playershooter2Script.gun.state != Gun.State.Reloading &&
            playershooter3Script.gun.state != Gun.State.Reloading
            )
        {
            //기본건 끼고있는 상태에서 재장전 or 샷건 or 바주카 끼고있는 상태에서 재장전 하고있던 경우라면 무기교체 제한
            if (Input.GetKeyDown("1"))
            {
                isWeapon1Active = true;
                isWeapon2Active = false;
                isWeapon3Active = false;
                isWeapon4Active = false;
                isWeapon5Active = false;
                isRifleActive();
            }else if (Input.GetKeyDown("2"))
            {
                isWeapon1Active = false;
                isWeapon2Active = true;
                isWeapon3Active = false;
                isWeapon4Active = false;
                isWeapon5Active = false;
                isRifleActive();
            }else if (Input.GetKeyDown("3"))
            {
                isWeapon1Active = false;
                isWeapon2Active = false;
                isWeapon3Active = true;
                isWeapon4Active = false;
                isWeapon5Active = false;
                isRifleActive();
            }
            else if (Input.GetKeyDown("4"))
            {
                isWeapon1Active = false;
                isWeapon2Active = false;
                isWeapon3Active = false;
                isWeapon4Active = true;
                isWeapon5Active = false;
                isRifleActive();
            }
            else if (Input.GetKeyDown("5"))
            {
                isWeapon1Active = false;
                isWeapon2Active = false;
                isWeapon3Active = false;
                isWeapon4Active = false;
                isWeapon5Active = true;
                isRifleActive();
            }
            else
            {
                isRifleActive();
            }
        }
    }

    void isRifleActive()
    {
        if (isWeapon1Active == true)
        {
            BaseGun.SetActive(true);
            ShotGun.SetActive(false);
            Bazooka.SetActive(false);
            SnowWand.SetActive(false);
            FireSword.SetActive(false);

            playershooterScript.enabled = true;
            playershooter2Script.enabled = false;
            playershooter3Script.enabled = false;
            wandshooter1Script.enabled = false;
            wandshooter2Script.enabled = false;
        }
        else if(isWeapon2Active == true)
        {
            BaseGun.SetActive(false);
            ShotGun.SetActive(true);
            Bazooka.SetActive(false);
            SnowWand.SetActive(false);
            FireSword.SetActive(false);

            playershooterScript.enabled = false;
            playershooter2Script.enabled = true;
            playershooter3Script.enabled = false;
            wandshooter1Script.enabled = false;
            wandshooter2Script.enabled = false;
        }
        else if (isWeapon3Active == true)
        {
            BaseGun.SetActive(false);
            ShotGun.SetActive(false);
            Bazooka.SetActive(true);
            SnowWand.SetActive(false);
            FireSword.SetActive(false);

            playershooterScript.enabled = false;
            playershooter2Script.enabled = false;
            playershooter3Script.enabled = true;
            wandshooter1Script.enabled = false;
            wandshooter2Script.enabled = false;
        }
        else if (isWeapon4Active == true)
        {
            BaseGun.SetActive(false);
            ShotGun.SetActive(false);
            Bazooka.SetActive(false);
            SnowWand.SetActive(true);
            FireSword.SetActive(false);

            playershooterScript.enabled = false;
            playershooter2Script.enabled = false;
            playershooter3Script.enabled = false;
            wandshooter1Script.enabled = true;
            wandshooter2Script.enabled = false;
        }
        else if (isWeapon5Active == true)
        {
            BaseGun.SetActive(false);
            ShotGun.SetActive(false);
            Bazooka.SetActive(false);
            SnowWand.SetActive(false);
            FireSword.SetActive(true);

            playershooterScript.enabled = false;
            playershooter2Script.enabled = false;
            playershooter3Script.enabled = false;
            wandshooter1Script.enabled = false;
            wandshooter2Script.enabled = true;
        }
    }
}
