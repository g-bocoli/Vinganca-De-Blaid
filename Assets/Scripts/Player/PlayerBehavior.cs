using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Propriedades de movimentação")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 5;

    [Header("Propriedades de ataque")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask attackLayer;

    private Rigidbody2D rigidbody;
    private IsGroundedChecker isGroundedChecker;
    private Health health;

    private SpriteRenderer spriteRenderer;

    private float moveDirection;

    private void Start()
    {
        GameManager.Instance.InputManager.OnJump += HandleJump;
        rigidbody = GetComponent<Rigidbody2D>();
        isGroundedChecker = GetComponent<IsGroundedChecker>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();

        health.OnDead += HandlePlayerDeath;
        health.OnHurt += HandlePlayerHurt;
    }

    private void FixedUpdate () 
    {
        MovePlayer();
        FlipPlayer(moveDirection);
    }

    private void MovePlayer()
    {
        moveDirection = GameManager.Instance.InputManager.Movement;
        //transform.Translate(moveDirection * Time.deltaTime * moveSpeed, 0, 0);
        rigidbody.velocity = new Vector2(moveDirection * moveSpeed * Time.fixedDeltaTime, rigidbody.velocity.y);
    }
    
    private void FlipPlayer(float direction)
    {
        if (direction < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void HandleJump()
    {
        if (!isGroundedChecker.IsGrounded()) return;
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerJump);
        rigidbody.velocity += jumpForce * Vector2.up;
    }

    private void HandlePlayerHurt()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerHurt);
    }
    
    private void HandlePlayerDeath()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerDeath);
        GetComponent<Collider2D>().enabled = false;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        GameManager.Instance.InputManager.DisablePlayerInput();
    }

    private void HandlePlayerWalk()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerWalk);
    }

    private void Attack()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerAttack);
        Collider2D[] hittedEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, attackLayer);
        print("Making enemy taking damage");
        print(hittedEnemies.Length);

        foreach(Collider2D hittedEnemy in hittedEnemies)
        {
            print("Checking enemy");
            if (hittedEnemy.TryGetComponent(out Health enemyHealth))
            {
                print("Enemy getting damage");
                enemyHealth.TakeDemage();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
