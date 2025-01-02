using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using KeymapsCards.Models;
using Newtonsoft.Json;

namespace KeymapsCards.Services;

public class StatisticsService
{
    private const string StatisticsFile = "statistics.json";

    private List<Statistics> _statistics = [];
    public ReadOnlyCollection<Statistics> Statistics => _statistics.OrderByDescending(q => q.Start).ToList().AsReadOnly();
    
    public void SaveStatistics(Statistics statistics)
    {
        _statistics.Add(statistics);
        
        var data = JsonConvert.SerializeObject(_statistics, Formatting.Indented);
        
        using var writer = new StreamWriter(StatisticsFile);
        writer.Write(data);
    }
    
    public void LoadStatistics()
    {
        if (!File.Exists(StatisticsFile))
            return;

        try
        {
            using var reader = new StreamReader(StatisticsFile);
            var json = reader.ReadToEnd();
            _statistics = JsonConvert.DeserializeObject<List<Statistics>>(json) ?? [];

            // foreach (var stat in _statistics.ToList())
            // {
            //     if (stat.End == default)
            //     {
            //         Console.WriteLine("Removing unfinished game from statistics");
            //         _statistics.Remove(stat);
            //     }
            // }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public void ClearStatistics()
    {
        _statistics.Clear();
        File.Delete(StatisticsFile);
    }
}