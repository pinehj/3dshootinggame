using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;

    private int _iKLayerIndex;
    public Transform RightHandTransform;
    public Transform LeftHandTransform;
    public bool ShouldIK = true;
    private void Awake()
    {

        _iKLayerIndex = _animator.GetLayerIndex("Shot Layer");
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if(layerIndex != _iKLayerIndex)
        {
            return;
        }
        if (ShouldIK)
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
    public void SetFloat(string name, float value)
    {
        _animator.SetFloat(name, value);
    }

    public void SetTrigger(string name)
    {
        _animator.SetTrigger(name);
    }

    public void SetLayerWeight(string layerName, float value)
    {
        int layerId = _animator.GetLayerIndex(layerName);
        _animator.SetLayerWeight(layerId, value);
    }
}
