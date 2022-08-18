using System.Xml.Serialization;

namespace NoteQuest.Core.Config;

/// <summary>
/// 表的基类
/// </summary>
public abstract class TableBase : IConfig
{
    [XmlElement("Id")]
    public string Id { get; set; }
}

public class TableResult<T>
{
    [XmlElement("Result")]
    public int Result { get; set; }
    [XmlElement("Value")]
    public T Value { get; set; }
}