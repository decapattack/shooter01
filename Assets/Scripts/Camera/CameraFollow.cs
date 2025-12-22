using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform target; // Quem a câmera vai seguir (Player)
	[SerializeField] private float smoothSpeed = 5f; // Suavidade do movimento (Lerp)
	[SerializeField] private Vector3 offset; // A distância original (ex: 10m de altura)

	void Start()
	{
		// Calcula a distância atual entre a câmera e o player automaticamente no início
		if (target != null)
		{
			offset = transform.position - target.position;
		}
	}

	// LateUpdate roda APÓS todos os Updates normais.
	// Isso garante que a câmera só se mova DEPOIS que o player terminou de se mover no frame.
	// Evita tremedeiras (jitter).
	void LateUpdate()
	{
		if (target == null) return;

		// Posição desejada = Posição do Player + Distância original
		Vector3 desiredPosition = target.position + offset;

		// Lerp (Linear Interpolation) faz o movimento ser suave em vez de instantâneo.
		// Vai da posição atual para a desejada com velocidade 'smoothSpeed'.
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

		transform.position = smoothedPosition;
	}
}