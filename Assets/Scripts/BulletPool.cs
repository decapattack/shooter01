using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [Header("Prefabs")]
    [SerializeField] private BulletController playerBulletPrefab;
    [SerializeField] private BulletController enemyBulletPrefab;

    // Duas piscinas separadas
    private ObjectPool<BulletController> _playerPool;
    private ObjectPool<BulletController> _enemyPool;

    void Awake()
    {
        Instance = this;
        InitializePools();
    }

    void InitializePools()
    {
        // Pool de Balas do Jogador (Azul)
        _playerPool = new ObjectPool<BulletController>(
            createFunc: () => Instantiate(playerBulletPrefab),
            actionOnGet: (b) => b.gameObject.SetActive(true),
            actionOnRelease: (b) => b.gameObject.SetActive(false),
            actionOnDestroy: (b) => Destroy(b.gameObject),
            defaultCapacity: 20, maxSize: 100
        );

        // Pool de Balas do Inimigo (Vermelha)
        _enemyPool = new ObjectPool<BulletController>(
            createFunc: () => Instantiate(enemyBulletPrefab),
            actionOnGet: (b) => b.gameObject.SetActive(true),
            actionOnRelease: (b) => b.gameObject.SetActive(false),
            actionOnDestroy: (b) => Destroy(b.gameObject),
            defaultCapacity: 20, maxSize: 100
        );
    }

    // --- API PÚBLICA ---

    public BulletController GetPlayerBullet()
    {
        var bullet = _playerPool.Get();
        bullet.SetPool(_playerPool); // Ensina a bala a voltar para casa
        return bullet;
    }

    public BulletController GetEnemyBullet()
    {
        var bullet = _enemyPool.Get();
        bullet.SetPool(_enemyPool); // Ensina a bala a voltar para casa
        return bullet;
    }
}