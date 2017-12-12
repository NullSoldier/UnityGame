using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 6f;
    public float AttackCooldown = 0.70f;
    public int Damage = 2;
    public bool IsDead = false;
    public int Health = 3;
    public GameObject projectileTemplate;

    private CharacterController controller;
    private GameObject bodyContainer;
    private GameObject lich;
    private Animator anim;
    private GameObject shootPoint;
    private Vector3 direction;
    private float attackTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        lich = this.FindGameObjectByName("Lich");
        shootPoint = this.FindGameObjectByName("ShootPoint");

        if (controller == null)
            throw new Exception("CharacterController is required on PlayerController");
        if (lich == null)
            throw new Exception("Lich is required in PlayerController");
        if (shootPoint == null)
            throw new Exception("ShootPoint is required in PlayerController");
        
        direction = calculateMoveDir(0f, 1f);
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDir = calculateMoveDir(horizontal, vertical) * MoveSpeed;

        if (IsDead)
            return;

        if (Input.GetButton("Fire1"))
        {
            attack();
            rotateBody(moveDir);
        }
        else
        {
            rotateBody(moveDir);
            move(moveDir);
            anim.SetBool("IsMoving", moveDir != Vector3.zero);
        }

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            IsDead = true;
            anim.SetBool("IsMoving", false);
            anim.SetTrigger("Die");
        }
        else
        {
            anim.SetTrigger("TakeDamage");
        }
    }

    private Vector3 calculateMoveDir(float horizontal, float vertical)
    {
        Camera camera = Camera.main;

        Vector3 cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1));

        Vector3 moveDir = (
            (vertical * cameraForward.normalized) +
            (horizontal * camera.transform.right));

        return moveDir.normalized;
    }

    private void move(Vector3 moveDir)
    {
        moveDir.y -= 20;
        controller.Move(moveDir * Time.deltaTime);
    }

    private void attack()
    {
        if (attackTimer > 0)
            return;

        GameObject projectile = Instantiate(
            projectileTemplate,
            shootPoint.transform.position,
            shootPoint.transform.rotation);

        ProjectileController controller = projectile.GetComponent<ProjectileController>();
        controller.Damage = Damage;
        controller.Direction = direction;

        attackTimer = AttackCooldown;
        anim.SetTrigger("Attack");
//        Debug.DrawRay(transform.position, direction * 5, Color.black, 0.5f);
    }

    private void rotateBody(Vector3 moveDir)
    {
        if (moveDir != Vector3.zero)
        {
            direction = moveDir.normalized;
            direction.y = 0;
        }

        lich.transform.rotation = Quaternion.LookRotation(direction);
    }
}
