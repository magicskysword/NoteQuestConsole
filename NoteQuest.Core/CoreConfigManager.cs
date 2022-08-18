using System.Collections.Generic;
using System.Linq;
using NoteQuest.Core.Config;

namespace NoteQuest.Core;

public class CoreConfigManager : BaseConfigManager
{
    public Dictionary<string, Race> Races { get; set; }
    
    public RaceTable RaceTable { get; private set; }

    public virtual void Init(string configDirPath)
    {
        RaceTable = LoadConfig<RaceTable>($"{configDirPath}/Tables/Race/RaceTable.xml");
        Races = LoadConfigs<Race>($"{configDirPath}/Races").ToDictionary(race => race.Id);
    }
}