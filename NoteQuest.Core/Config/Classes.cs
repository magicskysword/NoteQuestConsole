using System.Xml.Serialization;

namespace NoteQuest.Core.Config;

/// <summary>
/// 职业
/// </summary>
public class Classes : IConfig
{
    [XmlElement("Id")]
    public string Id { get; set; }
        
    [XmlElement("Name")]
    public string Name { get; set; }
    
    [XmlElement("Description")]
    public string Description { get; set; }
    
    [XmlElement("OnGameStartCmd")]
    public string OnGameStartCmd { get; set; }
}