using System.Collections.Generic;
using UnityEngine;

public class PillarManager: MonoBehaviour
{

    public List<Pillar> pillars { get; private set; }
    public int numberOfTotalPillar;

    private void Awake()
    {
        numberOfTotalPillar = 0;

        pillars = new List<Pillar>();
    }

    public void RegisterPillar(Pillar pillar)
    {
        pillars.Add(pillar);
        pillar.index = numberOfTotalPillar;

        numberOfTotalPillar++;
    }
}

