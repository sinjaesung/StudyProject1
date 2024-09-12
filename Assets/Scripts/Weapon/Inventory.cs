using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Item slots")]
    public GameObject Weapon1;
    public bool isWeapon1Active = true;//�⺻ ����

    public GameObject Weapon2;//����ӽŰ�(����)
    public bool isWeapon2Active = false;

    public GameObject Weapon3;//����ī
    public bool isWeapon3Active = false;//gun���

    public GameObject Weapon4;//�����ϵ�
    public bool isWeapon4Active = false;//wand

    public GameObject Weapon5;//���̾����
    public bool isWeapon5Active = false;//wand

    [Header("Weapons to Use")]
    public GameObject BaseGun;//�⺻ ����
    public GameObject ShotGun;//���� ����2
    public GameObject Bazooka;//����ī ����3
    public GameObject SnowWand;//����� �ϵ�
    public GameObject FireSword;//���̾� ������

    [Header("Scripts")]
    public PlayerShooter playershooterScript;//�⺻����
    public PlayerShooter2 playershooter2Script;//���ǹ���2
    public PlayerShooter3 playershooter3Script;//����ī����3
    public WandShooter1 wandshooter1Script;//�����ϵ�
    public WandShooter2 wandshooter2Script;//���̾����

    private void Update()
    {
        if(playershooterScript.gun.state != Gun.State.Reloading &&
            playershooter2Script.gun.state != Gun.State.Reloading &&
            playershooter3Script.gun.state != Gun.State.Reloading
            )
        {
            //�⺻�� �����ִ� ���¿��� ������ or ���� or ����ī �����ִ� ���¿��� ������ �ϰ��ִ� ����� ���ⱳü ����
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
