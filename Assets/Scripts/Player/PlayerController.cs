using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 15f;

    [Header("Combate")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.1f; // Tiro normal mais rápido
    [SerializeField] private float specialFireRate = 0.1f; // Tiro especial mais lento

    [Header("Audio")]
    [SerializeField] private AudioClip shootSFX;
    private AudioSource audioSource;

    private Rigidbody rb;
    private Vector2 movementInput;

    // Timestamps para controlar os cooldowns separadamente
    private float nextFireTime = 0f;
    private float nextSpecialFireTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 1. Movimento e Rotacao (Mantido igual)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveZ).normalized;

        if (movementInput != Vector2.zero)
        {
            Vector3 lookDirection = new Vector3(movementInput.x, 0f, movementInput.y);
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }

        // 2. Disparo Normal (Botao Esquerdo / Ctrl)
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        // 3. Disparo Especial (Botao Direito / Alt) <--- NOVO
        if (Input.GetButton("Fire2") && Time.time >= nextSpecialFireTime)
        {
            SpecialShoot();
            nextSpecialFireTime = Time.time + specialFireRate;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;
        Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y);
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        // Removemos a checagem de 'bulletPrefab' pois o Player não precisa mais saber dele
        if (firePoint == null) return;

        // Verificação de segurança caso esqueça de colocar o Manager na cena
        if (BulletPool.Instance == null) return;

        // 1. Pega a bala configurada para o Player (Azul) direto da piscina
        BulletController bullet = BulletPool.Instance.GetPlayerBullet();

        // 2. Posiciona onde deve nascer
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        PlayShootSound();
    }

    // --- NOVO MÉTODO: O TIRO EM 4 DIREcoES ---
    void SpecialShoot()
    {
        if (firePoint == null) return;
        if (BulletPool.Instance == null) return;

        // Definição dos ângulos extras
        float[] angles = { 0f, 45f, -45f, 180f };

        foreach (float angle in angles)
        {
            // Cálculo da rotação (Matemática de Quaternions)
            Quaternion rotationOffset = Quaternion.Euler(0f, angle, 0f);
            Quaternion finalRotation = transform.rotation * rotationOffset;

            // 1. Pega uma NOVA bala do pool para cada direção do loop
            // Isso é extremamente rápido e não gera lixo de memória (GC)
            BulletController bullet = BulletPool.Instance.GetPlayerBullet();

            // 2. Posiciona com a rotação calculada
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = finalRotation;
        }

        PlayShootSound();
    }

    void PlayShootSound()
    {
        if (audioSource != null && shootSFX != null)
        {
            audioSource.PlayOneShot(shootSFX);
        }
    }

    // --- SISTEMA DE DANO (Mantido igual) ---
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) Die();
    }

    public void TakeDamage()
    {
        Die();
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}