using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	[SerializeField] private float speed = 60f;
	[SerializeField] private float lifeTime = 2f; // Tempo de vida em segundos

	void Start() // Start roda uma vez, logo antes do primeiro Update
	{
		// Destroy agendado: Remove o objeto da memoria apos 3 segundos
		// Isso evita vazar memoria com milhares de tiros voando para o infinito.
		Destroy(gameObject, lifeTime);
	}

	void Update()
	{
		// Move o tiro para "Frente" (Eixo Z positivo local) e noo global
		// Space.Self garante que se girarmos o tiro, ele vai para a frente dele
		transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
	}

	// Detectar colisoo (veremos no proximo passo)
	// Callback mogico da Unity: Dispara quando o Collider (Trigger) toca em outro Collider
	void OnTriggerEnter(Collider other)
	{
		// --- LINHA DE DEBUG NOVA ---
		// Vai imprimir o Nome e a Tag do objeto que a bala tocou.
		Debug.Log($"Bala tocou em: {other.name} | Tag: {other.tag}");
		// ---------------------------

		if (other.CompareTag("Enemy"))
		{
			EnemyController enemy = other.GetComponent<EnemyController>();
			if (enemy != null)
			{
				enemy.TakeDamage(1);
			}
			Destroy(gameObject);
		}
		else if (!other.CompareTag("Player") && !other.CompareTag("Untagged"))
		{
			Destroy(gameObject);
		}
	}
}
