using System.Collections.Generic;
using UnityEngine;

// 적 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour {
    public Enemy enemyPrefab; // 생성할 적 AI

    public Transform[] spawnPoints; // 적 AI를 소환할 위치들

    public float damageMax = 40f; // 최대 공격력
    public float damageMin = 20f; // 최소 공격력

    public float healthMax = 1400f; // 최대 체력
    public float healthMin = 600f; // 최소 체력

    public float speedMax = 3f; // 최대 속도
    public float speedMin = 1f; // 최소 속도

    public Color strongEnemyColor = Color.red; // 강한 적 AI가 가지게 될 피부색

    public List<Enemy> enemies = new List<Enemy>(); // 생성된 적들을 담는 리스트
    public int wave; // 현재 웨이브

    private void Update() {
        //실시간으로 웨이브(에너미 스폰) 상태를 체크
        //게임 오버 상태일 떄에는 스폰을 진행하지 않는다
        if(GameManager.instance !=null && GameManager.instance.isGameover)
        {
            return;
        }

        //게임 시스템
        //1웨이브마다 일정 갯수의 좀비가 스폰된다.
        //웨이브에 생성된 좀비를 모두 쓰러뜨리면 다음 웨이브가 실행되고,이전 웨이브보다 더 
        //많은 좀비가 생성된다. 웨이브 정보는 UI로도 표시한다.
        if(enemies.Count <= 0)
        {
            if (wave < 10)
            {
                SpawnWave();
            }
        }
    }

    // 웨이브 정보를 UI로 표시
    public void UpdateUI() {
        UIManager.instance.UpdateWaveText(wave, enemies.Count);
    }
    public bool isEnemyClear()
    {
        if (enemies.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // 현재 웨이브에 맞춰 적을 생성
    private void SpawnWave() {
        //좀비 생성 시마다 웨이브 레벨을 1씩 증가시킨다.
        wave++;

        //좀비가 생성된다.
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);
        //=> 현재 웨이브의 1.5배 만큼 좀비 수를 생성한다. 
        //좀비가 생성되는 타이밍 = 새로운 웨이브가 시작될 때.
        //좀비 생성 시마다 웨이브 레벨을 1씩 증가시킨다.
        Debug.Log("SpawnWave>>" + spawnCount);

        for (int j=0; j<spawnCount; j++)
        {
            //적의 파워 : 그때그때 랜덤으로 정한다.
            //최소0 최대1
            float enemytIntensity = Random.Range(0, 1.0f);
            CreateEnemy(enemytIntensity);
        }

        UpdateUI();
    }

    // 적을 생성하고 생성한 적에게 추적할 대상을 할당
    private void CreateEnemy(float intensity)
    {
        Debug.Log("에너미 소환 : " + intensity);
        //선택한 파워의 좀비 생산

        float health = Mathf.Lerp(healthMin, healthMax, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(speedMin, speedMax, intensity);

        Debug.Log($"체력: {health}, 데미지 : {damage}, 이동속도 : {speed}");

        //좀비의 세기가 육안으로 보이도록
        //좀비의 피부색을 바꾼다 => 강할수록 색에 물들게
        Color skinColor = Color.Lerp(Color.white, strongEnemyColor, intensity);
        Debug.Log("Enemy생성>" + skinColor);
       //에너미 생성
       Enemy enemy = Instantiate(enemyPrefab, spawnPoints[Random.Range(0,spawnPoints.Length)]);

        //생성한 좀비의 스펙 바꾸기.
        enemy.Setup(health, damage, speed, skinColor);

        enemies.Add(enemy);

        enemy.onDeath += () => enemies.Remove(enemy);
        enemy.onDeath += () => Destroy(enemy.gameObject, 10f);

        //적 사망 시 플레이어 점수 상승.
        enemy.onDeath += () => GameManager.instance.AddScore(100);
    }
}