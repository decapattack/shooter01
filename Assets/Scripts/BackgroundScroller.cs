using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Configurações")]
    // Velocidade do movimento. Negativo move para baixo (trazendo o fundo para perto)
    [SerializeField] private float scrollSpeed = 0.5f;

    private MeshRenderer meshRenderer;
    private Material myMaterial;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // Pega uma cópia do material para podermos mexer sem estragar o original na pasta
        myMaterial = meshRenderer.material;
    }

    void Update()
    {
        // Calcula o deslocamento baseado no tempo
        // O operador % 1 (módulo) mantém o valor sempre entre 0 e 1 para precisão matemática,
        // evitando que o número fique gigante após horas de jogo.
        float offset = (Time.time * scrollSpeed) % 1;

        // No URP, a textura principal chama-se "_BaseMap"
        // Criamos um novo Vector2(X, Y). Mexemos só no Y.
        // Se quiser inverter a direção, coloque 'offset' ou '-offset'
        myMaterial.SetTextureOffset("_BaseMap", new Vector2(0, -offset));
    }
}
