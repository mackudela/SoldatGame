using Photon.Pun;
using UnityEngine;

/// <summary>
/// Klasa SpawnPlayers reazliuj�ca abstrakt kontrolera inicjuj�cego obiekty graczy na planszy
/// </summary>
public class SpawnPlayers : MonoBehaviour
{
    /// <summary>
    /// Referencja do obiektu prefaba gracza
    /// </summary>
    [SerializeField] private GameObject playerPrefab;

    /// <summary>
    /// Zmienne przechowuj�ce koordynaty na kt�rych gracz zostanie utworzony
    /// </summary>
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    /// <summary>
    /// Zmienna przechowuj�ca obiekt lokalnego gracza
    /// </summary>
    private static GameObject localPlayer = null;

    /// <summary>
    /// Metoda zwracaj�ca obiekt lokalnego gracza
    /// </summary>
    /// <returns></returns>
    public static GameObject getLocalPlayerReference() { return localPlayer;  }

    /// <summary>
    /// W metodzie start utworzenie nowej instancji gracza na wylosowanej pozycji na mapie, na wszystkich zalogowanych komputerach
    /// </summary>
    void Start() {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        localPlayer = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }

}
