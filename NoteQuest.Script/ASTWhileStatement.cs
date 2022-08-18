using System;

namespace NoteQuest.Script;

public class ASTWhileStatement : ASTStatement
{
    public ASTWhileStatement(ASTNode condition, ASTNode statement) : base(new []{condition, statement})
    {
    }

    public ASTNode Condition => this[0];
    public ASTNode Statement => this[1];

    public override object Evaluate(ScriptEnvironment env)
    {
        var result = Condition.Evaluate(env);
        if (result is not bool b)
        {
            throw new EvaluateException($"Condition '{Condition}' must return boolean");
        }
        while (b)
        {
            try
            {
                Statement.Evaluate(env);
            }
            catch (BreakException)
            {
                break;
            }
            catch (ContinueException)
            {
                // ignore
            }
            
            result = Condition.Evaluate(env);
            if (result is not bool b2)
            {
                throw new EvaluateException($"Condition '{Condition}' must return boolean");
            }

            b = b2;
        }
        
        return null;
    }

    public override string ToString()
    {
        return $"while ({Condition}) {Statement}";
    }
}