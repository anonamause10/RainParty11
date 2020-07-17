using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string title;
    public string description;

    public Sprite image;
    public Sprite sideBar;
    
    public int cost;


}
