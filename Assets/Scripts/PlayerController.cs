using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 5f;

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
        if (bulletPrefab == null || firePoint == null) return;

        // Tiro simples para frente
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        PlayShootSound();
    }

    // --- NOVO MÉTODO: O TIRO EM 4 DIREcoES ---
    void SpecialShoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Array com os angulos desejados relativos a frente da nave
        float[] angles = { 0f, 45f, -45f, 180f };

        foreach (float angle in angles)
        {
            // Quaternion.Euler(x, y, z) cria uma rotacao baseada em graus.
            // Multiplicamos a rotacao ATUAL da nave pela rotacao do DESVIO.
            Quaternion rotationOffset = Quaternion.Euler(0f, angle, 0f);
            Quaternion finalRotation = transform.rotation * rotationOffset;

            Instantiate(bulletPrefab, firePoint.position, finalRotation);
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