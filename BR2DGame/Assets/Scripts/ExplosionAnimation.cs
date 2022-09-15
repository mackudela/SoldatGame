using Photon.Pun;
using UnityEngine;

/// <summary>
/// Klasa ExplosionAnimation realizuj�ca abstrakt animacji wybuchu
/// </summary>
public class ExplosionAnimation : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj�ca referencj� do obiektu animacji 
    /// </summary>
    public GameObject animation;
    /// <summary>
    /// Zmienna przechowuj�ca promie� animacji
    /// </summary>
    public float radius = 0.2f;

    /// <summary>
    /// Metoda Start wywo�ywana przed pierwsz� aktualizacj� klatki. 
    /// Przechowuje wywo�ania metod oraz inicjalizacje zmiennych.
    /// </summary>
    private void Start()
    {
        //pobranie pozycji startu animacji
        Vector2 position = getRandomPosition();
        this.transform.position = position;
        this.transform.rotation = randomizeExplosionRotation();
    }

    /// <summary>
    /// Metoda realizuj�ca losowanie pozycji wywo�ania animacji, dzi�ki czemu efekt chaotyczny jest mniej przewidywalny i bardziej naturalny
    /// </summary>
    /// <returns>Zwraca wektor z wylosowan� pozycj� animacji</returns>
    protected Vector2 getRandomPosition()
    {
        return Random.insideUnitCircle * radius + (Vector2)transform.position;
    }

    /// <summary>
    /// Metoda realizuj�ca losowanie rotacji wywo�anej animacji, dzi�ki czemu efekt chaotyczny jest mniej przewidywalny i bardziej naturalny
    /// </summary>
    /// <returns>Zwraca kwaternion z wylosowan� rotacj� animacji</returns>
    protected Quaternion randomizeExplosionRotation()
    {
        return Quaternion.Euler(0,0,Random.Range(0,360));
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC realizuj�ca dekontrukcje animacji
    /// </summary>
    [PunRPC]
    public void destroyAnimation()
    {
        Destroy(this.animation);
    }

    /// <summary>
    /// Metoda rysuj�ca sfer� 
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
