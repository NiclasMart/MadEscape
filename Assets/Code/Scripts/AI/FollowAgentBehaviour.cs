// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 16.04.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace AI
{
    public class FollowAgentBehaviour : AgentBehaviour
    {
        Transform _player;
        void Start()
        {
            _player = ServiceProvider.Get<GameManager>().GetPlayer().transform;
        }

        protected override Vector3 CalculateNewTargetPosition()
        {
            return _player.position;
        }
    }
}