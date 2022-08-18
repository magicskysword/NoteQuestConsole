namespace NoteQuest.Script;

public abstract class ASTUnaryExpr : ASTList
{
    public ASTUnaryExpr(ASTNode expr) : base(new ASTNode[] { expr })
    {
        
    }
    
    public ASTNode Expr => this[0];
}

public class ASTUnaryExprNegative : ASTUnaryExpr
{
    public ASTUnaryExprNegative(ASTNode expr) : base(expr)
    {
    }
    
    public override string ToString()
    {
        return $"-{Expr}";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var value = Expr.Evaluate(env);
        if(value is long l)
        {
            return -l;
        }

        if(value is double d)
        {
            return -d;
        }

        throw new EvaluateException($"Cannot negate {value}, need a long or double value.");
    }
}

public class ASTUnaryExprNot : ASTUnaryExpr
{
    public ASTUnaryExprNot(ASTNode expr) : base(expr)
    {
    }
    
    public override string ToString()
    {
        return $"not {Expr}";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var value = Expr.Evaluate(env);
        if(value is bool b)
        {
            return !b;
        }

        throw new EvaluateException($"Cannot 'not' {value}, need a boolean value.");
    }
}