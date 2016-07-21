using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class DataManager
{
	public static int CurrentLevel = 1;

	public static readonly string pathToLevels = "/Levels/";
	private static LevelData[] arrLevels;

	public static LevelData GetLevel(int levelNo)
	{
		LevelData levelData;

		if (arrLevels == null)
			levelData = LoadLevel (levelNo);
		else
			levelData = arrLevels [levelNo];
		
		return levelData;
	}

	public static LevelData[] GetLevels()
	{
		if (arrLevels == null)
			LoadLevels ();

		return arrLevels;
	}

	// write data to xml on a new thread
	public static void SaveLevelData(LevelData levelData)
	{
		/*
		XmlDocument xml = ReadXML( GetAbsolutePath( levelData.FileName );

		// things to write: rating, state 

		XmlNode myNode = root.SelectSingleNode("descendant::books");
		myNode.Value = "blabla";
		
		//xml.Save("D:\\build.xml");
		*/
	}

	public static LevelData[] LoadLevels() {

		string[] files = Directory.GetFiles (Application.dataPath + pathToLevels);

		List<LevelData> levelDataList = new List<LevelData>(); // since we don't know the number of correct files yet
		for (int i = 0; i < files.Length; i++)
		{
			if(Path.GetExtension(files[i]) == ".xml") 
				levelDataList.Add( LoadLevelData( Path.GetFileName(files[i]) ) );
		}

		// You can convert it back to an array if you would like to
		arrLevels = levelDataList.ToArray();
		return arrLevels;
	}

	private static LevelData LoadLevel(int levelNo)
	{
		return LoadLevelData ("level_" + levelNo + ".xml");
	}

	private static LevelData LoadLevelData(string filename)
    {
		string dataPath = GetAbsolutePath (filename);

		XmlDocument xml = ReadXML(dataPath);
        
		if (xml != null)
        {
			Dictionary<CubeSide, List<bool>> gridData = new Dictionary<CubeSide, List<bool>>();

			string contents = File.ReadAllText(dataPath);
            xml.LoadXml(contents);

            ushort gridSize = ushort.Parse(xml.DocumentElement.SelectSingleNode("/LevelData/GeneralData/GridSize").InnerText);
			ushort levelNo = ushort.Parse(xml.DocumentElement.SelectSingleNode("/LevelData/GeneralData/LevelNo").InnerText);
			ushort rating = ushort.Parse(xml.DocumentElement.SelectSingleNode("/LevelData/GeneralData/Rating").InnerText);
			LevelStatus levelStatus = (LevelStatus) Enum.Parse(typeof(LevelStatus), xml.DocumentElement.SelectSingleNode ("/LevelData/GeneralData/Status").InnerText);
            XmlNode gridCollection = xml.DocumentElement.SelectSingleNode("/LevelData/GridCollection");
            XmlNodeList grids = gridCollection.SelectNodes("Grid");

            var gridCounter = 0;
            ushort qtyDots = 0;
            foreach (XmlNode grid in grids)
            {
                XmlNodeList subGrids = grid.SelectNodes("Subgrid");
                gridData[(CubeSide)gridCounter] = new List<bool>();
                foreach (XmlNode subGrid in subGrids)
                {
                    bool hasDot = bool.Parse(subGrid.SelectSingleNode("Dot").InnerText);
                    qtyDots = (hasDot == true) ? ++qtyDots : qtyDots;
                    gridData[(CubeSide)gridCounter].Add(hasDot);
                }
                gridCounter++;
            }

			LevelData levelData = LevelData.CreateInstance<LevelData>();
			levelData.Init(levelNo, filename, levelStatus, rating, gridData, gridSize, qtyDots);

			return levelData;
        }
        return null;
    }

	private static string GetAbsolutePath(string filename)
	{
		return Application.dataPath + pathToLevels + filename.TrimStart('/');
	}

	private static XmlDocument ReadXML(string absolutePath)
	{
		if (!File.Exists (absolutePath))
			return null;
			
		XmlDocument xml = new XmlDocument();

		string contents = File.ReadAllText(absolutePath);
		xml.LoadXml(contents);

		return xml;
	}
}
