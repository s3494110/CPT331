﻿#region Using References

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using CPT331.Core.ObjectModel;

#endregion

namespace CPT331.Data.Parsers
{
	public class VicXmlParser
	{
		//	Number of offences by geographic area and offence type, year ending December 2014									
		public VicXmlParser(string filename)
		{
			_filename = filename;
		}

		private readonly string _filename;

		public void Parse()
		{
			if ((String.IsNullOrEmpty(_filename) == false) && (File.Exists(_filename) == true))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(_filename);

				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Workbook/Worksheet/Table/Row[position() > 1]");

				//	The dataset I'm working with has a single year - 2014
				int year = 2014;

				State vicState = StateRepository.GetStateByAbbreviatedName("VIC");
				List<LocalGovernmentArea> localGovernmentAreas = LocalGovernmentAreaRepository.GetLocalGovernmentAreasByStateID(vicState.ID);
				List<Offence> offences = OffenceRepository.GetOffences();

				List<Crime> crimes = new List<Crime>();

				foreach (XmlNode xmlNode in xmlNodeList)
				{
					string localGovernmentAreaName = xmlNode.ChildNodes[2].InnerText.Trim();
					string offenceName = xmlNode.ChildNodes[3].InnerText.Trim();
					string suboffenceName = xmlNode.ChildNodes[4].InnerText.Trim();
					int count = Convert.ToInt32(xmlNode.ChildNodes[5].InnerText);

					Console.WriteLine($"{localGovernmentAreaName}: {offenceName} ({suboffenceName}), count: {count}");

					LocalGovernmentArea localGovernmentArea = localGovernmentAreas.Where(m => (m.Name == localGovernmentAreaName)).FirstOrDefault();
					Offence offence = null;

					if (String.IsNullOrEmpty(offenceName) == false)
					{
						offence = offences.Where(m => (m.Name == offenceName)).FirstOrDefault();
					}

					if (String.IsNullOrEmpty(suboffenceName) == false)
					{
						offence = offences.Where(m => (m.Name == suboffenceName)).FirstOrDefault();
					}

					//	We only have crime data per year, so it will always be added in on 01/01/YYYY
					CrimeRepository.AddCrime(count, localGovernmentArea.ID, 1, offence.ID, year);
				}
			}
		}
	}
}