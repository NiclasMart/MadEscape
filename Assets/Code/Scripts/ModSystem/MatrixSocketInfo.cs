using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SocketInfo", menuName = "Scriptable Objects/CharacterMatrix/SocketInfo")]
public class MatrixSocketInfo : ScriptableObject
{
    public string Name;
    public SocketType SocketType;
    public Action Ability; //look into video: https://www.youtube.com/watch?v=jvokCXXYHCg
    [TextArea(1, 5)] public string Description;
}

//implement as custom property drawer in CharacterMatrix_Template: https://www.youtube.com/watch?v=3w9iXT6wx5g
