using Photon.Pun;
using UnityEngine;

/// <summary>
/// Klasa Barrel reprezentuj�ca obstrakt beczki z materia�em, kt�ry po trafieniu eksploduje
/// </summary>
public class Barrel : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj�ca ilo�ci punkt�w �ycia beczki
    /// </summary>
    [SerializeField] private float health = 100;
    /// <summary>
    /// Zmienna przechowuj�ca zakres zadawania obra�e� przez eksplozje
    /// </summary>
    [SerializeField] private float splashRange;
    /// <summary>
    /// Zmienna przechowuj�ca obra�enia zadawane w wyniku eksplozji beczki
    /// </summary>
    [SerializeField] private float damage = 100;
    /// <summary>
    /// Referencja do obiektu animacji wybuchu beczki
    /// </summary>
    [SerializeField] private GameObject animationPrefab;
    /// <summary>
    /// Zmienna przechowuj�ca liczb� zachodz�cych animacji
    /// </summary>
    int animationCounter = 1;

    /// <summary>
    /// Metoda realizuj�ca logik� otrzymywania przez obiekt beczki obra�e� od trafienia pociskiem
    /// </summary>
    /// <param name="damage">Obra�enia otrzymane w wyniku ataku</param>
    public void TakeDamage(float damage) {
        health -= damage;

        //Obs�uga logiki wybuchu beczki po utraceniu punkt�w �ycia
        if (health <= 0) {
            explode();
            this.GetComponent<PhotonView>().RPC("destroyBarrel", RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// Metoda realizuj�ca logik� eksplozji obiektu beczki oraz zadania graczom w wyniku eksplozji obra�e� w obszarze wybuchu
    /// </summary>
    public void explode() {
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRange);
        foreach (var hitCollider in hitColliders) {
            Player player = hitCollider.GetComponent<Player>();
            if (player) {
                player.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 100.00f);
            }
        }
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC wywo�uj�ca eksplozj� beczki oraz dekontruuj�ca obiekt beczki
    /// </summary>
    [PunRPC]
    public void destroyBarrel() 
    {
        barrelExplosion();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Metoda obs�ugujaca wywo�ywanie animacji wybuchu obiektu beczki
    /// </summary>
    public void barrelExplosion()
    {
        GameObject explosionAnimation = PhotonNetwork.Instantiate(animationPrefab.name,this.transform.position, this.transform.rotation);
    }
}
