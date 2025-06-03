// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.05.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using CharacterProgressionMatrix;
using Stats;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class AreaWalkerBehaviour : AgentBehaviour
    {
        private Vector3 _activeTargetPosition;

        protected override Vector3 CalculateNewTargetPosition()
        {
            //calculate random position
            if (!HasReachedDestination()) return _activeTargetPosition;
            
            RoomRef.GetRandomPointInRoom(out _activeTargetPosition, false);
            return _activeTargetPosition;
        }
        
        
    }
}