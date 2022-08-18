using System.Xml.Serialization;

namespace NoteQuest.Core.Config;

public class Weapon : IConfig
{
    [XmlElement("Id")]
    public string Id { get; set; }
        
    [XmlElement("Name")]
    public string Name { get; set; }
    
    [XmlElement("Description")]
    public string Description { get; set; }
    
    [XmlElement("Damage")]
    public string Damage { get; set; }
}