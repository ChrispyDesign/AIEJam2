using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public enum Team
{
    One,
    Two
};

enum PlayerState
{
    Base,
    Dashing,
    Dodging
};

public delegate void VoidEvent();

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public int m_playerNumber;
    public Team m_team;
    public XboxController m_controller;
    public int m_maxHealth;

    [Header("Attack")]
    public int m_swordDamage;
    public HurtBox m_swordHurtBox;

    [Header("Movement")]
    public float m_speed;
    public float m_drag;

    [Header("Dash")]
    public float m_dashLength;
    public float m_defaultDashSpeed;
    public HurtBox m_dashHurtBox;
    public int m_dashDamage;
    public float m_dashCooldown;

    [Header("Dodge")]
    public float m_dodgeLength;

    PlayerState m_currentState = PlayerState.Base;

    AudioManager m_audioManager;

    Animator m_animator;
    bool m_canAttack = true;

    CharacterController m_characterController;
    Vector3 m_velocity;
    float m_speedMultiplier = 1;

    float m_dashTimer;
    float m_dashCooldownTimer;
    Vector3 m_dashDirection;
    public float m_dashSpeed;

    bool m_dodging = false;
    float m_dodgeTimer = 0;

    bool m_invulnerable;

    //TEMP
    //public Vector3 m_gravity;
    //
    [Header("Debug")]
    [SerializeField]
    int m_health;
    public Renderer m_renderer;
    public List<Effect> m_effects;
    Color m_defaultColour;
    public bool m_useController;

    event VoidEvent OnDamageTaken;

    #region Getters/Setters

    public int Health
    {
        get { return m_health; }
        set
        {
            m_health = value;
            m_health = Mathf.Clamp(m_health, 0, m_maxHealth);
        }
    }

    public float SpeedMultiplier
    {
        get { return m_speedMultiplier; }
        set { m_speedMultiplier = value; }
    }

    public bool Invulnerable
    {
        get { return m_invulnerable; }
        set { m_invulnerable = value; }
    }


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_effects = new List<Effect>();
        m_audioManager = AudioManager.GetInstance();

        m_defaultColour = m_renderer.material.color;

        m_health = m_maxHealth;
        m_animator = GetComponent<Animator>();
        m_characterController = GetComponent<CharacterController>();

        m_swordHurtBox.m_damage = m_swordDamage;
        m_swordHurtBox.m_team = m_team;

        m_dashHurtBox.m_damage = m_dashDamage;
        m_dashHurtBox.m_team = m_team;

        m_dashSpeed = m_defaultDashSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_dashCooldownTimer > 0)
            m_dashCooldownTimer -= Time.deltaTime;

        switch (m_currentState)
        {
            case PlayerState.Base:
                BaseState();
                break;
            case PlayerState.Dashing:
                DashingState();
                break;
            case PlayerState.Dodging:
                DodgingState();
                break; 
        }
    }

    #region States
    void BaseState()
    {
        for (int i = 0; i < m_effects.Count; i++)
        {
            if (!m_effects[i].OnEffectUpdate())
            {
                m_effects.RemoveAt(i);
                i--;
            }
        }
        Vector3 movement;
        if (m_useController)
        {
            movement = new Vector3(XCI.GetAxisRaw(XboxAxis.LeftStickX, m_controller), 0, XCI.GetAxisRaw(XboxAxis.LeftStickY, m_controller));
        }
        else
        {
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }
        if (movement.sqrMagnitude != 0)
        {
            transform.forward = movement;
        }

        m_velocity += movement * m_speed * Time.deltaTime;
        //m_velocity += m_gravity * Time.deltaTime;

        m_characterController.Move(m_velocity * m_speedMultiplier * Time.deltaTime);

        m_velocity -= m_velocity * m_drag * Time.deltaTime;

        if (m_useController)
        {
            if (XCI.GetButtonDown(XboxButton.A, m_controller) && m_canAttack)
            {
                AttackStart();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && m_canAttack)
            {
                AttackStart();
            }
        }

        if (m_useController)
        {
            if (XCI.GetButtonDown(XboxButton.B, m_controller) && m_dashCooldownTimer <= 0)
            {
                if (!m_canAttack && !m_swordHurtBox.Collider.enabled)
                {
                    AnimatorStateInfo currentAnim = m_animator.GetCurrentAnimatorStateInfo(0);
                    m_animator.Play("ReverseAttack", 0, 1 - currentAnim.normalizedTime);
                    m_canAttack = true;
                }

                m_dashHurtBox.Collider.enabled = true;
                m_dashDirection = transform.forward;
                if (true/*!m_audioManager.IsInWindowOfOpportunity()*/)
                {
                    m_dashTimer = m_dashLength;
                    m_dashSpeed = m_defaultDashSpeed;
                }
                else
                {
                    m_dashTimer = m_dashLength / 2;
                    m_dashSpeed = m_defaultDashSpeed * 2;
                }

                m_dashCooldownTimer = m_dashCooldown;
                m_currentState = PlayerState.Dashing;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && m_dashCooldownTimer <= 0)
            {
                if (!m_canAttack && !m_swordHurtBox.Collider.enabled)
                {
                    AnimatorStateInfo currentAnim = m_animator.GetCurrentAnimatorStateInfo(0);
                    m_animator.Play("ReverseAttack", 0, 1 - currentAnim.normalizedTime);
                    m_canAttack = true;
                }

                m_dashHurtBox.Collider.enabled = true;
                m_dashDirection = transform.forward;
                if (true/*!m_audioManager.IsInWindowOfOpportunity()*/)
                {
                    m_dashTimer = m_dashLength;
                    m_dashSpeed = m_defaultDashSpeed;
                }
                else
                {
                    m_dashTimer = m_dashLength / 2;
                    m_dashSpeed = m_defaultDashSpeed * 2;
                }

                m_dashCooldownTimer = m_dashCooldown;
                m_currentState = PlayerState.Dashing;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!m_canAttack && !m_swordHurtBox.Collider.enabled)
            {
                AnimatorStateInfo currentAnim = m_animator.GetCurrentAnimatorStateInfo(0);
                m_animator.Play("ReverseAttack", 0, 1 - currentAnim.normalizedTime);
                m_canAttack = true;
            }

            m_dodging = true;
            m_dodgeTimer = m_dodgeLength;

            m_renderer.material.color = new Color(0, 0, 0);

            m_currentState = PlayerState.Dodging;
        }   
    }

    void DashingState()
    {
        m_dashTimer -= Time.deltaTime;

        if (m_dashTimer > 0)
        {
            m_characterController.Move(m_dashDirection * m_dashSpeed * Time.deltaTime);
        }
        else
        {
            m_dashHurtBox.Collider.enabled = false;
            m_currentState = PlayerState.Base;
        }
    }

    void DodgingState()
    {
        m_dodgeTimer -= Time.deltaTime;
        if (m_dodgeTimer <= 0)
        {
            m_dodging = false;
            m_renderer.material.color = m_defaultColour;
            m_currentState = PlayerState.Base;
        }
    }
    #endregion

    public void TakeDamage(int damage)
    {
        if (!m_dodging && !m_invulnerable)
        {
            m_health -= damage;

            if (OnDamageTaken != null)
                OnDamageTaken.Invoke();
        }
    }

    public void EnableSwordHurtBox()
    {
        //m_swordHurtBox.enabled = true;
        m_swordHurtBox.Collider.enabled = true;
    }

    public void DisableSwordHurtBox()
    {
        //m_swordHurtBox.enabled = false;
        m_swordHurtBox.Collider.enabled = false;
    }

    void AttackStart()
    {
        m_canAttack = false;
        if (false/*m_audioManager.IsInWindowOfOpportunity()*/)
        {
            m_swordHurtBox.m_damage = m_swordDamage * 2;
            m_swordHurtBox.transform.localScale = new Vector3(0.2f, 0.2f, 2);
            m_swordHurtBox.transform.localPosition = new Vector3(0, 0, 1);
        }
        else
        {
            m_swordHurtBox.m_damage = m_swordDamage;
            m_swordHurtBox.transform.localScale = new Vector3(0.2f, 0.2f, 1);
            m_swordHurtBox.transform.localPosition = new Vector3(0, 0, 0.5f);
        }
        m_animator.SetTrigger("Attack");
    }

    public void AttackEnd()
    {
        m_canAttack = true;
        m_swordHurtBox.m_damage = m_swordDamage;
        m_swordHurtBox.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        m_swordHurtBox.transform.localPosition = new Vector3(0, 0, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            Effect effect = other.gameObject.GetComponent<Effect>();

            if (!effect.Instant)
            {
                effect.Player = this;
                effect.OnPickup();
                m_effects.Add(effect);
                Destroy(effect.gameObject);                       
            }
            else
            {
                effect.Player = this;
                effect.OnPickup();
                Destroy(effect.gameObject);
            }
        }
    }
}
