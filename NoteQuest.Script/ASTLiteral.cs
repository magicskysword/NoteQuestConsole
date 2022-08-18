using System.Globalization;

namespace NoteQuest.Script;

public abstract class ASTLiteral : ASTLeaf
{
    
}

public class ASTLongIntegerLiteral : ASTLiteral
{
    public ASTLongIntegerLiteral(long value)
    {
        Value = value;
    }
    
    public long Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override object Evaluate(ScriptEnvironment env)
    {
        return Value;
    }
}

public class ASTDoubleFloatLiteral : ASTLiteral
{
    public ASTDoubleFloatLiteral(double value)
    {
        Value = value;
    }
    
    public double Value { get; }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }
    
    public override object Evaluate(ScriptEnvironment env)
    {
        return Value;
    }
}

public class ASTStringLiteral : ASTLiteral
{
    public ASTStringLiteral(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public override string ToString()
    {
        return "\"" + Value + "\"";
    }
    
    public override object Evaluate(ScriptEnvironment env)
    {
        return Value;
    }
}

public class ASTBooleanLiteral : ASTLiteral
{
    public ASTBooleanLiteral(bool value)
    {
        Value = value;
    }
    
    public bool Value { get; }

    public override string ToString()
    {
        return Value.ToString().ToLower();
    }
    
    public override object Evaluate(ScriptEnvironment env)
    {
        return Value;
    }
}

public class ASTNullLiteral : ASTLiteral
{
    public ASTNullLiteral()
    {
    }
    
    public override string ToString()
    {
        return "null";
    }
    
    public override object Evaluate(ScriptEnvironment env)
    {
        return null;
    }
}