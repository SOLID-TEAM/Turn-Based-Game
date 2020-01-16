using UnityEngine;

using System.IO;

public static class CSVManager
{
    private static string reportFolderName = "Report";
    private static string reportFileName = "BattleInfo.csv";
    private static string reportSeparator = ";"; // CSV separator for columns

    private static string[] reportHeaders = new string[16]
    {
        "Character A",
        "HP",
        "ATK",
        "DEF",
        "SPEED",
        "LVL",
        "Character B",
        "HP",
        "ATK",
        "DEF",
        "SPEED",
        "Num Simulations",
         "LVL",
        "Num Wins",
        "Num Defeats",
        "Win Rate"
    };

    public static void AppendToReport(Character charA, Character charB, int numSimulations, int numWins)
    {
        CheckDirectory();
        CheckFile();

        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            // character A and B -------------------------
            finalString += charA.characterName + reportSeparator;
            foreach(Statistic stat in charA.statistics)
            {
                if (stat.name == "avoid") continue;
                finalString += stat.levelValue.ToString() + reportSeparator;
            }
            finalString += charA.level.ToString() + reportSeparator;

            finalString += charB.characterName + reportSeparator;
            foreach (Statistic stat in charB.statistics)
            {
                if (stat.name == "avoid") continue;
                finalString += stat.levelValue.ToString() + reportSeparator;
            }

            finalString += charB.level.ToString() + reportSeparator;

            // -------------------------------------------
            // general info
            finalString += numSimulations.ToString() + reportSeparator;             // num simulations
            finalString += numWins.ToString() + reportSeparator;                    // num wins
            finalString += (numSimulations - numWins).ToString() + reportSeparator; // num defeats
            finalString += ( (float)numWins / (float)numSimulations) * 100f +"%";                       // timestamp

            // write to file
            sw.WriteLine(finalString);
        }
    }

    public static void CreateReport()
    {
        CheckDirectory();

        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for(int i = 0; i < reportHeaders.Length; ++i)
            {
                if(finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += reportHeaders[i];
            }

            sw.WriteLine(finalString);
        }
    }

    static void CheckDirectory()
    {
        string dir = GetDirectoryPath();

        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    static void CheckFile()
    {
        string file = GetFilePath();

        if(!File.Exists(file))
        {
            CreateReport();
        }
    }

    static string GetDirectoryPath()
    {
        return Application.dataPath + "/" + reportFolderName;
    }

    static string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }
}
