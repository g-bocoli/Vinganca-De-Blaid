using System;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] private Transform detectPosition;
    [SerializeField] private Vector2 detectBoxSize;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCoolDown;

    [Header("Audio properties")]
    [SerializeField] private AudioClip[] audioClips;

    private float coolDownTimer;

    protected override void Awake()
    {
        base.Awake();
        base.health.OnHurt += PlayHurtAudio;
        base.health.OnDead += PlayDeadAudio;
    }

    // Update is called once per frame
    protected override void Update()
    {
        coolDownTimer += Time.deltaTime;
        VerifyCanAttack();
    }

    private void VerifyCanAttack()
    {
        if (coolDownTimer < attackCoolDown)
            return;
        if (PlayerInSight())
        {
            animator.SetTrigger("attack");
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        coolDownTimer = 0;
        if (CheckPlayerInDetectArea().TryGetComponent(out Health playerHealth))
        {
            PlayAttackAudio();
            print("Making player take damage!!");
            playerHealth.TakeDemage();
        }
    }

    private Collider2D CheckPlayerInDetectArea()
    {
        return Physics2D.OverlapBox(detectPosition.position, detectBoxSize, 0f, playerLayer);
    }

    private bool PlayerInSight()
    {
        Collider2D playerCollider = CheckPlayerInDetectArea();
        return playerCollider != null;
    }
    private void PlayAttackAudio()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    private void PlayHurtAudio()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

    private void PlayDeadAudio()
    {
        audioSource.clip = audioClips[2];
        audioSource.Play();
    }

    

    private void OnDrawGizmos()
    {
        if (detectPosition == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(detectPosition.position, detectBoxSize);
    }
}
