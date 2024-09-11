using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// ���� �����Ѵ�
public class Wand : MonoBehaviour
{
    public Transform fireTransform; // ���� �߻�� ��ġ

    private LineRenderer BeamLineRenderer; // �� ������ �׸��� ���� ������(var)

    //private AudioSource gunAudioPlayer; // �Ҹ� �����
    //[SerializeField] public AudioClip shotClip; // �߻� �Ҹ�(var)

    [SerializeField] public float damage = 25; // ���ݷ�(var)

    [SerializeField] public float timeBetFire = 0.12f; // �� �߻� ����(var)
    public float lastFireTime; //���������� �߻��� ����

    public Vector3 HitPos;

    public Camera mainCamera;

    // Reference to the projectile prefab
    public GameObject projectilePrefab;
    // Array to hold nearby target points
    public Vector3[] nearbyPoints = new Vector3[5];
    // Height from which projectiles will spawn (sky)
    public float spawnHeight = 50f;
    // Speed of the projectile's movement
    public float projectileSpeed = 10f;

    private PlayerInput playerInput; // �÷��̾��� �Է�

    private void Awake()
    {
        // ����� ������Ʈ���� ������ ��������
        BeamLineRenderer = GetComponent<LineRenderer>();
        //gunAudioPlayer = GetComponent<AudioSource>();

        //������Ʈ ���� ������ �����ϱ� ����
        //���η����� ������ �̰����� ����.
        //���η����� �� ���� ���� ����
        BeamLineRenderer.positionCount = 2;

        //���η����� ��� ��Ȱ��ȭ
        BeamLineRenderer.enabled = false;

        playerInput = GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        //�� �������� 0���� �ʱ�ȭ.
        lastFireTime = 0;
    }

    // �߻� �õ�
    public void Fire()
    {
        //���� üũ: ���� �߻縦 �� �� �ִ� �����ΰ�?
        //�Ѿ� �߻� ���ݸ�ŭ �̻��� �ð��� �귶����
        //�Ѵ� ������ ��쿡 �߻� ó���� �����Ѵ�.

        //�߻� �ð� üũ : ������ �߻� �ð� + �߻� ������ �� �ð��� ���� �ð����� �۴�
        //�ð��� �� �귶�ٴ� ���̴� �߻簡 �����ϵ���.
        Debug.Log($"�� ��� �õ�{Time.time} >= {lastFireTime} + {timeBetFire}({lastFireTime + timeBetFire}>>");

        if (Time.time >= lastFireTime + timeBetFire)
        {
            Debug.Log("�� ��� ����>>");
            Shot();
            //���� ������� ������ ���� �� �ð��� ����� �����Ѵ�.
            lastFireTime = Time.time;
        }
        else
        {
            Debug.Log("�� ��Ÿ��");
        }
    }

    // ���� �߻� ó��
    protected virtual void Shot()
    {
        Debug.Log("����");
        // Step1: Get Mouse Position in Screen Coordinates
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Step2 : Convert Mouse Position to World Coordinates
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            //Get the point where the ray intersects the plane
            Vector3 pointInWorld = ray.GetPoint(rayDistance);
            Debug.Log("WandShot MouseCursor to world Plane raycast pointInWorld>>" + pointInWorld);

            StartCoroutine(ShotEffect(pointInWorld));

            SetNearbyPoints(pointInWorld);
        }
    }

    void SetNearbyPoints(Vector3 pointInWorld_)
    {
        float offsetDistance = 3f; // Distance to offset nearby points

        nearbyPoints[0] = pointInWorld_ + new Vector3(offsetDistance, 0, 0);
        nearbyPoints[1] = pointInWorld_ + new Vector3(-offsetDistance, 0, 0);
        nearbyPoints[2] = pointInWorld_ + new Vector3(0, 0, offsetDistance);
        nearbyPoints[3] = pointInWorld_ + new Vector3(0, 0, -offsetDistance);
        nearbyPoints[4] = pointInWorld_ + new Vector3(offsetDistance, 0, offsetDistance);

        LaunchProjectiles(pointInWorld_);
    }
    // Spawn projectiles in the sky and make them fly down diagonally
    void LaunchProjectiles(Vector3 pointInWorld_)
    {
        Vector3[] allTargetPoints = new Vector3[6];
        allTargetPoints[0] = pointInWorld_;
        // Include the nearby points in the array of all target points
        for (int i = 0; i < nearbyPoints.Length; i++)
        {
            allTargetPoints[i + 1] = nearbyPoints[i];
        }

        // Spawn a projectile at each point and make it move towards its target
        foreach (Vector3 target in allTargetPoints)
        {
            // Calculate spawn position in the sky above the target
            Vector3 spawnPosition = target + new Vector3(0, spawnHeight, spawnHeight);

            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Start moving the projectile towards the target point
            StartCoroutine(MoveProjectileToTarget(projectile, target));
        }
    }

    // Coroutine to move the projectile towards the target
    private IEnumerator MoveProjectileToTarget(GameObject projectile, Vector3 target)
    {
        while (projectile != null)
        {
            // Move the projectile towards the target position
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, target, projectileSpeed * Time.deltaTime);

            // If the projectile reaches the target, destroy it
            if (Vector3.Distance(projectile.transform.position, target) < 0.1f)
            {
                Destroy(projectile);
                yield break;
            }

            yield return null;
        }
    }
    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� �Ѿ� ������ �׸���
    protected IEnumerator ShotEffect(Vector3 hitPosition)
    {

        //�Ҹ��� �� ���� ����Ѵ�.
       // gunAudioPlayer.PlayOneShot(shotClip);

        //�Ѿ� ���� �׷��ֱ�

        //���� �߱� ���ؼ� ������,������ �˾ƾ� �Ѵ�.
        //������ : �ѱ�
        //������ �� : �ε��� ��ġ
        //SetPosition(�� ��° ������, ��)

        BeamLineRenderer.SetPosition(0, fireTransform.position);
        BeamLineRenderer.SetPosition(1, hitPosition);

        //������,������ ������ ���Ŀ�
        //���� ���̵��� Ȱ��ȭ�Ѵ�,.
        BeamLineRenderer.enabled = true;

        //0.03�ʸ� ���� ���̰�, ���� �ٷ� ���� �������.
        yield return new WaitForSeconds(0.03f);

        BeamLineRenderer.enabled = false;
    }
}