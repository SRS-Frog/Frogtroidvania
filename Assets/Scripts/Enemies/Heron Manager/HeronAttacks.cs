using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronAttacks : MonoBehaviour
{
    public int attackDamage = 20;
	public int enragedAttackDamage = 40;
	public Transform hitbox;
	public LayerMask pLayer;

	public Vector3 attackOffset;
	public float attackRange = 1f;
	public LayerMask attackMask;
	Vector2 pos; 
	Rigidbody2D rb;

	void Start()
	{
		rb = GameObject.FindGameObjectWithTag("Heron").GetComponent<Rigidbody2D>(); 
	}

	void Update()
	{
		pos = rb.position;
	}

    public void DiveAttack()
    {
        Collider2D[] colInfo = Physics2D.OverlapCircleAll(hitbox.position, 5f, pLayer);

		foreach (Collider2D player in colInfo)
		{
			player.GetComponent<Health>().Damage(attackDamage);
			Debug.Log("Current HP: " + player.GetComponent<Health>().health);
		}
    }

	void OnDrawGizmosSelected()
	{
		if (hitbox == null)
		return;

		Gizmos.DrawWireSphere(hitbox.position, 5f);

	}
}
