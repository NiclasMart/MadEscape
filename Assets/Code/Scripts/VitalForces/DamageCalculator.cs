// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 20.02.2025
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;

namespace VitalForces{
public static class DamageCalculator
{   
    const int ARMOR_OFFSETT = 10;
    public static float CalculateDamage(float amount, float armor)
        {
            float damageReduction = armor / (armor + ARMOR_OFFSETT);
            return amount * (1 - damageReduction);
        }
}

}