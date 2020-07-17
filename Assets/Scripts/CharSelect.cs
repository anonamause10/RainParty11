using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelect : MonoBehaviour
{
    Dictionary<int, string> characterDict;
    private int index;
    [SerializeField] private GameObject character;
    // Start is called before the first frame update
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
        Destroy(character.GetComponent<CharacterMovement>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetIndex(){
        return index;
    }

    public void upIndex(){
        index = mod((index+1),characterDict.Count);
        Destroy(character);
        character = Instantiate((GameObject)(Resources.Load("PhotonPrefabs/"+characterDict[index])),transform);
        Destroy(character.GetComponent<CharacterMovement>());
    }

    public void downIndex(){
        index = mod((index-1),characterDict.Count);
        Destroy(character);
        character = Instantiate((GameObject)(Resources.Load("PhotonPrefabs/"+characterDict[index])),transform);
        Destroy(character.GetComponent<CharacterMovement>());
    }

    int mod(int x, int m) {
        return (x%m + m)%m;
    }
}
