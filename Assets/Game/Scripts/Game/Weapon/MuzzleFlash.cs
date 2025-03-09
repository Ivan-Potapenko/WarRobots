using UnityEngine;

namespace Game {

    public class MuzzleFlash : MonoBehaviour {

        [SerializeField]
        private ParticleSystem ParticleSystem;

        public void Play() {
            ParticleSystem.Play(true);
        }
    }
}