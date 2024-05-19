using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticlesBehaviour : MonoBehaviour, IBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    public void ExecuteBehaviour()
    {
        _particles.Stop();
    }
}
