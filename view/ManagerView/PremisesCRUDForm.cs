﻿ 
using HealthCareInfromationSystem.utils;
using System;
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
using HealthCareInfromationSystem.Core.PremiseManagment;


namespace HealthCareInfromationSystem.view.ManagerView
{
    public partial class PremisesCRUDForm : Form
    {
        private PremiseService premiseService = new PremiseService();

        public PremisesCRUDForm()
        {
            InitializeComponent();
        }

        private void SetLabelsAndButtons()
        {
            label1.Text = "Premise ID";
            label2.Text = "Name";
            label3.Text = "Premise type";
            label4.Text = "";
            button1.Text = "Add new";
            button2.Text = "Edit";
            button3.Text = "Delete";
            button4.Text = "Clear";
        }

        private void FillComboBox()
        {
            comboBox1.Items.Add("operational room");
            comboBox1.Items.Add("examination room");
            comboBox1.Items.Add("rest room");
            comboBox1.Items.Add("other");
        }

        private void ResetTextBoxes()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
        }

        private bool ValidateForm()
        {
            return String.IsNullOrWhiteSpace(textBox1.Text) &&
                    String.IsNullOrWhiteSpace(textBox2.Text) &&
                    String.IsNullOrWhiteSpace(comboBox1.SelectedItem.ToString());
        }

        private void FillTable()
        {
            dataGridView1.DataSource = premiseService.LoadAll();
        }

        private void PremisesCRUDForm_Load(object sender, EventArgs e)
        {
            SetLabelsAndButtons();
            FillComboBox();
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            FillTable();
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow currentRow = dataGridView1.CurrentRow;
            if (currentRow == null) return;

            String premiseId = currentRow.Cells[0].Value.ToString();
            String name = currentRow.Cells[1].Value.ToString();
            String type = currentRow.Cells[2].Value.ToString();

            textBox1.Text = premiseId;
            textBox1.Enabled = false;
            textBox2.Text = name;
            comboBox1.SelectedItem = type;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateForm() ||
                !textBox1.Enabled ||
                premiseService.CheckIfPremiseExistsById(textBox1.Text))
                return;

            String premiseId = textBox1.Text;
            String name = textBox2.Text;
            String type = comboBox1.SelectedItem.ToString();

            Premise premise = new Premise(premiseId, name, type);
            premiseService.Save(premise);

            ResetTextBoxes();
            FillTable();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            ResetTextBoxes();
            textBox1.Enabled = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (ValidateForm() || textBox1.Enabled) return;

            String premiseId = textBox1.Text;
            String name = textBox2.Text;
            String type = comboBox1.SelectedItem.ToString();

            Premise premise = new Premise(premiseId, name, type);
            premiseService.Edit(premise);

            ResetTextBoxes();
            FillTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ValidateForm() || textBox1.Enabled) return;

            String premiseId = textBox1.Text;

            premiseService.Delete(premiseId);

            ResetTextBoxes();
            FillTable();
        }
    }
}
