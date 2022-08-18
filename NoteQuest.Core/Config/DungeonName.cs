using System.Xml.Serialization;

namespace NoteQuest.Core.Config;

public class DungeonName : IConfig
{
    [XmlElement("Id")]
    public string Id { get; set; }
        
    [XmlElement("Name")]
    public string Name { get; set; }
}

public class DungeonLastName : IConfig
{
    [XmlElement("Id")]
    public string Id { get; set; }
        
    [XmlElement("Name")]
    public string Name { get; set; }
    
    [XmlElement("Result")]
    public string Result { get; set; }
}