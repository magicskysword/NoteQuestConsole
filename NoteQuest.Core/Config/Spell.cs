using System;
using System.Xml.Serialization;

namespace NoteQuest.Core.Config;

public enum SpellTargetType
{
    [XmlEnum("None")]
    None,
    [XmlEnum("Friend")]
    Friend,
    [XmlEnum("Enemy")]
    Enemy,
}

/// <summary>
/// 咒语
/// </summary>
public class Spell : IConfig
{
    [XmlElement("Id")]
    public string Id { get; set; }
        
    [XmlElement("Name")]
    public string Name { get; set; }
    
    [XmlElement("Description")]
    public string Description { get; set; }
    
    [XmlElement("TargetType", typeof(SpellTargetType))]
    public SpellTargetType TargetType { get; set; }
}