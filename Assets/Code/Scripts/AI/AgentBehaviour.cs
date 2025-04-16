// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 16.04.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;

namespace AI
{
    public abstract class AgentBehaviour : MonoBehaviour
    {
        private Vector3 _targetPosition;

        void Update()
        {
            _targetPosition = CalculateNewTargetPosition();
        }

        public Vector3 GetTargetPosition()
        {
            return _targetPosition;
        }

        protected abstract Vector3 CalculateNewTargetPosition();
    }
}