using Photon.Pun;
using UnityEngine;

/// <summary>
/// Klasa Shooting reprezentuj�ca abstrakt kontrolera oddawania strza�u
/// </summary>
public class Shooting : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj�ca pozycj� oraz rotacj� lufy broni
    /// </summary>
    [SerializeField] private Transform firePoint;
    /// <summary>
    /// Zmienna przechowuj�ca prefab pocisku
    /// </summary>
    [SerializeField] private GameObject bulletPrefab;
    /// <summary>
    /// Zmienna kontroluj�ca czas op�nienia po strzale
    /// </summary>
    [SerializeField] static private float shotCooldown = 0.1f;
    /// <summary>
    /// Zmienna okre�laj�ca rozmiar magazynka karabinu szturmowego
    /// </summary>
    [SerializeField] private int magazineSize = 30;

    /// <summary>
    /// Referencja do obiektu karabinu szturmowego
    /// </summary>
    [SerializeField] private GameObject ak;
    /// <summary>
    /// Referencja do obiektu pistoletu
    /// </summary>
    [SerializeField] private GameObject pistol;

    float timeStamp = 0;
    float timeStamp2 = 0;

    PhotonView pv;

    /// <summary>
    /// Metoda Start wywo�ywana przed pierwsz� aktualizacj� klatki. 
    /// </summary>
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }


    /// <summary>
    /// Metoda Update wywo�ywana po ka�dej klatce, kontroluje wp�yw logiki gry na stan obiektu
    /// </summary>
    void Update()
    {
        if (ak.activeInHierarchy && Input.GetButton("Fire1")&& pv.IsMine)
        {
            if((timeStamp <= Time.time)&&(magazineSize>0))
            {
                Shoot();
                timeStamp = Time.time + shotCooldown;
                magazineSize--;
            }
        }
        else if(pistol.activeInHierarchy && Input.GetButtonDown("Fire1") && pv.IsMine)
        {
            Shoot();
        }

    }

    //function realizing releasing the bullet from barell
    /// <summary>
    /// Metoda synchronizowana PunRPC obs�uguj�ca logik� oddania strza�u z broni
    /// </summary>
    [PunRPC]
    void Shoot()
    {
        //Utworzenie instancji pocisku
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.transform.position, firePoint.transform.rotation);

    }
}
