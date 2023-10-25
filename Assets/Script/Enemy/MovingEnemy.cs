using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    private DamageFlash _damageFlash;

    protected override void ReSet()
    {
        base.ReSet();
        RandomAction();
    }

    public override void Damage(int _dmg)
    {
        _damageFlash = GetComponent<DamageFlash>();
        _damageFlash.CallDamageFlash();
        base.Damage(_dmg);
    }

    //���� �ൿ

    private void RandomAction()
    {
        //RandomSound(); //���� �ϻ���� ���

        int _random = Random.Range(0, 3); //���, �θ���, �ȱ� (����int �� �ִ밪 ���Ծ���)

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Peek();
        else if (_random == 2)
            TryWalk();
    }


    //##�ൿ����##
    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("���");
    }
    private void Peek()
    {
        currentTime = waitTime;
        //anim.SetTrigger("Peek");
        Debug.Log("�θ���");
    }
}
