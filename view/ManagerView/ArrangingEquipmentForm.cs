﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthCareInfromationSystem.view.ManagerView
{
    public partial class ArrangingEquipmentForm : Form
    {
        public ArrangingEquipmentForm()
        {
            InitializeComponent();
        }

        private void SetLabelsAndButtons()
        {
            label1.Text = "Current premise";
            label2.Text = "New premise";
            label3.Text = "Move date";
            textBox1.Enabled = false;
            button1.Text = "Move";
        }

        private bool CheckIfTextBoxesAreEmpty()
        {
            return String.IsNullOrWhiteSpace(textBox1.Text) ||
                    comboBox1.SelectedItem == null ||
                    String.IsNullOrWhiteSpace(comboBox1.SelectedItem.ToString()) ||
                    String.IsNullOrWhiteSpace(textBox2.Text);
        }

        private void FillComboBox()
        {
            using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
            {
                String query = "select premises_id, name from premises";

                OleDbCommand command = new OleDbCommand(query, connection);

                connection.Open();
                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader[0] + " - " + reader[1]);
                }
                reader.Close();
            }
        }

        private void FillTable(String query, OleDbConnection connection)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
            //OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(adapter);
            //BindingSource bindingSource = new BindingSource();
            //dataGridView1.DataSource = bindingSource;
            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };

            adapter.Fill(table);
            //bindingSource.DataSource = table;
            dataGridView1.DataSource = table;
        }

        private void RefreshTable()
        {
            using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
            {
                String query = $"" +
                    $"select eq.equipment_id, eq.name, eq.quantity, eq.type, pr1.name as old_premise, pr2.name as new_premise, eq.move_date as move_date " +
                    $"from equipment as eq, premises as pr1, premises as pr2 " +
                    $"where eq.old_premises_id=pr1.premises_id and eq.new_premises_id=pr2.premises_id";
                FillTable(query, connection);
            }
        }
    }
}
