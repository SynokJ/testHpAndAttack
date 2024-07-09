using UnityEngine;

public class Player : MonoBehaviour
{

    private const float _PLAYER_MOVEMENT_SPEED = 3.0f;
    private const float _HIT_RANGE = 1.0f;
    private const int _PLAYER_DAMAGE = 10;

    private TurnSide _turnSide = default;
    private Vector2 _hitOriginPosition;

    private System.Collections.Generic.Dictionary<TurnSide, Vector2> _dirByTurn =
        new System.Collections.Generic.Dictionary<TurnSide, Vector2>(){
        {TurnSide.leftSide, Vector2.left},
        {TurnSide.rightSide, Vector2.right }
    };

    void Update()
    {
        if (TryGetMovementDirectionByInput(out Vector2 dir))
        {
            MoveByDirection(dir);
            DetectTurSide(dirX: dir.x);
        }

        if (Input.GetMouseButtonDown(0))
            Attack();
    }

    private bool TryGetMovementDirectionByInput(out Vector2 dir)
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirY = Input.GetAxis("Vertical");
        dir = new Vector2(dirX, dirY);
        return dir != Vector2.zero;
    }

    private void MoveByDirection(Vector2 dir)
        => transform.Translate(dir * _PLAYER_MOVEMENT_SPEED * Time.deltaTime);

    private void Attack()
    {
        Vector2 hitDir = _dirByTurn[_turnSide];
        _hitOriginPosition = (Vector2)transform.position + hitDir;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _HIT_RANGE, hitDir, _HIT_RANGE);

        foreach (RaycastHit2D hit in hits)
            if (TryGetIDamageable(out IDamageable damageable, hit))
                damageable.TakeDamage(_PLAYER_DAMAGE);
    }

    private bool TryGetIDamageable(out IDamageable damageable, RaycastHit2D hit)
    {
        damageable = hit.transform.gameObject.GetComponent<IDamageable>();
        return damageable != null && hit.transform.gameObject != this.gameObject;    
    }

    private void DetectTurSide(float dirX)
    {
        if (dirX < 0) _turnSide = TurnSide.leftSide;
        else _turnSide = TurnSide.rightSide;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_hitOriginPosition, _HIT_RANGE);
    }
}

public enum TurnSide
{
    rightSide = 0,
    leftSide = 1
}
