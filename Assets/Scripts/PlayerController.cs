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
    [SerializeField] private float fireRate = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip shootSFX;
    private AudioSource audioSource;

    private Rigidbody rb;
    private Vector2 movementInput;
    private float nextFireTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 1. Leitura de Movimento
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(moveX, moveZ).normalized;

        // 2. Rotação (8 Direções)
        if (movementInput != Vector2.zero)
        {
            Vector3 lookDirection = new Vector3(movementInput.x, 0f, movementInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            transform.rotation = targetRotation;
        }

        // 3. Disparo
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
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
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Toca o som
            if (audioSource != null && shootSFX != null)
            {
                audioSource.PlayOneShot(shootSFX);
            }
        }
    }

    // --- SISTEMA DE DANO ---

    // Chamado quando o inimigo encosta fisicamente (Colisão)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    // Chamado quando a BALA do inimigo acerta (Trigger)
    public void TakeDamage()
    {
        Die();
    }

    // A FUNÇÃO QUE FALTAVA
    void Die()
    {
        Debug.Log("GAME OVER! Reiniciando...");

        // Reinicia a cena atual
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}