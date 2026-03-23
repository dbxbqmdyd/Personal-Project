using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    public void Init(int damage, float duration)
    {
        // 0.2초 뒤에 자동으로 사라지게 설정 (그림 한 장일 때 유용)
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 검기 자체에도 공격 판정을 넣고 싶다면 여기에 TakeDamage 로직 추가
        if (collision.TryGetComponent<Project.Boss.Boss>(out var boss))
        {
            // 여기서 데미지를 줄 수도 있습니다.
        }
    }
}