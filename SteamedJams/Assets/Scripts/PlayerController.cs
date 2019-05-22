using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public int m_maxHealth;
    public Team m_team;

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

    [Header("Dodge")]
    public float m_dodgeLength;

    PlayerState m_currentState = PlayerState.Base;

    AudioManager m_audioManager;

    Animator m_animator;
    bool m_canAttack = true;

    CharacterController m_characterController;
    Vector3 m_velocity;

    float m_dashTimer;
    Vector3 m_dashDirection;
    public float m_dashSpeed;

    bool m_dodging = false;
    float m_dodgeTimer = 0;

    //TEMP
    //public Vector3 m_gravity;
    //
    [Header("Debug")]
    [SerializeField]
    int m_health;
    public Renderer m_renderer;
    Color m_defaultColour;

    public int Health
    {
        get { return m_health; }
        set { m_health = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_audioManager = AudioManager.GetInstance();
        m_audioManager.SetBGM(null);

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
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (movement.sqrMagnitude != 0)
        {
            transform.forward = movement;
        }

        m_velocity += movement * m_speed * Time.deltaTime;
        //m_velocity += m_gravity * Time.deltaTime;

        m_characterController.Move(m_velocity * Time.deltaTime);

        m_velocity -= m_velocity * m_drag * Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && m_canAttack)
        {
            AttackStart();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!m_canAttack && !m_swordHurtBox.enabled)
            {
                AnimatorStateInfo currentAnim = m_animator.GetCurrentAnimatorStateInfo(0);
                m_animator.Play("ReverseAttack", 0, 1 - currentAnim.normalizedTime);
                m_canAttack = true;
            }

            m_dashHurtBox.enabled = true;
            m_dashDirection = transform.forward;
            if (!m_audioManager.IsInWindowOfOpportunity())
            {
                m_dashTimer = m_dashLength;
                m_dashSpeed = m_defaultDashSpeed;
            }
            else
            {
                m_dashTimer = m_dashLength / 2;
                m_dashSpeed = m_defaultDashSpeed * 2;
            }

            m_currentState = PlayerState.Dashing;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!m_canAttack && !m_swordHurtBox.enabled)
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
            m_dashHurtBox.enabled = false;
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
        if (!m_dodging)
            m_health -= damage;
    }

    public void EnableSwordHurtBox()
    {
        m_swordHurtBox.enabled = true;
    }

    public void DisableSwordHurtBox()
    {
        m_swordHurtBox.enabled = false;
    }

    void AttackStart()
    {
        m_canAttack = false;
        if (m_audioManager.IsInWindowOfOpportunity())
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
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();

    //        if (otherPlayer)
    //        {
    //            otherPlayer.TakeDamage(m_dashDamage);
    //        }
    //    }
    //}
}
