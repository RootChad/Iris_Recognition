using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetMazoto.Class
{
    class ExcelReader
    {
        public static DataTable ImportFile(string path, string sheet)
        {
            string filePath = path;

            //Open file and returns as Stream
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader;

            //1. Reading Excel file
            if (Path.GetExtension(filePath).ToUpper() == ".XLS")
            {
                //1.1 Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                //1.2 Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
           
            //2. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            DataTable dt = result.Tables[sheet];
            result.Dispose();
            
            excelReader.Dispose();
            
            stream.Dispose();
            return dt;
        }
       
        public static DataTable ImportFile(string path)
        {
            string filePath = path;

            //Open file and returns as Stream
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
            IExcelDataReader excelReader;
            
            //1. Reading Excel file
            if (Path.GetExtension(filePath).ToUpper() == ".XLS")
            {
                //1.1 Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                //1.2 Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            //2. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            DataTable dt = result.Tables[0];
            result.Dispose();
            excelReader.Close();
            excelReader.Dispose();
            stream.Close();
            stream.Dispose();
            return dt;
        }
    }
}
