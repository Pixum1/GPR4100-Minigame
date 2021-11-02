using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "GameData")]
public class GameData : ScriptableObject
{
    public List<Collectible> Collectibles { get { return collectibles; } }
    private List<Collectible> collectibles = new List<Collectible>(); //a list of all coins in the level

    public List<GuardBehaviour> Guards { get { return guards; } }
    private List<GuardBehaviour> guards = new List<GuardBehaviour>();

    //public void AddGuard(GuardBehaviour _guard)
    //{
    //    guards.Add(_guard);
    //}
}
