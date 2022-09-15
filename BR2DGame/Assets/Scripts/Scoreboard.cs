using Photon.Pun;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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
    public GameObject scoreEntryGameObject;

    private class _scoreEntry
    {
        public string nickName;
        public string livingStatus;
        public int score;
    }

    private List<_scoreEntry> _scoreEntries = new List<_scoreEntry>();
    private GameObject [] arrayOfGameObjects = new GameObject[9];


    /// <summary>
    /// Metoda Start wywo�ywana przed pierwsz� aktualizacj� klatki. Przechowuje wywo�ania metod oraz inicjalizacje zmiennych.
    /// Stworzenie instancji obiektu newScoreEntry, pobranie danych oraz zapis na tablic� wynik�w
    /// </summary>
    void Start()
    {
        scoreboard.SetActive(false);
        CreateScoreboard();
    }

    /// <summary>
    /// Metoda update realizuj�ca pokazanie tablicy wynik�w na ��danie gracza, po wci�ni�ciu przycisku tab
    /// </summary>
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
            UpdateScoreboard();
        }
        else
        {
            scoreboard.SetActive(false);
        }   
    }

    private void CreateScoreboard() 
    {

        for (int i = 0; i < 9; i++)
        {
            arrayOfGameObjects[i] = Instantiate(scoreEntryGameObject, scoreboard.transform);
            TMP_Text[] texts = arrayOfGameObjects[i].GetComponentsInChildren<TMP_Text>();

            switch (i)
            {
                case 0:
                    texts[0].text = "1st";
                    break;

                case 1:
                    texts[0].text = "2nd";
                    break;

                case 2:
                    texts[0].text = "3rd";
                    break;

                default: texts[0].text = (i + 1).ToString() + "th";
                         break;

            }

            texts[1].text = "";//_scoreEntries.ElementAt(i).nickName;
            texts[2].text = "";//_scoreEntries.ElementAt(i).livingStatus;
            texts[3].text = "";//_scoreEntries.ElementAt(i).score.ToString();
        }

        
    }
    private void UpdateScoreboard()
    {
        _scoreEntries = new List<_scoreEntry>();

        foreach (var player in PhotonNetwork.PlayerList)
        {
            _scoreEntries.Add(new _scoreEntry()
            {
                nickName = player.NickName,
                livingStatus = (bool)player.CustomProperties["livingStatus"] ? "Alive" : "Dead",
                score = (int)player.CustomProperties["score"],
            });
        }

        _scoreEntries = _scoreEntries.OrderByDescending(scoreEntry => scoreEntry.score).Take(9).ToList();

        int rowsToUpdate = _scoreEntries.Count();

        for (int i = 0; i < rowsToUpdate; i++)
        {
            TMP_Text[] texts = arrayOfGameObjects[i].GetComponentsInChildren<TMP_Text>();

            texts[1].text = _scoreEntries.ElementAt(i).nickName;
            texts[2].text = _scoreEntries.ElementAt(i).livingStatus;
            texts[3].text = _scoreEntries.ElementAt(i).score.ToString();
            //Debug.Log("Scoreboard log: " + _scoreEntries.ElementAt(i).score.ToString());
        }

        for (int i = rowsToUpdate; i < 9; i++)
        {
            TMP_Text[] texts = arrayOfGameObjects[i].GetComponentsInChildren<TMP_Text>();

            texts[1].text = "";
            texts[2].text = "";
            texts[3].text = "";
        }
    }
}
