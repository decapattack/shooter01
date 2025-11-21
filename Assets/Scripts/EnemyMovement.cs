using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Configuração")]
    [SerializeField] private float speed = 3.5f; // Velocidade de perseguição
    [SerializeField] private float stopDistance = 5f; // Para ele não colar em você (Fica atirando de longe)

    private Transform player;

    void Start()
    {
        // Busca automática: Encontra o objeto que tem a Tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return; // Se o player morreu, o inimigo para.

        // 1. Calcular distância
        float distance = Vector3.Distance(transform.position, player.position);

        // 2. Olhar para o Player (Fundamental para o tiro sair na direção certa também!)
        // O "LookAt" roda o inimigo para que o eixo Z dele aponte para o alvo.
        transform.LookAt(player);

        // 3. Mover (apenas se estiver longe)
        if (distance > stopDistance)
        {
            // Move para a "frente" dele mesmo
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}
