using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

/// <summary>
/// Class to read node pairs given as {i,j} or {i,j,k} separated
/// by a blank ' ' from a text file. This class is intended
/// to be used as a function within a library. The main methods
/// are for tests only.<\br>
/// Other nomenclature is possible by changing the default parameters.
/// </summary>
public class TupleReader
{
	private const string DEFAULT_OPENER = @"{";
	private const string DEFAULT_CLOSER = @"}";
	private const string DEFAULT_INNER_SEPARATOR = @",";
	private const string DEFAULT_OUTER_SEPARATOR = @"[\s]";


//	public static void Main(string[] args)
//	{
//		String pairPath = "./blatt2_aufgabe1_b_graph.txt";
//		String triplePath = "./blatt4_aufg1_b.txt";

//		Console.WriteLine("processing " + pairPath);
//		List<int[]> pairs = ReadInBrackets(pairPath, 2);
//		foreach (int[] t in pairs)
//		{
//			Console.WriteLine(t[0] + "->" + t[1]);
//		}

//		Console.WriteLine("----");
//		Console.WriteLine("processing " + triplePath);

//		List<int[]> triples = ReadInBrackets(triplePath, 3);
//		foreach (int[] t in triples)
//		{
//			Console.WriteLine(t[0] + "->" + t[1] + ": " + t[2]);
//		}

//		Console.WriteLine("----");
//		Console.WriteLine("processing cities");

//		List<string> names = ReadList(@"city_names.txt", "", "^", "$");
//		List<double[]> coords = ReadDoublesFromFile(@"./cities_xy.txt", 2, ";", "", "^", "$");
//		for (int i = 0; i < names.Count; ++i )
//		{
//			Console.WriteLine(names[i] + ": " + coords[i][0] + "," + coords[i][1]);
//		}
//	}

	public static List<int[]> ReadPairs(string path, char bracket = '{')
	{
		return ReadInBrackets(path, 2, bracket);
	}
	public static List<int[]> ReadTriples(string path, char bracket = '{')
	{
		return ReadInBrackets(path, 3, bracket);
	}

	/// <summary>
	/// Liest n-Tupel von Ganzzahlen (mit festem n) aus einer Datei aus.
	/// Einzelne Tupel können auch durch Newlines getrennt werden.
	/// 
	/// Die optionalen String-Parameter bilden reguläre Ausdrücke, welche mit der eingelesenen Datei übereinstimmen müssen. Sie können auch leere Strings sein.
	/// Hinweis: Beachte die Verwendung von "\" als Escape-Symbol in Standardstrings.
	/// 
	/// Der bracket-Parameter gibt den Typ der Klammern an, von denen die Tupel umgeben sind. Die Angaben zusammengehöriger Klammern (öffnend/schließend) sind äquivalent
	/// 
	/// Beispiel einer Datei mit dem Inhalt "{0,3} {4,-2}"
	/// ReadInBrackets(path, 2, "[\\s]", "", '{')
	/// 
	/// </summary>
	/// <param name="path">Dateipfad</param>
	/// <param name="nElems">n, Anzahl Elemente pro Tupel</param>
	/// <param name="innerSeparator">Trennsequenz innerhalb eines Tupels</param>
	/// <param name="outerSeparator">Trennsequenz zwischen zwei Tupeln</param>
	/// <param name="bracket">Klammerntyp</param>
	/// <returns>Liste aller erfolgreich eingelesenen Tupel, jedes Arrayelement hat die Länge nElems</returns>
	public static List<int[]> ReadInBrackets(string path, int nElems,
		char bracket = '{', string innerSeparator = DEFAULT_INNER_SEPARATOR,
		string outerSeparator = DEFAULT_OUTER_SEPARATOR)
	{
		string bracketOpen = @"{";
		string bracketClose = @"}";
		switch (bracket) {
		case '{':
		case '}':
			bracketOpen = @"{";
			bracketClose = @"}";
			break;
		case '(':
		case ')':
			bracketOpen = @"\(";
			bracketClose = @"\)";
			break;
		case '[':
		case ']':
			bracketOpen = @"\[";
			bracketClose = @"\]";
			break;
		case '<':
		case '>':
			bracketOpen = @"<";
			bracketClose = @">";
			break;
		default:
			throw new ArgumentException("invalid bracket");
		}

		return ReadIntsFromFile(path, nElems, innerSeparator, outerSeparator, bracketOpen, bracketClose);
	}

	/// <summary>
	/// Liest n-Tupel von Ganzzahlen (mit festem n) aus einer Datei aus.
	/// Einzelne Tupel können auch durch Newlines getrennt werden.
	/// 
	/// Die optionalen Parameter bilden reguläre Ausdrücke, welche mit der eingelesenen Datei übereinstimmen müssen. Sie können auch leere Strings sein.
	/// Hinweis: Beachte die Verwendung von "\" als Escape-Symbol in Standardstrings.
	/// 
	/// Beispiel einer Datei mit dem Inhalt "{0, 3};{4, -2}"
	/// ReadIntsFromFile(path, 2, ",[\\s]", ";", "{", "}")
	/// 
	/// Beispiel einer Datei mit dem Inhalt "(0,3,2)(4,-2,1)"
	/// ReadIntsFromFile(path, 3, ",", "", "\\(", "\\)")
	/// 
	/// </summary>
	/// <param name="path">Dateipfad</param>
	/// <param name="nElems">n, Anzahl Elemente pro Tupel</param>
	/// <param name="innerSeparator">Trennsequenz innerhalb eines Tupels</param>
	/// <param name="outerSeparator">Trennsequenz zwischen zwei Tupeln</param>
	/// <param name="opener">Sequenz zu Beginn eines Tupels</param>
	/// <param name="closer">Sequenz am Ende eines Tupels</param>
	/// <returns>Liste aller erfolgreich eingelesenen Tupel, jedes Arrayelement hat die Länge nElems</returns>
	public static List<int[]> ReadIntsFromFile(string path, int nElems,
		string innerSeparator = DEFAULT_INNER_SEPARATOR,
		string outerSeparator = DEFAULT_OUTER_SEPARATOR,
		string opener = DEFAULT_OPENER, string closer = DEFAULT_CLOSER)
	{
		List<int[]> resultList = new List<int[]>();

		FileStream aFile = new FileStream(path, FileMode.Open);
		using (StreamReader sr = new StreamReader(aFile, System.Text.Encoding.Default))
		{
			StringBuilder sb = new StringBuilder(@"\G" + opener);
			for (int i = 0; i < nElems; ++i)
			{
				sb.Append((i != nElems - 1) ? @"(-?\d+)" + innerSeparator : @"(-?\d+)");
			}
			sb.Append(closer + outerSeparator + "?");
			string pat = sb.ToString();

			Regex oRegEx = new Regex(pat);

			while (sr.Peek() > -1)
			{
				string strLine = sr.ReadLine();

				Match m = oRegEx.Match(strLine);
				while (m.Success)
				{
					int[] newElem = new int[nElems];
					for (int i = 0; i < nElems; ++i)
					{
						newElem[i] = int.Parse(m.Groups[i+1].Value, CultureInfo.InvariantCulture);
					}
					resultList.Add(newElem);
					m = m.NextMatch();
				}
			}
		}

		return resultList;
	}

	/// <summary>
	/// Liest n-Tupel von Doubles (mit festem n) aus einer Datei aus. Liest nur Format mit optionalem Dezimalpunkt, keine wissenschaftliche Schreibweise o. ä.
	/// Einzelne Tupel können auch durch Newlines getrennt werden.
	/// 
	/// Die optionalen Parameter bilden reguläre Ausdrücke, welche mit der eingelesenen Datei übereinstimmen müssen. Sie können auch leere Strings sein.
	/// Hinweis: Beachte die Verwendung von "\" als Escape-Symbol in Standardstrings.
	/// 
	/// Beispiel einer Datei mit dem Inhalt "{0, 3.4};{4.2, -2.1}"
	/// ReadDoublesFromFile(path, 2, ",[\\s]", ";", "{", "}")
	/// 
	/// Beispiel einer Datei mit dem Inhalt "(0,3.4,2)(4.2,-2.1,1)"
	/// ReadDoublesFromFile(path, 3, ",", "", "\\(", "\\)")
	/// 
	/// </summary>
	/// <param name="path">Dateipfad</param>
	/// <param name="nElems">n, Anzahl Elemente pro Tupel</param>
	/// <param name="innerSeparator">Trennsequenz innerhalb eines Tupels</param>
	/// <param name="outerSeparator">Trennsequenz zwischen zwei Tupeln</param>
	/// <param name="opener">Sequenz zu Beginn eines Tupels</param>
	/// <param name="closer">Sequenz am Ende eines Tupels</param>
	/// <returns>Liste aller erfolgreich eingelesenen Tupel, jedes Arrayelement hat die Länge nElems</returns>
	public static List<double[]> ReadDoublesFromFile(string path, int nElems,
		string innerSeparator = DEFAULT_INNER_SEPARATOR,
		string outerSeparator = DEFAULT_OUTER_SEPARATOR,
		string opener = DEFAULT_OPENER, string closer = DEFAULT_CLOSER)
	{
		List<double[]> resultList = new List<double[]>();

		FileStream aFile = new FileStream(path, FileMode.Open);
		using (StreamReader sr = new StreamReader(aFile, System.Text.Encoding.Default))
		{
			StringBuilder sb = new StringBuilder(@"\G" + opener);
			for (int i = 0; i < nElems; ++i)
			{
			    sb.Append((i != nElems - 1) ? @"(-?\d+(?:\.\d+)?)" + innerSeparator : @"(-?\d+(?:\.\d+)?)"); // this line is different from ReadIntsFromFile
			}
			sb.Append(closer + outerSeparator + "?");
			string pat = sb.ToString();

			Regex oRegEx = new Regex(pat);

			while (sr.Peek() > -1)
			{
				string strLine = sr.ReadLine();

				Match m = oRegEx.Match(strLine);
				while (m.Success)
				{
					double[] newElem = new double[nElems];
					for (int i = 0; i < nElems; ++i)
					{
						newElem[i] = double.Parse(m.Groups[i + 1].Value, CultureInfo.InvariantCulture);
					}
					resultList.Add(newElem);
					m = m.NextMatch();
				}
			}
		}

		return resultList;
	}

	/// <summary>
	/// Liest eine Liste von Strings aus einer Datei aus.
	/// 
	/// Die ausgelesenen Strings können in einem Muster eingebettet sein.
	/// 
	/// Die optionalen Parameter bilden reguläre Ausdrücke, welche mit der eingelesenen Datei übereinstimmen müssen. Sie können auch leere Strings sein.
	/// Hinweis: Beachte die Verwendung von "\" als Escape-Symbol in Standardstrings.
	/// 
	/// </summary>
	/// <param name="path">Dateipfad</param>
	/// <param name="outerSeparator">Sequenz zwischen einzelnen closer und opener</param>
	/// <param name="opener">Sequenz vor einzulesenden Strings</param>
	/// <param name="closer">Sequenz nach einzulesenden Strings</param>
	/// <returns></returns>
	public static List<string> ReadList(string path,
		string outerSeparator = DEFAULT_OUTER_SEPARATOR,
		string opener = DEFAULT_OPENER, string closer = DEFAULT_CLOSER)
	{
		List<string> resultList = new List<string>();

		FileStream aFile = new FileStream(path, FileMode.Open);
		using (StreamReader sr = new StreamReader(aFile, System.Text.Encoding.Default))
		{
			string pat = @"\G" + opener + @"(.*)" + closer + outerSeparator + "?";

			Regex oRegEx = new Regex(pat);

			while (sr.Peek() > -1)
			{
				string strLine = sr.ReadLine();

				Match m = oRegEx.Match(strLine);
				while (m.Success)
				{
					resultList.Add(m.Groups[1].Value);
					m = m.NextMatch();
				}
			}
		}

		return resultList;
	}
}
