using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronAttacks : MonoBehaviour
{
    public int attackDamage = 20;
	public int enragedAttackDamage = 40;

	public Vector3 attackOffset;
	public float attackRange = 1f;
	public LayerMask attackMask;


    public void DiveAttack()
    {
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
		if (colInfo != null)
		{
			colInfo.GetComponent<Health>().Damage(attackDamage);
		}
    }
}
