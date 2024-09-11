using UnityEngine;

// 게임 점수를 증가시키는 아이템
public class Coin : MonoBehaviour, IItem {
    public int score = 200; // 증가할 점수

    public void Use(GameObject target) {
        //사용시 
        //게임의 점수가 상승한다 => score만큼 상승
        GameManager.instance.AddScore(score);
        //아이템 획득 후에는 본인 오브젝트를 파괴한다.
        Destroy(this.gameObject);
    }
}