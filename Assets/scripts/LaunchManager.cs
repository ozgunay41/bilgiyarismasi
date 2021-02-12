using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon.StructWrapping;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    public InputField nameInputField;

    [Header("Login Panel")]
    public GameObject loginPanel;
    public GameObject[] loginPanelObjects;

    [Header("Game Settings Panel")]
    public GameObject gameSettingPanel;
    public GameObject[] gameSettingPanelObjects;
    public Text scoreTxt, nicknameTxt;

    [Header("Join Room Panel")]
    public GameObject joinRoomPanel;
    public GameObject[] joinRoomPanelObjects;
    public Text randomRoomText; 


    void Start()
    {
        PlayerPrefs.SetInt("puan", 0);
        Debug.Log(PlayerPrefs.GetInt("score"));
        if (PlayerPrefs.HasKey("nickname"))
        {
            if(PhotonNetwork.CurrentRoom!=null)
                PhotonNetwork.LeaveRoom();

            
            PhotonNetwork.Disconnect();
            ActiveGameSettingsPanel();
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = PlayerPrefs.GetString("nickname");
            nicknameTxt.text = PlayerPrefs.GetString("nickname");
            scoreTxt.text = "Score: "+PlayerPrefs.GetInt("score");

            
        }
        else
        {
            ActiveLoginPanel();
        }

        
    }

    void Update()
    {

    }
    #region PhotonCallbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("photona bağlandı");
        Debug.Log(PhotonNetwork.NickName);
        ActiveGameSettingsPanel(); 
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SSTools.ShowMessage("Rakip ayrıldı.", SSTools.Position.bottom, SSTools.Time.threeSecond);
    }
    public override void OnConnected()
    {
        Debug.Log("internete bağlandı");
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount==2)
        SceneManager.LoadScene("New Scene");
    }
    public override void OnPlayerEnteredRoom(Player rival)
    {
       /* foreach (var player in PhotonNetwork.PlayerList)
        {
            player.CustomProperties.Add("score", 0);
        }*/

        SceneManager.LoadScene("New Scene");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        Debug.Log("Oda kuruluyor...");
        randomJoinRoom();
        Debug.Log(PhotonNetwork.CurrentRoom);
    }
    #endregion
 
    #region Unity Methods
   public void ActiveLoginPanel()
    {
        loginPanel.SetActive(true);
        gameSettingPanel.SetActive(false);

    }

    public void ActiveGameSettingsPanel()
    {
        loginPanel.SetActive(false);
        gameSettingPanel.SetActive(true);
    }

    public void ActiveJoinRandomRoomPanel()
    {
        gameSettingPanel.SetActive(false);
        joinRoomPanel.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        randomRoomText.text = "Rakip Aranıyor...";
    }
    public void onGiris()
    {
        PhotonNetwork.NickName = nameInputField.text;
        PlayerPrefs.SetString("nickname", nameInputField.text);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("puan", 0);
        PhotonNetwork.ConnectUsingSettings();
        nicknameTxt.text = PlayerPrefs.GetString("nickname");
        PlayerPrefs.SetInt("score", 0);
        scoreTxt.text = "Score: " + PlayerPrefs.GetInt("score");
    }
    public void randomJoinRoom()
    {
        gameSettingPanel.SetActive(false);

        string randomRoomName = "Room" + Random.Range(1, 100);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
        
       
    }
   
    #endregion
}
