using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NoteQuest.Core.Config;

public abstract class SingleTable<T> : TableBase
{
    [XmlElement("DiceExpr")]
    public string DiceExpr { get; set; }
    
    [XmlElement("Results")]
    public List<TableResult<T>> Results { get; set; }

    protected virtual T OnGetResult(int result)
    {
        foreach (var tableResult in Results)
        {
            if (tableResult.Result == result)
            {
                return tableResult.Value;
            }
        }
        
        throw new Exception($"Result {result} not found in table {Id}");
    }

    public virtual T RandomResult()
    {
        return OnGetResult(DiceUtil.Roll(DiceExpr));
    }
}