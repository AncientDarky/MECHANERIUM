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
                    switch (Owner.ROBTYPE)
                    {
                        case RobiteTypes.Standard:

                            Owner.OnEngagement?.Invoke();
                            break;
                        case RobiteTypes.SuddenRobite:

                            break;
                        case RobiteTypes.CrateRobite:

                            break;
                    }
                 
                    break;
                case "HackedCrate":
                    switch (Owner.ROBTYPE)
                    {
                        case RobiteTypes.Standard:

                            break;
                        case RobiteTypes.SuddenRobite:

                            break;
                        case RobiteTypes.CrateRobite:

                            break;
                    }
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
                    switch (Owner.ROBTYPE)
                    {
                        case RobiteTypes.Standard:
                            Owner.OnRoam?.Invoke();

                            break;
                        case RobiteTypes.SuddenRobite:
                            // if too long past or distance despawn
                            break;
                        case RobiteTypes.CrateRobite:
                            // Attack crate
                            Owner.OnEngagement?.Invoke();
                            break;
                    }

                    break;
                case "HackedCrate":
                    switch(Owner.ROBTYPE)
                    {
                        case RobiteTypes.Standard:

                            break;
                        case RobiteTypes.SuddenRobite:

                            break;
                        case RobiteTypes.CrateRobite:

                            break;
                    }
                    break;
                case "Node":
                    switch (Owner.ROBTYPE)
                    {
                        case RobiteTypes.Standard:

                            break;
                        case RobiteTypes.SuddenRobite:

                            break;
                        case RobiteTypes.CrateRobite:

                            break;
                    }
                    break;
            }
        }
    }
}
