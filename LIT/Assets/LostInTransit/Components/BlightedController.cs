﻿using LostInTransit.Buffs;
using RoR2;
using System.Linq;
using UnityEngine.Networking;

namespace LostInTransit.Components
{

    //Todo: turn this into probably a networked body attachment?
    public class BlightedController : NetworkBehaviour
    {
        [SyncVar]
        public int FirstEliteIndex;

        [SyncVar]
        public int SecondEliteIndex;

        public EliteDef[] availableElites { get => Elites.Blight.EliteDefsForBlightedElites.ToArray(); }

        public AffixBlighted.AffixBlightedBehavior BuffBehavior
        {
            get
            {
                if(_buffBehavior)
                {
                    return _buffBehavior;
                }
                else
                {
                    var body = GetComponent<CharacterMaster>()?.GetBodyObject();
                    if(body)
                    {
                        _buffBehavior = body.GetComponent<AffixBlighted.AffixBlightedBehavior>();
                    }
                    return _buffBehavior;
                }
            }
            set
            {
                _buffBehavior = value;
            }
        }

        private AffixBlighted.AffixBlightedBehavior _buffBehavior;

        private Xoroshiro128Plus RNG => Run.instance.runRNG;

        public void OnEnable()
        {
            RandomizeElites();
        }

        private void RandomizeElites()
        {
            var elites = availableElites;
            if (NetworkServer.active)
            {
                FirstEliteIndex = (int)elites[RNG.RangeInt(0, elites.Length)].eliteIndex;
                //Removes the first elite index from available elites, avoids duplicates.
                elites = availableElites.Where(elite => elite.eliteIndex != (EliteIndex)FirstEliteIndex).ToArray();
                SecondEliteIndex = (int)elites[RNG.RangeInt(0, elites.Length)].eliteIndex;
            }
            //            Invoke("RpcOnEliteIndexAssigned", 0.1f);
            RpcOnEliteIndexAssigned();
        }

        public void Ability()
        {
            RandomizeElites();
        }

        [ClientRpc]
        private void RpcOnEliteIndexAssigned(/*int index1, int index2*/)
        {
            BuffBehavior?.SetElites(FirstEliteIndex, SecondEliteIndex);
        }
        /*[Server]
        private void HostOnEliteIndexAssigned(int index1, int index2)
        {
            buffBehavior.SetElites(index1, index2);
        }*/

    }
}
