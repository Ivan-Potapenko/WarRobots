using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class RailgunProjectile : AbstractProjectile
    {
        private void Update()
        {
            DestructionThroughTime();
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
    }
}