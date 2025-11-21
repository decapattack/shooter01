using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[Header("Configurações da Fábrica")]
	[SerializeField] private GameObject enemyPrefab; // O molde
	[SerializeField] private float spawnInterval = 2.0f; // Tempo entre nascimentos
	[SerializeField] private float spawnRadius = 10f; // Raio de distância para nascer

	[Header("Target")]
	[SerializeField] private Transform player; // Referência para não nascer em cima do player

	void Start()
	{
		// Inicia a rotina em paralelo (não bloqueia o jogo)
		StartCoroutine(SpawnRoutine());
	}

	// IEnumerator define que este método pode ser pausado e resumido
	IEnumerator SpawnRoutine()
	{
		// Loop infinito enquanto o objeto Spawner existir
		while (true)
		{
			SpawnEnemy();
			// Pausa esta linha por X segundos, depois continua
			yield return new WaitForSeconds(spawnInterval);
		}
	}

	void SpawnEnemy()
	{
		if (enemyPrefab == null) return;

		// Gera uma posição aleatória dentro de um círculo de raio 'spawnRadius'
		Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;

		// Converte o ponto 2D (X, Y) para 3D (X, 0, Z)
		// Somamos à posição do Spawner para ser relativo ao centro
		// Mudamos o Y de 1f para 0f
		Vector3 spawnPos = new Vector3(randomPoint.x, 0f, randomPoint.y) + transform.position;

		// Instancia o inimigo
		Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
	}

	// Desenha um gizmo no editor para vermos a área de spawn (Debug Visual)
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, spawnRadius);
	}
}