
using UnityEngine;
using System.Collections;
using System.Linq; 
 
public class CSVReader : MonoBehaviour 
{
	
	/*** Field ***/
    private TextAsset csvFile;

	public string[,] grid;
	public int readCSVrow=1;
    public int row=-0,col=0;
    

	/**********methods************/	
	public void set_csvFile_joint(TextAsset t){
		this.csvFile = t;
	}
	
	public void Start()
	{
        
		//Setup
		TextAsset t = Resources.Load("Datas/testdata_ori") as TextAsset;
        set_csvFile_joint(t);
	    
        initGrids();
		DebugOutputGrid(grid);
    	StartCoroutine(WaitForSecond(1.0f));
	}
	
	public void initGrids(){
		grid = SplitCsvGrid(csvFile.text);
        Debug.Log("size = " + grid.GetUpperBound(0) + "," + grid.GetUpperBound(1));
        col = grid.GetUpperBound(0)-1;
        row = grid.GetUpperBound(1)-1;
	}

    private void Update(){
	}

    
	private IEnumerator WaitForSecond(float second){
		yield return new WaitForSeconds(second);
	}
	
	// outputs the content of a 2D array, useful for checking the importer
	public void DebugOutputGrid(string[,] grid)
	{
		string textOutput = ""; 
		for (int y = 0; y < grid.GetUpperBound(1); y++) {	
			for (int x = 0; x < grid.GetUpperBound(0); x++) {
 
				textOutput += grid[x,y]; 
				textOutput += "|"; 
			}
			textOutput += "\n"; 
		}
		Debug.Log(textOutput);
	}
 
	// splits a CSV file into a 2D string array
	public string[,] SplitCsvGrid(string csvText)
	{
		string[] lines = csvText.Split("\n"[0]); 
 
		// finds the max width of row
		int width = 0; 
		for (int i = 0; i < lines.Length; i++)
		{
			string[] row = SplitCsvLine( lines[i] ); 
			width = Mathf.Max(width, row.Length); 
		}
 
		// creates new 2D string grid to output to
		string[,] outputGrid = new string[width + 1, lines.Length + 1]; 
		for (int y = 0; y < lines.Length; y++)
		{
			string[] row = SplitCsvLine( lines[y] ); 
			for (int x = 0; x < row.Length; x++) 
			{
				outputGrid[x,y] = row[x]; 
 
				// This line was to replace "" with " in my output. 
				// Include or edit it as you wish.
				outputGrid[x,y] = outputGrid[x,y].Replace("\"\"", "\"");
			}
		}
 
		return outputGrid; 
	}
 
	// splits a CSV row 
	public string[] SplitCsvLine(string line)
	{
		return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)", 
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
		select m.Groups[1].Value).ToArray();
	}

    
}