using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Controls;
using Accord.IO;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using Accord.MachineLearning.Bayes;
using System.Data;
using System.IO;

namespace ProjetMazoto.Class
{
    class Classification
    {
        public DataTable Algorithm(string path,string sheet)
        {
            if(path=="")
                 path = @"C:\Users\Chad\Documents\Visual Studio 2015\Projects\ProjetMazoto\ProjetMazoto\examples.xls";
            
           
            DataTable table = ExcelReader.ImportFile(path, "Classification - Ying Yang");
            // Convert the DataTable to input and output vectors
            double[][] inputs = table.ToJagged<double>("X","Y");
            
           // double[][] inputs = table.ToJagged<double>();
            int[] outputs = table.Columns["G"].ToArray<int>();

            // Plot the data
            ScatterplotBox.Show("Classification", inputs, outputs).Hold();
            return table;
           
        }
        public DataTable importExcel(string path)
        {
            if (path == "")
                path = @"E:\mazoto\projet\projet final\ProjetMazoto\ProjetMazoto\iris.xlsx";
            DataTable table = ExcelReader.ImportFile(path);
            table.AcceptChanges();
            return table;
        }
    }
}
