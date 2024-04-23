using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProjectDataBase
{
	public class DBLimitsExtractor
	{

		public DBLimitsExtractor() 
		{ 
		
		}

		public Dictionary<string, object> ExtractSettings(string variableNames, string variableValues)
		{
			// Split the variable names and values based on the '+' delimiter
			string[] names = variableNames.Split('+');
			string[] values = variableValues.Split('+');

			// Create a dictionary to hold the variable names and corresponding values
			var settingsDictionary = new Dictionary<string, object>();

			// Iterate through the variable names and values and add them to the dictionary
			for (int i = 0; i < names.Length; i++)
			{
				names[i].Trim();
				names[i] = names[i].Replace(" ", "");
				// Check if the corresponding value exists before adding to the dictionary
				if (i < values.Length)
				{
					// Add the name and value to the dictionary
					settingsDictionary.Add(names[i], values[i]);
				}
				else
				{
					// If there is no corresponding value, add the name with a null value
					settingsDictionary.Add(names[i], null);
				}
			}

			return settingsDictionary;
		}
	}
}
