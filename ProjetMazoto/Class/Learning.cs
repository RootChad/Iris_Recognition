using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetMazoto.Class
{
    class Learning
    {
        public static int iterationNb = 0;
        //Fonction d'apprentissage
        public ActivationNetwork Trainig(DataTable data, double alpha, double seuil)
        {

            //Recuperation des donnees du tableau d'excel (entree et sortie)
            double[][] inputs = data.ToJagged<double>("SepalLength","SepalWidth","PetalLength","PetalWidth");
            double[][] outputs = data.ToJagged<double>("Setosa", "Versicolor","Virginica") ;
            //Donnee de prediction
            double[][] predicted = new double[outputs.Length][];

            bool stop = false;

            //Creation du reseau de neurone
             ActivationNetwork network = new ActivationNetwork(
             new SigmoidFunction(alpha),
             4, // two inputs in the network
             3, // two neurons in the first layer
             2, 
             3) ; // one neuron in the second layer

            BackPropagationLearning teacher = new BackPropagationLearning(network);
            
            
            //Les fichiers de sauvegarde
            StreamWriter errorsFile = null;
            StreamWriter weightsFile = null;

          //  Double seuil = 1 ;
            try
            {

                errorsFile = File.CreateText("errors.csv");
                weightsFile = File.CreateText("weights.csv");
                
                iterationNb = 0;
                while (!stop)
                {

                    // Sauvegarde des poids
                    //-------------------
                    // run epoch of learning procedure
                    double error = teacher.RunEpoch(inputs, outputs);
                  


                    if (error < seuil)
                    {
                        stop = true;
                    }
                    Console.WriteLine("error:" + error);
                    iterationNb++;
                }
                MessageBox.Show("Succes de l'apprentissage!");
               
            }
            catch (IOException)
            {
                MessageBox.Show("Failed writing file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // close files
                if (errorsFile != null)
                    errorsFile.Close();
                if (weightsFile != null)
                    weightsFile.Close();
            }
            return network;
        }

        public void ConfusionMatrix()
        {

        }

        public int ConvertFormatOutput(Double[] output)
        {
            int val=0;
            for(int i = 0; i < output.Length; i++)
            {
                if (Convert.ToInt32(output[i]) == 1)
                {
                    val = i;
                }
                else
                {
                    continue;
                }
            }
            return val;
        }
        public String ConvertIntToString(int i)
        {
            if (i == 0)
            {
                return "Setosa";
            }
            else if (i == 1)
            {
                return "Versicolor";
            }
            else return "Virginica";
        }
        public double[][] ConvertDataTableToMatrix(DataTable dt)
        {

            double[][] matrix = new double[dt.Rows.Count][];
            Converter<object, double> converter = Convert.ToDouble;
            
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                matrix[row] = Array.ConvertAll(dt.Rows[row].ItemArray, converter);
            }

            return matrix;
        }

    }
}
