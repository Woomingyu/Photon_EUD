using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemy : Enemy
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

    //랜덤 행동

    private void RandomAction()
    {
        //RandomSound(); //랜덤 일상사운드 재생

        int _random = Random.Range(0, 2); //대기, 두리번, 걷기 (랜덤int 는 최대값 포함안함)

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Peek();
    }


    //##행동패턴##
    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    private void Peek()
    {
        currentTime = waitTime;
        //anim.SetTrigger("Peek");
        Debug.Log("두리번");
    }
}
