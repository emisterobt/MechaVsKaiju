using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObjects/InputConfig", fileName = "New InputConfig")]
public class InputSO : ScriptableObject
{
    public KeyCode jump;
    public KeyCode dash;
    public KeyCode run;
    public KeyCode crouch;
    public KeyCode pickup;
    public KeyCode mainAttack;
    public KeyCode secondaryAttack;
    public KeyCode specialAttack;
}
