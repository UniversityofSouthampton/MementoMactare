using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack")]
public class Attack : ScriptableObject
{
    public AttackType[] attacks;
    public AttackReactions[] reactions;

}
