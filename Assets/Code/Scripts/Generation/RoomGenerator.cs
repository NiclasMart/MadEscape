using UnityEngine;

namespace Generator
{
    public class RoomGenerator : MonoBehaviour
    {
        [SerializeField] private Vector2 roomSize = new Vector2(10, 10);
        public Vector2 RoomSize {get => roomSize; private set => roomSize = value;}
    }
}
