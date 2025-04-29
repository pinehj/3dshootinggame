using UnityEngine;

public class IKControl : MonoBehaviour
{
    private Animator _animator;

    public Transform RightHandTransform;
    public Transform LeftHandTransform;

    private int _iKLayerIndex;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _iKLayerIndex = _animator.GetLayerIndex("Shot Layer");
    }

    private void OnAnimatorIK()
    {
        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        _animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandTransform.position);
        _animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandTransform.rotation);

        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTransform.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTransform.rotation);
    }

}
