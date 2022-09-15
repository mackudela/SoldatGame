using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Klasa MenuController realizuj�ca abstrakt kontrolera pocz�tkowej fazy gry, udost�pniaj�ca graczowi mo�liwo�� konfiguracji
/// parametr�w gry multiplayer.
/// </summary>
public class MenuController : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Zmienna przechowuj�ca wersj� gry
    /// </summary>
    [SerializeField] private string versionName = "0.0.1";
    /// <summary>
    /// Zmienna przechowuj�ca referencje do obiektu menu u�ytkownika
    /// </summary>
    [SerializeField] private GameObject usernameMenu;
    /// <summary>
    /// Zmienna przechowuj�ca referencje do obiektu panelu umo�liwiaj�cy nawi�zanie po��czenia
    /// </summary>
    [SerializeField] private GameObject connectPanel;
    /// <summary>
    /// Zmienna przechowuj�ca referencje do obiektu ekran sygnalizuj�cego ��czenie z serwerem
    /// </summary>
    [SerializeField] private GameObject connectingScreen;
    /// <summary>
    /// Zmienna przechowuj�ca referencje do pola tekstowego umo�liwiaj�ce nadaniu grze identyfikatora
    /// </summary>
    [SerializeField] private GameObject CreateGameInput;
    /// <summary>
    /// Zmienna przechowuj�ca referencje do obiektu przycisku do��czenia do gry
    /// </summary>
    [SerializeField] private GameObject JoinGameInput;
    /// <summary>
    /// Zmienna przechowuj�ca referencje do obiektu pola tekstowego do nadania graczowi przezwiska
    /// </summary>
    [SerializeField] private GameObject UsernameInput;
    /// <summary>
    /// Zmienna przechowuj�ca referencje do obiektu przycisku start
    /// </summary>
    [SerializeField] private GameObject StartButton;

    /// <summary>
    /// Metoda awake ustawiaj�ca wersj� gry oraz inicjuj�ca zadanie ustawie� dla po��czenia
    /// </summary>
    private void Awake() {
        Debug.Log("Connecting to server");
        PhotonNetwork.GameVersion = versionName;
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Metoda start odkrywaj�ce menu u�ytkownika
    /// </summary>
    private void Start() {
        usernameMenu.SetActive(true);
    }

    /// <summary>
    /// Metoda realizuj�ca do��czenie do poczekalni
    /// </summary>
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to server");
    }

    /// <summary>
    /// Metoda wywo�ywana w chwili roz��czenia si� z serwerem
    /// </summary>
    /// <param name="cause">Przyczyna roz��czenia si� z serwerem</param>
    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("Disconnected from server: " + cause.ToString());
    }

    /// <summary>
    /// Metoda wprowadzaj�ca nick gracza do programu
    /// </summary>
    public void ChangeUserNameInput() {

        if (UsernameInput.GetComponent<InputField>().text.Length >= 3) {
            StartButton.SetActive(true);
        }
        else {
            StartButton.SetActive(false);
        }
    }

    /// <summary>
    /// Metoda ustawiaj�ca nick gracza w rozgrywce
    /// </summary>
    public void SetUserName() {
        usernameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.GetComponent<InputField>().text;
        Debug.Log(UsernameInput.GetComponent<InputField>().text);
        connectPanel.SetActive(true);
    }

    /// <summary>
    /// Metoda obs�uguj�ca ekran ��czenia z serwerem
    /// </summary>
    public override void OnJoinedLobby() {
        PhotonNetwork.NickName = UsernameInput.GetComponent<InputField>().text;
        connectingScreen.SetActive(false);
    }

    /// <summary>
    /// Metoda realizuj�ca tworzenie nowego pokoju
    /// </summary>
    public void CreateRoom() {
        PhotonNetwork.CreateRoom(CreateGameInput.GetComponent<InputField>().text);
    }

    /// <summary>
    /// Metoda realizuj�ca do��czanie do nowego pokoju
    /// </summary>
    public void JoinRoom() {
        PhotonNetwork.JoinRoom(JoinGameInput.GetComponent<InputField>().text);
    }

    /// <summary>
    /// Metoda obs�uguj�ca zmian� sceny ze sceny poczekalni na scen� gry
    /// </summary>
    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("Game");
    }
}
