using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private const int _ENEMY_HP_ORIGIN = 100;

    private int _healthPoints = _ENEMY_HP_ORIGIN;

    public void TakeDamage(int damage)
    {
        _healthPoints -= damage;

        if (_healthPoints <= 0)
            Debug.Log("Enemy is dead");
        else 
            Debug.Log($"Enemy HP: {_healthPoints} => {Time.time}");
    }
}
