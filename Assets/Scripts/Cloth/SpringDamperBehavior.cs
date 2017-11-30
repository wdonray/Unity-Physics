﻿using UnityEngine;

namespace HookesLaw
{
    public class SpringDamperBehavior : MonoBehaviour
    {
        public ParticleBehaviour p1, p2;

        private SpringDamper sd;

        public float springConstant, restLength, springDamper;
        // Use this for initialization
        private void Start()
        {
            sd = new SpringDamper(p1.particle, p2.particle, springConstant, restLength, springDamper);
        }

        public void Spring(Particle a, Particle b)
        {
            var dir = b.Postion - a.Postion;

            var dirNormal = dir.normalized;

            var dist = (a.Postion - b.Postion).magnitude;

            var L = sd._lo - dist;

            var force = -sd._ks * L * dirNormal;

            a.AddForce(force);
            b.AddForce(-force);
        }

        public void SpringDot(Particle a, Particle b, float springK, float restL, float springD)
        {
            sd._ks = springK;
            sd._lo = restL;
            sd._kd = springD;

            var dir = b.Postion - a.Postion;
            var l = dir.magnitude;
            var e = dir / l;

            var v1 = Vector3.Dot(e, a.Velocity);
            var v2 = Vector3.Dot(e, b.Velocity);

            var s = sd._ks * (sd._lo - l);
            var d = sd._kd * (v1 - v2);

            var f = -s - d;
            var f1 = f * e;
            var f2 = -f1;

            a.AddForce(f1);
            b.AddForce(f2);
        }
    }
}