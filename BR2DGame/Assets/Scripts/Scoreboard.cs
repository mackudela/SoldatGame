using Photon.Pun;
using TMPro;
using UnityEngine;

/// <summary>
/// Klasa Scoreboard realizuj�ca abstrakt tablicy wynik�w 
/// </summary>
public class Scoreboard : MonoBehaviour
{
    /// <summary>
    /// Zmienna przechowuj�ca referencj� do obiektu tablicy wynik�w
    /// </summary>
    public GameObject scoreboard;
    /// <summary>
    /// Zmienna przechowuj�ca referencj� do obiektu realizujacego przechowywanie wynik�w
    /// </summary>
    public GameObject scoreEntry;

    /// <summary>
    /// Metoda Start wywo�ywana przed pierwsz� aktualizacj� klatki. Przechowuje wywo�ania metod oraz inicjalizacje zmiennych.
    /// Stworzenie instancji obiektu newScoreEntry, pobranie danych oraz zapis na tablic� wynik�w
    /// </summary>
    void Start()
    {
        scoreboard.SetActive(false);

        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject newScoreEntry = Instantiate(scoreEntry, scoreboard.transform);
            TMP_Text[] texts = newScoreEntry.GetComponentsInChildren<TMP_Text>();

            texts[0].text = "9th";
            texts[1].text = player.NickName;
            texts[2].text = "99";
            texts[3].text = "59:59";
        }
    }

    /// <summary>
    /// Metoda update realizuj�ca pokazanie tablicy wynik�w na ��danie gracza, po wci�ni�ciu przycisku tab
    /// </summary>
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else
        {
            scoreboard.SetActive(false);
        }   
    }
}
