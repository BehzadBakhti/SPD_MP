using UnityEngine;

namespace PlayerControl {
    public class TankGun : MonoBehaviour {
        [SerializeField] private float aimingSpeed;
        
        public void Aim(Vector2 aim) {

            transform.Rotate(transform.up, aim.x * Time.deltaTime * aimingSpeed, Space.Self);
            transform.Rotate(transform.right, -aim.y * Time.deltaTime * aimingSpeed, Space.Self);
        }
    }
}