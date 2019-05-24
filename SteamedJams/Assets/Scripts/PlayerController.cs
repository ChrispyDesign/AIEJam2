using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public enum Team
{
    One = 1,
    Two
};

enum PlayerState
{
    Base,
    Dashing,
};

public delegate void VoidEvent();

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Team m_team;
    public XboxController m_controller;
    public int m_maxHealth;    
    public Animator m_animator;    

    [Header("Attack")]
    public int m_defaultSwordDamage = 5;
    public int m_boostedSwordDamage = 15;
    public HurtBox m_swordHurtBox;

    [Header("Movement")]
    public float m_speed;
    public float m_drag;

    [Header("Dash")]
    public float m_dashLength;
    public float m_defaultDashSpeed;
    public float m_boostedDashSpeed;
    public HurtBox m_dashHurtBox;
    public int m_dashDamage;
    public float m_dashCooldown;

    [Header("Renderers")]
    public Renderer m_playerRenderer;
    public Renderer m_swordRenderer;
    public Renderer m_hairRenderer;
    public Renderer m_hitBoxRenderer;

    [Header("Particles")]
    public ParticleSystem m_bloodSpray;
    public ParticleSystem m_healing;
    public ParticleSystem m_playerTrail;
    public ParticleSystem m_godMode;
    public ParticleSystem m_swordTrail;

    PlayerState m_currentState = PlayerState.Base;

    AudioManager m_audioManager;

    bool m_canAttack = true;

    CharacterController m_characterController;
    Vector3 m_velocity;
    float m_speedMultiplier = 1;

    float m_dashTimer;
    float m_dashCooldownTimer;
    Vector3 m_dashDirection;
    float m_dashSpeed;

    bool m_invulnerable;    

    //TEMP
    //public Vector3 m_gravity;
    //
    [Header("Debug")]
    [SerializeField]
    int m_health;
    public List<Effect> m_effects;
    Color m_defaultColour;
    public bool m_useController;
    public float m_setHeight;

    event VoidEvent OnDamageTaken;

    #region Getters/Setters

    public CharacterController CharacterController
    {
        get { return m_characterController; }
    }

    public Vector3 Velocity
    {
        get { return m_velocity; }
    }

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

        //m_defaultColour = m_renderer.material.color;

        m_health = m_maxHealth;
        m_characterController = GetComponent<CharacterController>();

        m_swordHurtBox.m_damage = m_defaultSwordDamage;
        m_swordHurtBox.m_team = m_team;
        m_swordHurtBox.m_player = this;

        m_dashHurtBox.m_damage = m_dashDamage;
        m_dashHurtBox.m_team = m_team;
        m_dashHurtBox.m_player = this;

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

        m_animator.SetFloat("Speed", m_velocity.magnitude);

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
            if (XCI.GetButtonDown(XboxButton.B, m_controller) && m_dashCooldownTimer <= 0 && m_canAttack)
            {
                m_animator.SetTrigger("Dash");

                m_dashHurtBox.Collider.enabled = true;
                m_dashDirection = transform.forward;

                if (m_audioManager && !m_audioManager.IsInWindowOfOpportunity())
                {
                    m_audioManager.StartFeedbackTextRoutine((int)m_team);

                    m_dashTimer = m_dashLength;
                    m_dashSpeed = m_defaultDashSpeed;
                }
                else
                {
                    m_dashTimer = m_dashLength;
                    m_dashSpeed = m_boostedDashSpeed;
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
                if (m_audioManager && !m_audioManager.IsInWindowOfOpportunity())
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
    #endregion

    public void TakeDamage(int damage, PlayerController attacker)
    {
        if (!m_invulnerable)
        {
            m_health -= damage;

            if (m_bloodSpray)
            {
                m_bloodSpray.transform.forward = (transform.position - attacker.transform.position).normalized;
                m_bloodSpray.Play();
            }

            if (OnDamageTaken != null)
                OnDamageTaken.Invoke();
        }
    }

    public void EnableSwordHurtBox()
    {
        m_swordHurtBox.Collider.enabled = true;
    }

    public void DisableSwordHurtBox()
    {
        m_swordHurtBox.Collider.enabled = false;
    }

    void AttackStart()
    {
        m_canAttack = false;
        if (m_audioManager && m_audioManager.IsInWindowOfOpportunity())
        {
            m_audioManager.StartFeedbackTextRoutine((int)m_team);

            m_swordHurtBox.m_damage = m_boostedSwordDamage;
            m_swordHurtBox.transform.localScale = new Vector3(2, 1, 2);
        }
        else
        {
            m_swordHurtBox.m_damage = m_defaultSwordDamage;
            m_swordHurtBox.transform.localScale = new Vector3(1, 1, 1);
        }

        m_hitBoxRenderer.enabled = true;
        m_animator.SetTrigger("Attack");
    }

    public void AttackEnd()
    {
        m_canAttack = true;
        m_hitBoxRenderer.enabled = false;
        //m_swordHurtBox.m_damage = m_swordDamage;
        //m_swordHurtBox.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        //m_swordHurtBox.transform.localPosition = new Vector3(0, 0, 0.5f);
    }

    public void AddEffect(Effect effect)
    {
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

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, m_setHeight, transform.position.z);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody)
        {
            if (hit.moveDirection.y >= -0.3)
            {
                hit.rigidbody.velocity = new Vector3(m_velocity.x, 0, m_velocity.z);
            }
        }
        //if (hit.normal.y > 0)
        //{
        //    transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z);    
        //}
    }
}
