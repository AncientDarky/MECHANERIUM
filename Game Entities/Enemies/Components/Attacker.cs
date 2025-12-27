using UnityEngine;

namespace Robitnekics
{


    public class Attacker : MonoBehaviour
    {
        public Robites Owner;
        private void OnEnable()
        {

        }
        private void OnDisable()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Player":

                    Owner.OnAttacking?.Invoke();

                    break;
                case "LockedCrate":
                    Owner.OnAttacking?.Invoke();
                    break;
                case "Node":

                    break;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Player":
                    Owner.OnEngagement?.Invoke();
                    break;
                case "LockedCrate":

                    break;
                case "Node":

                    break;
            }
        }
    }
}
