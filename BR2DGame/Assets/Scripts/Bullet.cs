using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// Klasa Bullet realizuj�ca abstrakt standardowego pocisku
/// </summary>
public class Bullet : MonoBehaviourPun
{
    /// <summary>
    /// Zmienna przechowuj�ca referencj� do obiektu strzelaj�cego
    /// </summary>
    [SerializeField] private GameObject shooter;
    /// <summary>
    /// Zmienna przechowuj�ca czas �ycia pocisku
    /// </summary>
    [SerializeField] private float destroyTime;
    /// <summary>
    /// Zmienna przechowuj�ca obra�enia zadawane poprzez trafienie pociskiem
    /// </summary>
    [SerializeField] private float damage;
    /// <summary>
    /// Zmienna przechowuj�ca si�� nadawan� pociskowi
    /// </summary>
    [SerializeField] private float bulletForce;
    [SerializeField] PhotonView pv;

    private Rigidbody2D bulletRigidBody;

    /// <summary>
    /// Zmienna przechowuj�ca po�o�enie oraz rotacj� lufy broni, z kt�rej pocisk jest wystrzeliwany
    /// </summary>
    private Transform firePoint;

    /// <summary>
    /// Metoda Start wywo�ywana przed pierwsz� aktualizacj� klatki. 
    /// Przechowuje wywo�ania metod oraz inicjalizacje zmiennych.
    /// </summary>
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        bulletRigidBody = this.GetComponent<Rigidbody2D>();
        //Dodanie si�y do RigidBody pocisku w celu nadania ruchu
        bulletRigidBody.AddForce(this.transform.up * bulletForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// W metodzie Awake, kontrolowanie fizyki pocisku - uruchomienie korutyny odmierzaj�cej czas �ycia pocisku
    /// </summary>
    private void Awake()
    {
        StartCoroutine("DestroyByTime");
    }

    /// <summary>
    /// Korutyna odmierzaj�ca czas �ycia pocisku. Po up�yni�ciu czasu �ycia obiekt pocisku jest dekontruuowany
    /// </summary>
    /// <returns>obiekt IEnumerator</returns>
    IEnumerator DestroyByTime()
    {
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
        if (destroyable != null)
        {
            destroyable.TakeDamage(damage);
            hit = true;
        }

        //logika po trafieniu beczki
        if (barrel != null) {
            barrel.TakeDamage(damage);
            hit = true;
        }

        //logika po trafieniu w obiekt gracza
        if ((playerBody != null)&&(!collision.gameObject.GetPhotonView().IsMine))
        {
            playerBody.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damage);
            hit = true;
        }

        //logika po trafieniu pocisku
        if (hit)
        {
            StopCoroutine("DestroyByTime");
            this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Metoda realizuj�ca logik� gry po wej�ciu w kolizj� z obiektem �ciany
    /// </summary>
    /// <param name="collision">Kolizjator obiektu z kt�rym zachodzi kolizja</param>
    private void OnCollisionEnter2D(Collision2D collision) {
        Wall wall = collision.gameObject.GetComponent<Wall>();
        if (collision.gameObject.tag == "wall") {
            //dekonstrukcja pocisku
            StopCoroutine("DestroyByTime");
            this.GetComponent<PhotonView>().RPC("destroyBullet", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC odpowiedzialna za destrukcj� obiektu pocisku
    /// </summary>
    [PunRPC]
    public void destroyBullet()
    {
        Destroy(this.gameObject);
    }
}
