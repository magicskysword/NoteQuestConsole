using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace NoteQuest.Core.Config;

public abstract class BaseConfigManager
{
    public static Dictionary<Type, XmlSerializer> Serializers = new Dictionary<Type, XmlSerializer>();
    
    private XmlSerializer GetSerializer(Type type)
    {
        if (!Serializers.ContainsKey(type))
        {
            Serializers.Add(type, new XmlSerializer(type));
        }
        return Serializers[type];
    }

    public T LoadConfig<T>(string filePath)
    {
        if(File.Exists(filePath))
        {
            return LoadConfig<T>(new FileStream(filePath, FileMode.Open));
        }
        throw new FileNotFoundException("File not found", filePath);
    }
    
    public T LoadConfig<T>(Stream stream)
    {
        var serializer = GetSerializer(typeof(T));
        var data = serializer.Deserialize(stream);
        if(data is not T tData)
        {
            throw new InvalidDataException("Invalid data");
        }
        return tData;
    }
    
    /// <summary>
    /// 读取配置文件
    /// </summary>
    /// <param name="dirPath">目标目录</param>
    /// <param name="recursive">递归读取</param>
    /// <typeparam name="T">配置类型</typeparam>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public IEnumerable<T> LoadConfigs<T>(string dirPath, bool recursive = false)
    {
        if(Directory.Exists(dirPath))
        {
            var serializer = GetSerializer(typeof(T));
            foreach (var file in Directory.GetFiles(dirPath, "*.xml", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                yield return (T)serializer.Deserialize(new FileStream(file, FileMode.Open));
            }
        }
        else
        {
            // 目录不存在
            throw new FileNotFoundException("Directory not found", dirPath);
        }
    }
}