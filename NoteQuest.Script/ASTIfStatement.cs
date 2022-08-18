using Sprache;

namespace NoteQuest.Script;

public class ASTIfStatement : ASTStatement
{
    public ASTIfStatement(ASTNode condition, ASTNode statement) : base(new []{ condition, statement})
    {
        HasElse = false;
    }
    
    public ASTIfStatement(ASTNode condition, ASTNode statement, ASTNode elseStatement) : base(new []{ condition, statement, elseStatement})
    {
        HasElse = true;
    }

    public bool HasElse;

    public ASTNode Condition => this[0];
    
    public ASTNode Statement => this[1];
    
    public ASTNode ElseStatement => HasElse ? this[2] : null;

    public override object Evaluate(ScriptEnvironment env)
    {
        var result = Condition.Evaluate(env);
        if (result is not bool b)
        {
            throw new EvaluateException($"Condition '{Condition}' must return boolean");
        }

        if(b)
        {
            Statement.Evaluate(env);
        }
        else if(HasElse)
        {
            ElseStatement.Evaluate(env);
        }

        return null;
    }

    public override string ToString()
    {
        if(HasElse)
            return $"if ({Condition}) {Statement} else {ElseStatement}";
        
        return $"if ({Condition}) {Statement}";
    }
}