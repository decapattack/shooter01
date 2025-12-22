using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Estatasticas")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("Ataque")]
    [SerializeField] private GameObject enemyBulletPrefab; // O Prefab da bala do inimigo
    [SerializeField] private float shootInterval = 1.0f;   // Tempo entre cada tiro (segundos)

    [Header("audio e Visual")]
    [SerializeField] private AudioClip hitSFX;        // Som de levar dano
    [SerializeField] private AudioClip deathSFX;      // Som de morrer
    [SerializeField] private GameObject explosionPrefab; // Partacula de explosao

    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        // Inicia a rotina de tiro automatica assim que o inimigo nasce
        StartCoroutine(ShootRoutine());
    }

    // --- LaGICA DE ATAQUE (COROUTINE) ---
    IEnumerator ShootRoutine()
    {
        // Espera um pouco antes do primeiro tiro para nao ser injusto
        yield return new WaitForSeconds(1.0f);

        while (true) // Loop infinito enquanto o inimigo estiver vivo
        {
            Shoot();
            // Pausa a execuaao desta rotina por X segundos
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void Shoot()
    {
        if (BulletPool.Instance == null) return;

        // USA O MÉTODO ESPECÍFICO PARA INIMIGOS
        BulletController bullet = BulletPool.Instance.GetEnemyBullet();

        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
    }

    // --- LaGICA DE DANO E MORTE ---

    // Matodo chamado pela bala do Player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Toca som de impacto (se ainda tiver vida)
        if (hitSFX != null && currentHealth > 0 && audioSource != null)
        {
            audioSource.PlayOneShot(hitSFX);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. Som de Morte
        // Usamos PlayClipAtPoint porque o objeto vai ser destruado imediatamente.
        // Se usassemos o audioSource normal, o som seria cortado no meio.
        if (deathSFX != null)
        {
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
        }

        // 2. Efeito Visual (Explosao)
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        // 3. Destruiaao do Objeto
        Destroy(gameObject);
    }
}
