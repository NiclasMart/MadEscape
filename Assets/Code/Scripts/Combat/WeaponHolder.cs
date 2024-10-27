// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 24.07.24
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class WeaponHolder : MonoBehaviour 
    {
        private EnemyFinderAll enemyFinderAll;

        private void Awake() 
        {
            enemyFinderAll = GetComponent<EnemyFinderAll>();    
        }

        private void Update()
        {
            //handle weapon rotation
            Vector3 lookTargetPosition = transform.position + transform.forward;
            GameObject closestEnemy = enemyFinderAll.GetClosestEnemy();
            if (closestEnemy != null)
            {
                lookTargetPosition = closestEnemy.transform.position;
            }
            RotateTo(lookTargetPosition);
        }

        public void RotateTo(Vector3 position)
        {
            Vector3 targetPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.LookAt(targetPosition);
        }
    }
}