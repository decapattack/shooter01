using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    //[SerializeField] private float damage = 1f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Impulso para frente (na direção que o inimigo estava olhando)
        rb.velocity = transform.forward * speed;

        // Destruir após 5 segundos para não sujar a memória
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        // Se bater no Player
        if (other.CompareTag("Player"))
        {
            // Busca o script do Player para causar dano
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(); // Vamos criar este método já já
            }
            Destroy(gameObject); // Destroi a bala
        }
        // Se bater no chão ou obstáculos (mas ignora o próprio inimigo e outras balas)
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Untagged"))
        {
             Destroy(gameObject);
        }
    }
}
