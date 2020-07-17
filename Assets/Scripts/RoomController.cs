using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int multiplayerSceneIndex;
    [SerializeField] private Button startButton;
    [SerializeField] private Text playerList;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void Update() {
        string playerListString = "";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerListString += player.NickName + "\n";//.Split('|')[0]
            
        }

        playerList.text = playerListString;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room. Multiplayer game has begun.");
        ReadyGame();
    }

    private void ReadyGame(){
        startButton.interactable = true;
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }
}