using System.Collections.Generic;

public class EnemyStateMachine : AStateMachine
{
    private Enemy _enemy;
    public EnemyStateMachine(AStateMachineOwner owner) : base(owner)
    {
        //상태에 관한 컴포넌트
        //움직임에 관한 컴포넌트
        _enemy = _owner.GetComponent<Enemy>();
    }
}
