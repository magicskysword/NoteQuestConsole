using System.Xml.Serialization;

namespace NoteQuest.Core.Config;

public enum OpeningDoorsResult
{
    [XmlEnum("Success")]
    Success,
    [XmlEnum("TriggerTrap")]
    TriggerTrap,
    [XmlEnum("Locked")]
    Locked
}

public class OpeningDoorsTable : SingleTable<OpeningDoorsResult>
{
    
}