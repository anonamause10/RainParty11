using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


//[ExecuteInEditMode]
public class GameSetupController : MonoBehaviourPun
{
    Dictionary<int, string> characterDict;
    [SerializeField] private Texture2D map;
    private GameObject[,] tiles;
    public float mult;
    // Start is called before the first frame update

    private void Awake() {
        PlaceTiles();
    }

    void Start()
    {
        characterDict = new Dictionary<int, string>();
        characterDict.Add(0,"TT_demo_female 2");
        characterDict.Add(1,"TT_demo_female");
        characterDict.Add(2,"TT_demo_male_A 2");
        characterDict.Add(3,"TT_demo_male_A");
        characterDict.Add(4,"TT_demo_male_B 2");
        characterDict.Add(5,"TT_demo_male_B");
        characterDict.Add(6,"TT_demo_police 2");
        characterDict.Add(7,"TT_demo_police");
        characterDict.Add(8,"TT_demo_zombie 2");
        characterDict.Add(9,"TT_demo_zombie");
        PlaceTiles();
        CreatePlayer();
    }

    private void PlaceTiles(){
        if(GameObject.Find("GameTile(Clone)")){
            return;
        }
        tiles = new GameObject[map.width,map.height];
        Material material = null;
        for (int i = 0; i < map.width; i++)
        {
            for (int j = 0; j < map.height; j++)
            {
                GameObject newTile = null;
                if(map.GetPixel(i,j)==Color.white){
                    continue;
                }
                if(PhotonNetwork.IsConnected){
                    newTile = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GameTile"), new Vector3(i*2.2f,0,j*2.2f) + (2.2f*15/2)*Vector3.left, Quaternion.identity);
                }else{
                    newTile = Instantiate((GameObject)Resources.Load("PhotonPrefabs/GameTile"), transform);
                }
                newTile.transform.position = new Vector3(i*2.2f,0,j*2.2f) + ((2.2f*15/2)-1)*Vector3.left;
                material = newTile.GetComponent<Renderer>().material;
                material.color = map.GetPixel(i,j);
                material.SetColor("_EmissionColor", map.GetPixel(i,j)*mult);
                tiles[i,j] = newTile;
            }
        }
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        if(!PhotonNetwork.IsConnected){
            Instantiate((GameObject)Resources.Load("PhotonPrefabs/TT_demo_female"), Vector3.zero, Quaternion.Euler(0,180,0));
            Instantiate((GameObject)Resources.Load("PhotonPrefabs/Hand"), Vector3.zero, Quaternion.Euler(0,180,0));
            Instantiate((GameObject)Resources.Load("PhotonPrefabs/Deck"), new Vector3(20,0,15), Quaternion.Euler(0,0,0));
        }
        if(PhotonNetwork.NickName==""){
            return;
        }
        string num = PhotonNetwork.NickName.Split('|')[1];
        if(num == ""){
            num = "0";
        }
        int randnum = int.Parse(num);
        PhotonNetwork.NickName = PhotonNetwork.NickName.Split('|')[0];
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", characterDict[randnum]), Vector3.zero, Quaternion.Euler(0,180,0));
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Hand"), Vector3.zero, Quaternion.Euler(0,180,0));
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Deck"), new Vector3(20,0,15), Quaternion.Euler(0,0,0));
        }
        
    }
}
