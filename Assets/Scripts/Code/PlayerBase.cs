using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour, IDamageable
{
    public Action OnBaseDestroyed;

    public float Hp { get; set; }
    [SerializeField] private float m_StartingHp;
    [SerializeField] private Image m_HpUI;

    private void Awake()
    {
        Hp = m_StartingHp;
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage;
        m_HpUI.fillAmount = Hp/m_StartingHp;

        if(Hp <= 0 )
        {
            OnBaseDestroyed?.Invoke();
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        OnBaseDestroyed -= OnBaseDestroyed;
    }
}
