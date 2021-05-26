using System.Collections;
using System.Collections.Generic;
using System;

public class Dialogue {

	public int dialogueID {get; private set;}	// ID # of this dialogue node. IMPORTANT: ID's ARE 1-INDEXED, BUT LIST IS 0-INDEXED
	public int speakerID {get; private set;}	// ID # of speaker
	public string content {get; private set;}	// Dialogue lines
	private List<int> links;					// ID # or #s of following dialogue nodes

	private static char separator = ';';		// Character used to separate fields within the text file

	public Dialogue(int dID, int sID, string text, List<int> l)
	{
		dialogueID = dID;
		speakerID = sID;
		content = text;
		links = l;
	}

	// Returns a list of the ID numbers of the dialogue nodes directly after this one
	public List<int> Responses()
	{
		return links;
	}

	// Converts a text file into a List of Dialogue nodes
	public static List<Dialogue> Parse(string text)
	{
		List<Dialogue> nodes = new List<Dialogue>();

		// Parse text file
		string[] lines = text.Split('\n');
		for (int i = 0; i < lines.Length; i++)
		{
			string line = lines[i];
			string[] fields = line.Split(separator);
			nodes.Add(new Dialogue(i+1, int.Parse(fields[0]), fields[1], new List<int>(Array.ConvertAll(fields[2].Split(','), Convert.ToInt32))));
		}
		return nodes;
	}
}
