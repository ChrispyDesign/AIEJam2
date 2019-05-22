using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    One,
    Two
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
    public float m_dashSpeed;
    public HurtBox m_dashHurtBox;
    public int m_dashDamage;

    Animator m_animator;
    bool m_canAttack = true;

    CharacterController m_characterController;
    Vector3 m_velocity;

    bool m_dashing;
    float m_dashTimer;
    Vector3 m_dashDirection;

    //TEMP
    //public Vector3 m_gravity;
    //

    [SerializeField]
    int m_health;

    public int Health
    {
        get { return m_health; }
        set { m_health = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_health = m_maxHealth;
        m_animator = GetComponent<Animator>();
        m_characterController = GetComponent<CharacterController>();

        m_swordHurtBox.m_damage = m_swordDamage;
        m_swordHurtBox.m_team = m_team;

        m_dashHurtBox.m_damage = m_dashDamage;
        m_dashHurtBox.m_team = m_team;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_dashing)
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
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_dashing = true;
                m_dashHurtBox.enabled = true;
                m_dashTimer = 0;

                m_dashDirection = transform.forward;
            }

            if (Input.GetMouseButtonDown(0) && m_canAttack)
            {
                AttackStart();
            }
        }
        else
        {
            Dashing();
        }
    }

    void Dashing()
    {
        m_dashTimer += Time.deltaTime;

        if (m_dashTimer < m_dashLength)
        {
            m_characterController.Move(m_dashDirection * m_dashSpeed * Time.deltaTime);         
        }
        else
        {
            m_dashing = false;
            m_dashHurtBox.enabled = false;
        }
    }

    public void TakeDamage(int damage)
    {
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
        bool inWindow = false; // TEMP REPLACE WITH BEAT SYSTEM
        if (inWindow)
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
