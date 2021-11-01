using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl {
    public class TankController : MonoBehaviour {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float accelaration;
        [SerializeField] private float breakeForce;
        [SerializeField] private float resistanceFactor;
        [SerializeField] private TankGun gun;
        [SerializeField] private Bone rootBone;
        [SerializeField] private List<IkHandle> ikHandles;
        [SerializeField] private List<Transform> ikInitPostion;
        [SerializeField] private List<Vector3> ikDesiredPostion;
        [SerializeField] private GameObject ikPrefab;
        [SerializeField]private float threshold;
        private Skeletone skeletone;
        private TankInputs tankInputs;

        private float speed;
        private float rotation;

        private void Awake() {
            tankInputs = new TankInputs();
        }
        private void Start() {
            skeletone = new Skeletone(rootBone);
            InitializeIkHandles();
        }

        private void InitializeIkHandles() {
            for (int i = 0; i < skeletone.ListOfBones.Count; i++) {
                var ih = skeletone.ListOfBones[i].GetComponent<IkHandle>();
                if (ih != null) {
                    ikHandles.Add(ih);
                    ikInitPostion.Add(Instantiate(ikPrefab, ih.transform.position, Quaternion.identity, this.transform).transform);
                    ikDesiredPostion.Add(ih.transform.position);
                }
            }
        }
        private void FixedUpdate() {
            for (int i = 0; i < ikInitPostion.Count; i++) {
                var ray = Physics.Raycast(ikInitPostion[i].position+Vector3.up, Vector3.down, out RaycastHit hit);
                if (ray) {
                Debug.Log(ray);
                    ikDesiredPostion[i] = hit.point;
                    Debug.DrawLine(hit.point, hit.point + Vector3.down, Color.red,1);
                }
            }
            for (int i = 0; i < ikHandles.Count; i++) {
                if(Vector2.SqrMagnitude(ikHandles[i].transform.position - ikDesiredPostion[i]) >= threshold && !ikHandles[i].moving) {
                    // animatte the IK handle toward desiredPOstion or furhther

                    
                }
            }
        }
        private void OnEnable() {
            tankInputs.Enable();

        }
        private void OnDisable() {
            tankInputs.Disable();
        }

        private void Update() {
            var movement = tankInputs.Moving.Move.ReadValue<float>();
            var rotationInput = tankInputs.Moving.Rotate.ReadValue<float>();
            var mouseDelta = tankInputs.Moving.Aim.ReadValue<Vector2>();
            // Debug.Log(mouseDelta);
            Move(movement, rotationInput);
            gun.Aim(mouseDelta);
        }
        public void Move(float yAxis, float xAxis) {

            speed += accelaration * yAxis * Time.deltaTime;
            var resistance = -1 * speed * resistanceFactor;
            speed += resistance;
            if (Mathf.Abs(speed) > maxSpeed)
                speed = maxSpeed * Mathf.Sign(speed);
            transform.position += speed * transform.forward;
            transform.Rotate(transform.up, xAxis, Space.Self);
        }

    }
}