using System.Xml.Serialization;

namespace NoteQuest.Core.Config;

/// <summary>
/// 种族
/// </summary>
public class Race : IConfig
{
    [XmlElement("Id")]
    public string Id { get; set; }
        
    [XmlElement("Name")]
    public string Name { get; set; }
        
    [XmlElement("Description")]
    public string Description { get; set; }
        
    [XmlElement("Hp")]
    public int Hp { get; set; }

    [XmlElement("OnGameStartCmd")]
    public string OnGameStartCmd { get; set; }
}