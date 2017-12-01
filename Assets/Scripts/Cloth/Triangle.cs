﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HookesLaw
{
    [Serializable]
    public class Triangle
    {
        public float p = 1;
        public float Cd = 1;
        public Particle Particle1, Particle2, Particle3;

        public Triangle(Particle p1, Particle p2, Particle p3)
        {
            Particle1 = p1;
            Particle2 = p2;
            Particle3 = p3;
        }

        public void AerodynamicForce(Vector3 Force)
        {
            var v = (Particle1.Velocity + Particle2.Velocity + Particle3.Velocity) / 3f - Force;
           // var ao = ((Particle2.Postion - Particle1.Postion) (Particle3.Postion - Particle1.Postion));
        }
    }
}
