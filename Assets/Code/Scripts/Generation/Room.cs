using UnityEngine;

namespace Generator
{
    public class RoomGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2 _roomSize = new Vector2(10, 10);
        public Vector2 RoomSize {get => _roomSize; private set => _roomSize = value;}
    }
}
