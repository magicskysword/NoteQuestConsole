namespace NoteQuest.Script;

public static class Words
{
    public static readonly string[] ALL_RESERVED_WORDS = new[]
    {
        BOOLEAN_TRUE,
        BOOLEAN_FALSE,
        NULL,
        NOT,
        AND,
        OR,
        IF,
        ELSE,
        WHILE,
        BREAK,
        CONTINUE,
    };
    
    public const string BOOLEAN_TRUE = "true";
    public const string BOOLEAN_FALSE = "false";
    public const string NULL = "null";
    public const string NOT = "not";
    public const string AND = "and";
    public const string OR = "or";
    public const string IF = "if";
    public const string ELSE = "else";
    public const string DO = "do";
    public const string WHILE = "while";
    public const string BREAK = "break";
    public const string CONTINUE = "continue";
    public const string DICE = "d";
    public const string DICE_UPPER = "D";
}