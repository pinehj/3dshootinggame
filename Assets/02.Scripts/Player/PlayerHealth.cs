using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private Animator _animator;
    public float MaxHealth;
    private float _health;
    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            UIManager.Instance.UpdateHealthSlider(_health);
            _animator.SetLayerWeight(2, 1- _health / MaxHealth);
        }
    }
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        UIManager.Instance.InitializePlayerHealthSlider(MaxHealth);
        Health = MaxHealth;
    }
    public  void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        UIManager.Instance.StartHitEffect();
    }
    
}
