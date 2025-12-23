using UnityEngine;
namespace Robitnekics
{


    public class Detector : MonoBehaviour
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
                    Owner.OnPlayerEncounter?.Invoke();
                    break;
                case "HackedCrate":

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

                    Owner.OnRoam?.Invoke();

                    break;
                case "HackedCrate":

                    break;
                case "Node":

                    break;
            }
        }
    }
}
