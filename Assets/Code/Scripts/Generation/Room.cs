using UnityEngine;

namespace Generator
{
    public class Room : MonoBehaviour
    {
        [field: SerializeField] public Vector2 RoomSize { get; private set; } = new Vector2(10, 10);
        
        public bool GetRandomPointInRoom(out Vector3 point, bool testNavMesh = true)
        {
            float x = (RoomSize.x - 1) / 2f * Random.Range(-1f, 1f);
            float z = (RoomSize.y - 1) / 2f * Random.Range(-1f, 1f);

            if (!testNavMesh)
            {
                point = new Vector3(x, 0, z);
                return true;
            }
            
            if (UnityEngine.AI.NavMesh.SamplePosition(new Vector3(x, 0, z), out UnityEngine.AI.NavMeshHit hit, 1, UnityEngine.AI.NavMesh.AllAreas))
            {
                point = hit.position;
                return true;
            }
            point = Vector3.zero;
            return false;
        }
    }
}
