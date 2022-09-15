using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// Klasa meleeAttack realizuj�ca abstrakt walki wr�cz
/// </summary>
public class meleeAttack : MonoBehaviour
{
    /// <summary>
    /// Referencja do komponentu animatora
    /// </summary>
    [SerializeField] Animator animator;

    /// <summary>
    /// Metoda Start wywo�ywana przed pierwsz� aktualizacj� klatki. Uruchamia animacj� ataku wr�cz
    /// </summary>
    void Start()
    {
        animator.Play("MeleeAttack");
    }

    /// <summary>
    /// W metodzie Awake, kontrolowanie animacji, uruchomienie korutyny odmierzaj�cej czas animacji
    /// </summary>
    private void Awake() {
        StartCoroutine("DestroyByTime");
    }

    /// <summary>
    /// Korutyna kontroluj�ca czas �ycia animacji.
    /// </summary>
    /// <returns>obiekt IEnumerator</returns>
    IEnumerator DestroyByTime() {
        yield return new WaitForSeconds(2f);
        this.GetComponent<PhotonView>().RPC("destroyAnimation", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Metoda synchronizowana PunRPC inicjuj�ca dekonstrukcje obiektu animacji
    /// </summary>
    [PunRPC]
    public void destroyAnimation() {
        Destroy(this.gameObject);
    }
}
