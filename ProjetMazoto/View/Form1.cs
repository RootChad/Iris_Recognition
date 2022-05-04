using Accord.IO;
using Accord.Math;
using Accord.Statistics.Analysis;
using ProjetMazoto.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetMazoto
{
    public partial class Form1 : Form
    {
        private string path="";
        private DataTable data;
        private Accord.Neuro.ActivationNetwork network;
        public Form1()
        {
            InitializeComponent();
            
            openFileDialog1 = new OpenFileDialog()
            {
                FileName = "iris.xlsx",
                Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx",
                Title = "Open excel file"
            };

            button2.Click += new EventHandler(selectButton_Click);
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filePath = openFileDialog1.FileName;
                    
                    using (Stream str = openFileDialog1.OpenFile())
                    {
                        path = filePath;
                        str.Dispose();
                    }
                    
                    Classification algo = new Classification();
                    // algo.Algorithm(path, "");
                    data = algo.importExcel(path);
                    dataGridView1.DataSource = data;
                    dataGridView1.Update();
                    dataGridView1.Refresh();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Classification algo = new Classification();
             algo.Algorithm(path, "");
            data = algo.importExcel(path);
            dataGridView1.DataSource = data;
            dataGridView1.Update();
            dataGridView1.Refresh();


        }
       
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (data != null)
            {
                Learning learn = new Learning();
                if (alphaText.Text == "")
                    alphaText.Text = "1";
                if (seuilText.Text == "")
                    seuilText.Text = "1";
                this.network = learn.Trainig(data, int.Parse(alphaText.Text), int.Parse(seuilText.Text));
                iterationBox.Text = Learning.iterationNb.ToString();
            }
            else
                MessageBox.Show("Veuillez importer un fichier");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                MessageBox.Show("Erreur inserer quelque chose");
            else
            {
                double SepalL = Convert.ToDouble(textBox1.Text);
                double SepalW = Convert.ToDouble(textBox2.Text);
                double PetalL = Convert.ToDouble(textBox3.Text);
                double PetalW = Convert.ToDouble(textBox4.Text);



                double[] inputs = { SepalL, SepalW, PetalL, PetalW };
                Double[] output = this.network.Compute(inputs);

                Learning learn = new Learning();
                int index = learn.ConvertFormatOutput(output);
                resultTxt.Text = learn.ConvertIntToString(index);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (data != null)
            {
                Learning l = new Learning();
                double[][] inputs = data.ToJagged<double>("SepalLength", "SepalWidth", "PetalLength", "PetalWidth");
                double[][] outputs = data.ToJagged<double>("Setosa", "Versicolor", "Virginica");

                int[] expected = new int[outputs.Length];
                for (int i = 0; i < outputs.Length; i++)
                {
                    expected[i] = l.ConvertFormatOutput(outputs[i]);
                }

                int[] predicted = new int[outputs.Length];
                for (int i = 0; i < inputs.Length; i++)
                {
                    Double[] temp = this.network.Compute(inputs[i]);
                    predicted[i] = l.ConvertFormatOutput(temp);
                }

                int classes = 3;

                GeneralConfusionMatrix cm2 = new GeneralConfusionMatrix(classes, expected, predicted);
                int[,] matrix = cm2.Matrix;

                Console.WriteLine("Affichage matrice de confusion:");
                for (int i = 0; i < classes; i++)
                {
                    for (int j = 0; j < classes; j++)
                    {
                        if (j != (classes - 1))
                        {
                            Console.Write(matrix[i, j] + "//");
                        }
                        else
                        {
                            Console.Write(matrix[i, j]);
                        }
                    }
                    Console.WriteLine();
                }

                Double accuracy = cm2.Accuracy;
                Double pourcent = accuracy * 100;
                // Console.WriteLine("accuracy2 :" + pourcent + "%");
                pourcentage.Text = "Pourcentage : " + pourcent + "%";

                dataGridView2.ColumnCount = 4;
                dataGridView2.Columns[0].Name = "Pred/Exp";
                dataGridView2.Columns[1].Name = "Setoza";
                dataGridView2.Columns[2].Name = "Versicolor";
                dataGridView2.Columns[3].Name = "Virginica";

                dataGridView2.Rows.Clear();

                string[] row = new string[] { "Setoza", matrix[0, 0].ToString(), matrix[0, 1].ToString(), matrix[0, 2].ToString() };
                dataGridView2.Rows.Add(row);
                row = new string[] { "Versicolor", matrix[1, 0].ToString(), matrix[1, 1].ToString(), matrix[1, 2].ToString() };
                dataGridView2.Rows.Add(row);
                row = new string[] { "Virginica", matrix[2, 0].ToString(), matrix[2, 1].ToString(), matrix[2, 2].ToString() };
                dataGridView2.Rows.Add(row);
            }
            else
                MessageBox.Show("Veuillez d'abord faire un apprentissage");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.network.Save(Path.Combine("D:\\EtudesMaster\\Mazoto\\ProjetMazoto\\", "save.data"));
            MessageBox.Show("Sauvegarde Reussie");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.network = Serializer.Load<Accord.Neuro.ActivationNetwork>(Path.Combine("D:\\EtudesMaster\\Mazoto\\ProjetMazoto\\", "save.data"));
            MessageBox.Show("Chargement Reussie");
        }
    }
}
