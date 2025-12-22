using UnityEngine;
using UnityEngine.Pool;

public class BulletController : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;

    // Quem essa bala deve machucar? ("Enemy" para balas do player, "Player" para balas do inimigo)
    [Tooltip("Tag do objeto que vai levar dano. Ex: 'Player' ou 'Enemy'")]
    [SerializeField] private string targetTag;

    private ObjectPool<BulletController> _myPool;

    // Substitui o Start() para funcionar com Pooling
    void OnEnable()
    {
        // Garante que o timer de morte seja reiniciado toda vez que a bala sai da piscina
        CancelInvoke(nameof(Deactivate));
        Invoke(nameof(Deactivate), lifeTime);
    }

    // Injeção de dependência: O Pool avisa a bala para onde ela deve voltar
    public void SetPool(ObjectPool<BulletController> pool)
    {
        _myPool = pool;
    }

    void Update()
    {
        // Move para frente (Z positivo local)
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        // 1. Acertou o Alvo Correto?
        if (other.CompareTag(targetTag))
        {
            // Lógica dinâmica: Verifica se é Player ou Inimigo
            if (targetTag == "Enemy")
            {
                var enemy = other.GetComponent<EnemyController>();
                if (enemy) enemy.TakeDamage(damage);
            }
            else if (targetTag == "Player")
            {
                var player = other.GetComponent<PlayerController>();
                if (player) player.TakeDamage();
            }

            Deactivate(); // Devolve para a piscina
        }
        // 2. Acertou parede/chão? (Ignora quem atirou para não morrer na saída)
        // Se a tag do objeto tocado NÃO for a tag do alvo E NÃO for a tag da própria bala
        else if (!other.CompareTag(targetTag) && !other.CompareTag(gameObject.tag) && !other.CompareTag("Untagged"))
        {
            // Opcional: Desativar ao bater em obstáculos do cenário
            // Deactivate(); 
        }
    }

    private void Deactivate()
    {
        if (_myPool != null)
        {
            _myPool.Release(this);
        }
        else
        {
            // Fallback se a bala estiver na cena sem pool (para testes)
            gameObject.SetActive(false);
            Destroy(gameObject, 0.1f); // Segurança
        }
    }
}