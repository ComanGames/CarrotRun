using UnityEngine;
using UnityStandardAssets.Cameras;

namespace Assets.Standard_Assets.Cameras.Scripts
{
    public class LookatTarget : AbstractTargetFollower
    {
        // A simple script to make one object look at another,
        // but with optional constraints which operate relative to
        // this gameobject's initial rotation.

        // Only rotates around local X and Y.

        // Works in local coordinates, so if this object is parented
        // to another moving gameobject, its local constraints will
        // operate correctly
        // (Think: looking out the side window of a car, or a gun turret
        // on a moving spaceship with a limited angular range)

        // to have no constraints on an axis, set the rotationRange greater than 360.

        [SerializeField] private Vector2 _mRotationRange;
        [SerializeField] private float _mFollowSpeed = 1;

        private Vector3 _mFollowAngles;
        private Quaternion _mOriginalRotation;

        protected Vector3 MFollowVelocity;


        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            _mOriginalRotation = transform.localRotation;
        }


        protected override void FollowTarget(float deltaTime)
        {
            // we make initial calculations from the original local rotation
            transform.localRotation = _mOriginalRotation;

            // tackle rotation around Y first
            Vector3 localTarget = transform.InverseTransformPoint(m_Target.position);
            float yAngle = Mathf.Atan2(localTarget.x, localTarget.z)*Mathf.Rad2Deg;

            yAngle = Mathf.Clamp(yAngle, -_mRotationRange.y*0.5f, _mRotationRange.y*0.5f);
            transform.localRotation = _mOriginalRotation*Quaternion.Euler(0, yAngle, 0);

            // then recalculate new local target position for rotation around X
            localTarget = transform.InverseTransformPoint(m_Target.position);
            float xAngle = Mathf.Atan2(localTarget.y, localTarget.z)*Mathf.Rad2Deg;
            xAngle = Mathf.Clamp(xAngle, -_mRotationRange.x*0.5f, _mRotationRange.x*0.5f);
            var targetAngles = new Vector3(_mFollowAngles.x + Mathf.DeltaAngle(_mFollowAngles.x, xAngle),
                                           _mFollowAngles.y + Mathf.DeltaAngle(_mFollowAngles.y, yAngle));

            // smoothly interpolate the current angles to the target angles
            _mFollowAngles = Vector3.SmoothDamp(_mFollowAngles, targetAngles, ref MFollowVelocity, _mFollowSpeed);


            // and update the gameobject itself
            transform.localRotation = _mOriginalRotation*Quaternion.Euler(-_mFollowAngles.x, _mFollowAngles.y, 0);
        }
    }
}
