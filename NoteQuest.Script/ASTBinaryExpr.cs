using System;

namespace NoteQuest.Script;

public abstract class ASTBinaryExpr : ASTList
{
    public ASTBinaryExpr(ASTNode left, ASTNode right) : base(new []{left, right})
    {

    }

    public ASTNode Left => this[0];
    
    public ASTNode Right => this[1];
}

public class ASTBinaryExprDice : ASTBinaryExpr
{
    public ASTBinaryExprDice(ASTNode left, ASTNode right) : base(left, right)
    {

    }

    public override string ToString()
    {
        return $"({Left}d{Right})";
    }
    
    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if (l1 <= 0)
            {
                throw new EvaluateException($"Dice roll must be greater than 0, but was {l1}");
            }
            
            if(right is long l2)
            {
                long result = 0;
                for (int i = 0; i < l1; i++)
                {
                    result += env.Random.Next(1, (int)l2 + 1);
                }
                return result;
            }
        }
        
        throw new EvaluateException($"Invalid dice operation, cannot power '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprPower : ASTBinaryExpr
{
    public ASTBinaryExprPower(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} ^ {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return (long) Math.Pow(l1, l2);
            }
            if (right is double d2)
            {
                return Math.Pow(l1, d2);
            }
        }

        if (left is double d1)
        {
            if(right is long l2)
            {
                return Math.Pow(d1, l2);
            }
            if (right is double d2)
            {
                return Math.Pow(d1, d2);
            }
        }
        
        throw new EvaluateException($"Invalid power operation, cannot power '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprMultiply : ASTBinaryExpr
{
    public ASTBinaryExprMultiply(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} * {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return l1 * l2;
            }
            if (right is double d2)
            {
                return l1 * d2;
            }
        }
        
        if (left is double d1)
        {
            if(right is long l2)
            {
                return d1 * l2;
            }
            if (right is double d2)
            {
                return d1 * d2;
            }
        }
        
        throw new EvaluateException($"Invalid multiply operation, cannot multiply '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprDivide : ASTBinaryExpr
{
    public ASTBinaryExprDivide(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} / {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (right is long and 0 or double and 0)
            throw new EvaluateException($"Cannot divide by zero");
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return l1 / l2;
            }
            if (right is double d2)
            {
                return l1 / d2;
            }
        }
        
        if (left is double d1)
        {
            if(right is long l2)
            {
                return d1 / l2;
            }
            if (right is double d2)
            {
                return d1 / d2;
            }
        }
        
        throw new EvaluateException($"Invalid divide operation, cannot divide '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprModulo : ASTBinaryExpr
{
    public ASTBinaryExprModulo(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} % {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return l1 % l2;
            }
            if (right is double d2)
            {
                return l1 % d2;
            }
        }
        
        if (left is double d1)
        {
            if(right is long l2)
            {
                return d1 % l2;
            }
            if (right is double d2)
            {
                return d1 % d2;
            }
        }
        
        throw new EvaluateException($"Invalid modulo operation, cannot modulo '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprPlus : ASTBinaryExpr
{
    public ASTBinaryExprPlus(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} + {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return l1 + l2;
            }
            if (right is double d2)
            {
                return l1 + d2;
            }
        }
        
        if (left is double d1)
        {
            if(right is long l2)
            {
                return d1 + l2;
            }
            if (right is double d2)
            {
                return d1 + d2;
            }
        }
        
        if(left is string s1)
        {
            if(right is string s2)
            {
                return s1 + s2;
            }
        }
        
        throw new EvaluateException($"Invalid plus operation, cannot plus '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprMinus : ASTBinaryExpr
{
    public ASTBinaryExprMinus(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} - {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return l1 - l2;
            }
            if (right is double d2)
            {
                return l1 - d2;
            }
        }
        
        if (left is double d1)
        {
            if(right is long l2)
            {
                return d1 - l2;
            }
            if (right is double d2)
            {
                return d1 - d2;
            }
        }
        
        throw new EvaluateException($"Invalid minus operation, cannot minus '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprGreaterThan : ASTBinaryExpr
{
    public ASTBinaryExprGreaterThan(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} > {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return l1 > l2;
            }
            if (right is double d2)
            {
                return l1 > d2;
            }
        }
        
        if (left is double d1)
        {
            if(right is long l2)
            {
                return d1 > l2;
            }
            if (right is double d2)
            {
                return d1 > d2;
            }
        }
        
        throw new EvaluateException($"Invalid greater than operation, cannot greater than '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprGreaterThanOrEqual : ASTBinaryExpr
{
    public ASTBinaryExprGreaterThanOrEqual(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} >= {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return l1 >= l2;
            }
            if (right is double d2)
            {
                return l1 >= d2;
            }
        }
        
        if (left is double d1)
        {
            if(right is long l2)
            {
                return d1 >= l2;
            }
            if (right is double d2)
            {
                return d1 >= d2;
            }
        }
        
        throw new EvaluateException($"Invalid greater than or equal operation, cannot greater than or equal '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprLessThan : ASTBinaryExpr
{
    public ASTBinaryExprLessThan(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} < {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return l1 < l2;
            }
            if (right is double d2)
            {
                return l1 < d2;
            }
        }
        
        if (left is double d1)
        {
            if(right is long l2)
            {
                return d1 < l2;
            }
            if (right is double d2)
            {
                return d1 < d2;
            }
        }
        
        throw new EvaluateException($"Invalid less than operation, cannot less than '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprLessThanOrEqual : ASTBinaryExpr
{
    public ASTBinaryExprLessThanOrEqual(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} <= {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is long l1)
        {
            if(right is long l2)
            {
                return l1 <= l2;
            }
            if (right is double d2)
            {
                return l1 <= d2;
            }
        }
        
        if (left is double d1)
        {
            if(right is long l2)
            {
                return d1 <= l2;
            }
            if (right is double d2)
            {
                return d1 <= d2;
            }
        }
        
        throw new EvaluateException($"Invalid less than or equal operation, cannot less than or equal '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprEqual : ASTBinaryExpr
{
    public ASTBinaryExprEqual(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} == {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);

        if (left is bool b1)
        {
            if (right is bool b2)
            {
                return b1 == b2;
            }
        }
        
        if(left is long l1)
        {
            if (right is double d2)
            {
                return l1 == d2;
            }
        }
        
        if(left is double d1)
        {
            if (right is long l2)
            {
                return d1 == l2;
            }
        }

        if (left is not null && right is not null)
        {
            return left.Equals(right);
        }

        return left == right;
    }
}

public class ASTBinaryExprNotEqual : ASTBinaryExpr
{
    public ASTBinaryExprNotEqual(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} != {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is bool b1)
        {
            if (right is bool b2)
            {
                return b1 != b2;
            }
        }
        
        if(left is long l1)
        {
            if (right is double d2)
            {
                return l1 != d2;
            }
        }
        
        if(left is double d1)
        {
            if (right is long l2)
            {
                return d1 != l2;
            }
        }
        
        if (left is not null && right is not null)
        {
            return !left.Equals(right);
        }
        
        return left != right;
    }
}

public class ASTBinaryExprAnd : ASTBinaryExpr
{
    public ASTBinaryExprAnd(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} and {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is bool b1)
        {
            if (right is bool b2)
            {
                return b1 && b2;
            }
        }
        
        throw new EvaluateException($"Invalid 'and' operation, cannot 'and' '{left.GetType()}' and '{right.GetType()}'");
    }
}

public class ASTBinaryExprOr : ASTBinaryExpr
{
    public ASTBinaryExprOr(ASTNode left, ASTNode right) : base(left, right)
    {
        
    }

    public override string ToString()
    {
        return $"({Left} or {Right})";
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        var left = Left.Evaluate(env);
        var right = Right.Evaluate(env);
        
        if (left is bool b1)
        {
            if (right is bool b2)
            {
                return b1 || b2;
            }
        }
        
        throw new EvaluateException($"Invalid 'or' operation, cannot 'or' '{left.GetType()}' and '{right.GetType()}'");
    }
}