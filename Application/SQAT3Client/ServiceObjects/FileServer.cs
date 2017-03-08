using System;
using System.IO;
using System.Collections.Specialized;

namespace Molmed.SQAT.ServiceObjects
{
    public class FileServerStatusEventArgs : EventArgs
    {
        private string MyStatus;

        public FileServerStatusEventArgs(string status)
        {
            MyStatus = status;
        }

        public string Status
        {
            get { return MyStatus; }
        }
    }

	public class FileServer
	{
        private StreamWriter MyWriter;

        public delegate void FileServerStatusChangeHandler(FileServer fServerObject, FileServerStatusEventArgs status);

        public event FileServerStatusChangeHandler StatusChange;

		public FileServer()
		{
		}

        public StringCollection GetExperimentsInComparisonFile(string filePath)
        {
            return GetUniqueValuesInFile(filePath, 1);
        }

        public StringCollection GetItemsInComparisonFile(string filePath)
        {
            return GetUniqueValuesInFile(filePath, 0);
        }

		public string[] ReadSingleColumn(string filePath)
		{
			//Returns trimmed text lines from a file.
			StreamReader sr = null;
			string textLine;
			int lineCount;
			string [] stringArray;
			int i;

            try
            {
                //Count the number of items.
                sr = new StreamReader(filePath);
                lineCount = 0;
                while ((textLine = sr.ReadLine()) != null)
                {
                    if (textLine.Trim() != "")
                    {
                        lineCount++;
                    }
                }
                sr.Close();
                //Read the items.
                stringArray = new string[lineCount];
                sr = new StreamReader(filePath);
                i = 0;
                while ((textLine = sr.ReadLine()) != null)
                {
                    if (textLine.Trim() != "")
                    {
                        stringArray[i] = textLine.Trim();
                        i++;
                    }
                }
                return stringArray;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }


		}

		public string[][] ReadMultipleColumns(string filePath, int numberOfColumns)
		{
			//Returns rows (index 0) and columns (index 1) from a tab delimited text file.
			StreamReader sr = null;
			string textLine;
			int lineCount;
			string [][] colArray;
            string[] tempCols;
			int i;
			char[] delimiter;

            try
            {
                delimiter = new Char[1];

                delimiter[0] = (char)9;

                //Count the number of items.
                sr = new StreamReader(filePath);
                lineCount = 0;
                while ((textLine = sr.ReadLine()) != null)
                {
                    if (textLine.Trim() != "")
                    {
                        lineCount++;
                    }
                }
                sr.Close();
                //Read the items.
                colArray = new string[lineCount][];
                sr = new StreamReader(filePath);
                i = 0;
                while ((textLine = sr.ReadLine()) != null)
                {
                    textLine = textLine.Trim();
                    if (textLine != "")
                    {
                        tempCols = textLine.Split(delimiter, numberOfColumns);
                        if (tempCols.Length < numberOfColumns)
                        {
                            throw new Exception("Too few columns on line " + Convert.ToString(i + 1) + ".");
                        }
                        colArray[i] = tempCols;
                        i++;
                    }
                }
                return colArray;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

		}

		public bool SaveFile(string text, string filePath)
		{
			//Creates a nen file and saves the text string to the specified file.
            //Returns true on success.
			StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(filePath, false);
                sw.Write(text);
                sw.Flush();
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
		}

        public void OpenFileForWriting(string filePath)
        {
            MyWriter = new StreamWriter(filePath, false);
        }

        public void AppendStringToFile(string text)
        {
            MyWriter.Write(text);
        }

        public bool FileIsOpen
        {
            get
            {
                return (MyWriter != null);
            }
        }

        public void CloseFile()
        {
            if (MyWriter != null)
            {
                MyWriter.Flush();
                MyWriter.Close();
                MyWriter = null;
            }
        }

		public bool FileExists(string filePath)
		{
			//Returns true if the file exists, otherwise false.
			FileInfo fi;

			fi = new FileInfo(filePath);
			return fi.Exists;
		}

        private void OnStatusChange(FileServerStatusEventArgs status)
        {
            if (StatusChange != null)
            {
                StatusChange(this, status);
            }
        }

        private StringCollection GetUniqueValuesInFile(string filePath, int column)
        {
            //Returns a collection with all unique values found in the specified
            //column (the column index is zero-based). 
            StreamReader sr = null;
            string textLine, tempValue;
            string[] tempCols;
            char[] delimiter;
            int lineCounter;

            SQAT.ClientObjects.CIStringCollection values;

            try
            {
                delimiter = new Char[1];
                delimiter[0] = (char)9;

                lineCounter = 1;
                values = new Molmed.SQAT.ClientObjects.CIStringCollection();
                sr = new StreamReader(filePath);
                while ((textLine = sr.ReadLine()) != null)
                {
                    if (textLine.Trim() != "")
                    {
                        tempCols = textLine.Split(delimiter);
                        if (tempCols.Length - 1 < column)
                        {
                            throw new Exception("Too few columns on line " + lineCounter.ToString() + ".");
                        }
                        tempValue = tempCols[column].Trim();
                        if (!values.Contains(tempValue))
                        {
                            values.Add(tempValue);
                        }
                    }
                    lineCounter++;
                    if (lineCounter % 1000 == 0)
                    {
                        OnStatusChange(new FileServerStatusEventArgs("Scanned line " + lineCounter.ToString()));
                    }
                }
                return values.StringCollection;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                sr.Close();
            }

        }

	}
}