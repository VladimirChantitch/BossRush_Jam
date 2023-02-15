using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Tuto : BossCharacter
{
    [SerializeField] List<Sub_BossTuto> subBosses = new List<Sub_BossTuto> ();

    public int deadCount;

    private void Awake()
    {
        subBosses.ForEach(sb => sb.onKilled.AddListener(() =>
        {
            deadCount++;
            if (deadCount == subBosses.Count)
            {
                AllSubBossesDestroyed();
            }
        }));
    }

    private void AllSubBossesDestroyed()
    {
        /// Start a count down for each sub 
            /// Make them reappear in their next phase of bullet hell
        /// The first time the maine boss arrives
        /// The second time the main boss Attaks the pleyr with his vulneravle head
        /// The first time he goes full bullet hell
        throw new NotImplementedException();
    }
}
