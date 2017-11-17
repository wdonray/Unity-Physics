﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace BoidsSpace
{
    public class FlockingBehaviour : MonoBehaviour
    {
        public bool isReady = false;
        public FloatVariable AFac;
        public FloatVariable CFac;
        public FloatVariable DFac;
        public FloatVariable MaxSpeed;
        public FloatVariable MaxForce;
        public FloatVariable BoundaryDistance;
        public FloatVariable BFac;
        public FloatVariable Count;
        //public Transform target;
        
        [SerializeField]
        private List<Agent> _agents = new List<Agent>();

        public void SetAgents()
        {
            _agents = AgentFactory.Agents;
            isReady = true;
            BFac.Value = 20;
        }

        public List<Boid> Neighbors(Boid b)
        {
            var neighbors = new List<Boid>();
            var agents = _agents.FindAll(x => Vector3.Distance(x.Position, b.Position) < 5);
            agents.ForEach(a => neighbors.Add(a as Boid));
            return neighbors;
        }
        void Update()
        {
            if (!isReady) return;
            foreach (var agent in _agents)
            {
                agent.MaxSpeed = MaxSpeed.Value;
                agent.MaxForce = MaxForce.Value;
                var v1 = Alignment(agent as Boid);
                var v2 = Dispersion(agent as Boid);
                var v3 = Cohesion(agent as Boid);
                var v4 = BoundaryForce(agent as Boid);
                var allforces = AFac.Value * v1 + DFac.Value * v2 + CFac.Value * v3;

                agent.AddForce(BFac.Value, v4.normalized);
                agent.AddForce(allforces.magnitude, allforces.normalized);
            }
        }
        #region Algorithm
        public Vector3 Avoid(Boid b)
        {
            var avoidForce = Vector3.zero;
            foreach (var neighbor in Neighbors(b))
            {
                var dist = Vector3.Distance(neighbor.AvoidPos, neighbor.Position);
                if (dist < 5f)
                {
                    var dir = (neighbor.AvoidPos - neighbor.Position).normalized;
                    avoidForce = dist * dir;
                }
            }
            return avoidForce;
        }

        public Vector3 Dispersion(Boid b)
        {
            if (Neighbors(b).Count <= 0)
                return Vector3.zero;
            var seperationForce = Vector3.zero;
            foreach (var neighbor in Neighbors(b))
            {
                if (neighbor == b) continue;
                var dist = Vector3.Distance(b.Position, neighbor.Position);
                if (dist < 10f)
                {
                    var dir = (b.Position - neighbor.Position).normalized;
                    seperationForce += dir;
                }
            }
            return seperationForce;
        }

        public Vector3 Cohesion(Boid b)
        {
            if (Neighbors(b).Count <= 1)
                return Vector3.zero;
            var cohesionForce = Vector3.zero;
            foreach (var neighbor in Neighbors(b))
            {
                if (neighbor == b) continue;
                cohesionForce += neighbor.Position;
            }
            cohesionForce /= Neighbors(b).Count - 1;

            return (cohesionForce - b.Position) / 100;
        }

        public Vector3 Alignment(Boid b)
        {
            if (Neighbors(b).Count <= 1)
                return Vector3.zero;
            var alignmentForce = Vector3.zero;
            foreach (var neighbor in Neighbors(b))
            {
                if (neighbor == b) continue;
                alignmentForce += neighbor.Velocity;
            }
            alignmentForce /= Neighbors(b).Count - 1;

            return (alignmentForce - b.Velocity) / 8;
        }

        public Vector3 BoundaryForce(Boid b)
        {
            var force = Vector3.zero;
            var dist = Vector3.Distance(b.Position, Vector3.zero);
            if (dist > BoundaryDistance.Value)
                force = dist  * (Vector3.zero - b.Position);
            return force;
        }
    }
    #endregion Algorithm

}