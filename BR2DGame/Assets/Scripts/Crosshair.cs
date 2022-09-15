using UnityEngine;

/// <summary>
/// Klasa Crosshair realizuj�ca abstrakt celownika widocznego w GUI gracza
/// </summary>
public class Crosshair : MonoBehaviour
{
    /// <summary>
    /// Referencja do obiektu celownika
    /// </summary>
    public GameObject crosshair;

    /// <summary>
    /// Metoda Start wywo�ywana przed pierwsz� aktualizacj� klatki. 
    /// Przechowuje wywo�ania metod oraz inicjalizacje zmiennych.
    /// </summary>
    void Start()
    {
        crosshair.SetActive(true);
        Cursor.visible = false;
    }

    /// <summary>
    /// W metodzie update obs�uga transformacji celownika zgodnie z kursorem myszy
    /// </summary>
    void Update()
    {
        crosshair.transform.position = Input.mousePosition;
    }
}
