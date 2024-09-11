using UnityEngine;

// �־��� Gun ������Ʈ�� ��ų� ������
// �˸��� �ִϸ��̼��� ����ϰ� IK�� ����� ĳ���� ����� �ѿ� ��ġ�ϵ��� ����
public class PlayerShooter2 : MonoBehaviour
{
    //���� ��ũ��Ʈ(����,�⺻�� ����Gun���):���� �����ɽ�Ʈ Ÿ�� ��� ���
    public Gun gun; // ����� ��(����,����)
    public Transform gunPivot; // �� ��ġ�� ������
    public Transform leftHandMount; // ���� ���� ������, �޼��� ��ġ�� ����
    public Transform rightHandMount; // ���� ������ ������, �������� ��ġ�� ����

    private PlayerInput playerInput; // �÷��̾��� �Է�
    private Animator playerAnimator; // �ִϸ����� ������Ʈ

    public float UpperBodyIKWeight = 1;
    private void Start()
    {
        //playerinput,playeranimator ���� �޾ƿ���
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();

    }

    private void OnEnable()
    {
        //���Ϳ� ���� �׻� �Բ� �ֵ���
        //���Ͱ� Ȱ��ȭ�Ǹ�,�ѵ� Ȱ��ȭ�ǵ��� �Ѵ�.
        gun.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        //���Ͱ� ������� �ѵ� ��Ȱ��ȭ�ǵ��� �Ѵ�
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        // �Է��� �����ϰ� �� �߻��ϰų� ������

        //���� �߻��Ѵٴ� �Է��� �������� ��
        //�� �߻� ��ũ��Ʈ�� ����. (gun ��ũ��Ʈ�� Fire)
        if (playerInput.fire)
        {
            //���� �߻��� �� �ִ��� üũ�ϴ� �Լ� ���� ( gun ��ũ��Ʈ�� Fire)
            gun.Fire();
        }
        //���� �������Ѵٴ� �Է��� �������� ��
        else if (playerInput.reload)
        {
            //������
            if (gun.Reload() == true)//�� Ÿ�ֿ̹� �̹� ���ε� �Լ��� ����ƴ�.
            {
                playerAnimator.SetTrigger("Reload");
            }
        }
        //UpdateUI();
        //������
        //�������� �������� �� ���� �ִϸ��̼��� ����
    }

    // ź�� UI ����
    private void UpdateUI()
    {
        /*if (UIManager.instance != null && gun != null)
        {
            //�����ؾ� �� Ŭ������ �Ҵ�Ǿ� �ִ��� üũ
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }*/
    }

    // �ִϸ������� IK ����
    private void OnAnimatorIK(int layerIndex)
    {
        //FK:�θ� ����Ʈ(����) �� ��ġ�� ���� �ڽ� ����Ʈ(����)�� ��ġ�� ������
        //�ڵ����� ��ġ�� �����ϴ� ���.
        //IK: �ڽ� ����Ʈ�� ��ġ�� ���� �θ� ����Ʈ�� ����ġ�� ����,�ڵ����� ��ġ�� �����Ѵ�.

        //���� : ���� ��� ��ġ�� �÷��̾��� ���� �����Ѵ�.
        //���� ��ġ�� ���� �Ȳ�ġ�� ��ġ�� �ڵ����� ����ǵ��� �����Ѵ�.

        //���� ��ġ�� �÷��̾��� ������ �Ȳ�ġ�� ����ٴϵ��� �̺�Ʈ �Լ� �ֱ�� �����Ѵ�.
        //GetIKHintPosition : IK(����) �� �����ϴ� Ű������ ���� ��ġ ������ ����´�.
        Debug.Log($"OnAnimatorIK {layerIndex}");
        gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, UpperBodyIKWeight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, UpperBodyIKWeight);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);


        //������
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, UpperBodyIKWeight);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, UpperBodyIKWeight);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    }
}