using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// Klasa Bullet realizuj�ca abstrakt pocisku eksploduj�cego
/// </summary>
public class ExploBullet : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj�ca referencj� do obiektu strzelaj�cego
    /// </summary>
    [SerializeField] private GameObject shooter;
    /// <summary>
    /// Zmienna przechowuj�ca czas �ycia pocisku
    /// </summary>
    [SerializeField] private float destroyTime = 2f;
    /// <summary>
    /// Zmienna przechowuj�ca obra�enia zadawane poprzez trafienie pociskiem
    /// </summary>
    [SerializeField] private float damage;
    /// <summary>
    /// Zmienna przechowuj�ca si�� nadawan� pociskowi
    /// </summary>
    [SerializeField] private float bulletForce = 50f;
    [SerializeField] PhotonView pv;
    [SerializeField] private float splashRange;
    [SerializeField] private GameObject animationPrefab;

    private Rigidbody2D bulletRigidBody;

    /// <summary>
    /// Zmienna przechowuj�ca po�o�enie oraz rotacj� lufy broni, z kt�rej pocisk jest wystrzeliwany
    /// </summary>
    private Transform firePoint;

    /// <summary>
    /// Metoda Start wywo�ywana przed pierwsz� aktualizacj� klatki. 
    /// Przechowuje wywo�ania metod oraz inicjalizacje zmiennych.
    /// </summary>
    private void Start() {
        pv = GetComponent<PhotonView>();
        bulletRigidBody = this.GetComponent<Rigidbody2D>();
        //Dodanie si�y do RigidBody pocisku w celu nadania ruchu
        bulletRigidBody.AddForce(this.transform.up * bulletForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// W metodzie Awake, kontrolowanie fizyki pocisku - uruchomienie korutyny odmierzaj�cej czas �ycia pocisku
    /// </summary>
    private void Awake() {
        StartCoroutine("DestroyByTime");
    }

    /// <summary>
    /// Korutyna odmierzaj�ca czas �ycia pocisku. Po up�yni�ciu czasu �ycia obiekt pocisku jest dekontruuowany
    /// </summary>
    /// <returns>obiekt IEnumerator</returns>
    IEnumerator DestroyByTime() {
        yield return new WaitForSeconds(destroyTime);
        this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Metoda realizuj�ca logik� kolizji pocisku z obiektami na mapie
    /// </summary>
    /// <param name="collision">Kolizjator obiektu z kt�rym zachodzi kolizja</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        bool hit = false;
        Box destroyable = collision.GetComponent<Box>();
        Barrel barrel = collision.GetComponent<Barrel>();
        Player playerBody = collision.GetComponent<Player>();
        Wall wall = collision.GetComponent<Wall>();

        //logika po trafieniu w skrzynk�
        if (destroyable != null) {
            destroyable.TakeDamage(10f);
            hit = true;
        }

        //logika po trafieniu beczki
        if (barrel != null) {
            barrel.TakeDamage(10f);
            hit = true;
        }

        //logika po trafieniu w obiekt gracza
        if ((playerBody != null) && (!collision.gameObject.GetPhotonView().IsMine)) {
            playerBody.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 10f);
            hit = true;
        }

        //logika po trafieniu pocisku - wywo�anie eksplozji
        if (hit) {
            explode();
            StopCoroutine("DestroyByTime");
            this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Metoda realizuj�ca logik� gry po wej�ciu w kolizj� z obiektem �ciany
    /// </summary>
    /// <param name="collision">Kolizjator obiektu z kt�rym zachodzi kolizja</param>
    private void OnCollisionEnter2D(Collision2D collision) {
        //Wall wall = collision.gameObject.GetComponent<Wall>();
        if (collision.gameObject.tag == "wall") {
            explode();
            StopCoroutine("DestroyByTime");
            this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Metoda realizuj�c logik� gry zwi�zan� z eksplodowaniem pocisku. Zadanie obra�e� obiektom znajduj�cym si� w obszarze eksplozji.
    /// </summary>
    public void explode() {
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);
        foreach (var hitCollider in hitColliders) {
            Player player = hitCollider.GetComponent<Player>();
            Box box = hitCollider.GetComponent<Box>();
            Barrel barrel = hitCollider.GetComponent<Barrel>();
            if (player) {
                player.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage);
              
            }
            if (box) {
                box.TakeDamage(damage);
            }
            if (barrel) {
                barrel.TakeDamage(damage);
            }
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC odpowiedzialna za destrukcj� obiektu pocisku oraz uruchomienie animacji eksplozji
    /// </summary>
    [PunRPC]
    public void destroyBullet() {
        bulletExplosion();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Metoda odpowiedzialna za uruchomienie animacji eksplozji
    /// </summary>
    public void bulletExplosion()
    {
        GameObject explosionAnimation = PhotonNetwork.Instantiate(animationPrefab.name, this.transform.position, this.transform.rotation);
    }
}
