using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;
    [SerializeField] private InputField inputField;
    [SerializeField] private InputField roomField;
    [SerializeField] private int roomSize;
    [SerializeField] private CharSelect charSelect;
    

    private bool connected;
    private bool starting;
    private string roomName;

    // Callback function for when first connection established
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        connected = true;
        buttonText.text = "Begin Game";
    }

    public void GameButton()
    {
        if (connected)
        {
            if (!starting)
            {
                starting = true;
                buttonText.text = "Starting Game. Click Again to Cancel";
                PhotonNetwork.JoinRoom(roomName); // attempt joining a room
            }
            else
            {
                starting = false;
                buttonText.text = "Begin Game";
                PhotonNetwork.LeaveRoom(); // cancel the request
            }
        }
        else
            Debug.Log("Not connected to server!");
    }

    public void SetPlayerName()
    {
        string name = inputField.text + "|" + charSelect.GetIndex();
        button.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        string playerName = inputField.text + "|" + charSelect.GetIndex();

        PhotonNetwork.NickName = playerName;

        PlayerPrefs.SetString("PlayerName", playerName);

    }

    public void SetRoomNum()
    {
        roomName = roomField.text;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room... creating room");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps); 
        Debug.Log(roomName); 
    }

    public override void OnCreateRoomFailed(short returnCode, string message) 
    {
        Debug.Log("Failed to create room... trying again");
        CreateRoom();
    }
}