using UnityEngine;
using UnityEngine.Pool; // Namespace oficial da Unity para Pooling

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance; // Singleton simples

    [Header("Configuração")]
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    // A estrutura de dados mágica da Unity
    private ObjectPool<BulletController> _pool;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Inicializa o Pool definindo as 4 ações vitais:
        // 1. Create: Como instanciar se o pool estiver vazio?
        // 2. ActionOnGet: O que fazer ao pegar um objeto (resetar)?
        // 3. ActionOnRelease: O que fazer ao devolver (desativar)?
        // 4. ActionOnDestroy: O que fazer se o pool lotar e precisar jogar fora?
        _pool = new ObjectPool<BulletController>(
            createFunc: () => Instantiate(bulletPrefab),
            actionOnGet: (bullet) => bullet.gameObject.SetActive(true),
            actionOnRelease: (bullet) => bullet.gameObject.SetActive(false),
            actionOnDestroy: (bullet) => Destroy(bullet.gameObject),
            collectionCheck: false,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    // API pública para pedir uma bala
    public BulletController Get()
    {
        return _pool.Get();
    }

    // API pública para devolver a bala
    public void Release(BulletController bullet)
    {
        _pool.Release(bullet);
    }
}