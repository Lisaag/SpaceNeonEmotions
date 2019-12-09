using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Button Presses", menuName = "buttn Presses")]
public class ButtonPresses : ScriptableObject
{
    public List<int> blueRoom = new List<int>();
    public List<int> redRoom = new List<int>();
}