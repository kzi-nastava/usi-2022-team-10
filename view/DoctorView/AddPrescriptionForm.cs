﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HealthCareInfromationSystem.Core.MedicalRecord.MedicalPrescription;
using HealthCareInfromationSystem.Core.User;
using HealthCareInfromationSystem.utils;
using HealthCareInfromationSystem.Core.MedicineManagment;
 
using HealthCareInfromationSystem.Core.MedicalRecord;
using HealthCareInfromationSystem.Servise;

namespace HealthCareInfromationSystem.view.DoctorView
{
	public partial class AddPrescriptionForm : Form
	{
		MedicalRecordController medicalRecordController = new MedicalRecordController();
		private string patientId;
		private Person patient;

		public AddPrescriptionForm()
		{
			InitializeComponent();
		}

		public AddPrescriptionForm(int patientId)
		{
			InitializeComponent();
			LoadPatient(patientId);
			FillMedicineComboBox();
		}

		private void FillMedicineComboBox()
		{
			Dictionary<string, string> medicinePair = medicalRecordController.LoadMedicineNameAndId();
			medicineComboBox.DataSource = new BindingSource(medicinePair, null);
			medicineComboBox.DisplayMember = "Value";
			medicineComboBox.ValueMember = "Key";

			foreach (string type in Enum.GetNames(typeof(Medicine.DrinkingPeriod)))
			{
				periodComboBox.Items.Add(type);
				periodComboBox.SelectedItem = type;
			}
		}

		private void LoadPatient(int id)
		{
			patientId = id.ToString();
			Person patient = medicalRecordController.GetPersonById(patientId);
			this.patient = patient;
			patientFullNameLabel.Text = patient.FirstName + " " + patient.LastName;
		}

		private void SaveClick(object sender, EventArgs e)
		{
			MedicalRecord medical = medicalRecordController.GetMedicalRecordByPatient(patientId);
			string medicineId = medicineComboBox.SelectedValue.ToString();
			Medicine medicine = medicalRecordController.GetMedicine(medicineId);
			string time = periodTextBox.Text;
			string quantity = quantityTextBox.Text;
			string date = dateTextBox.Text;

			if (!IsDateVAlid(date)) return;
			if (!IsTimeVAlid(time)) return;

			if (IsPatientAlergic(medical, medicine)) return;
			MedicalPrescription medicalPrescription = new MedicalPrescription(0, medicine, quantity, DateTime.ParseExact(time, "HH:mm", null), patient, DateTime.Now, DateTime.ParseExact(date, "dd.MM.yyyy.", null));

			SaveChanges(medicalPrescription);

		}

		private void SaveChanges(MedicalPrescription medicalPrescription)
		{
			DialogResult dialogResult = MessageBox.Show("Are you sure you want to save changes?", "Check", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				MessageBox.Show("Changes saved.", "Success");

				medicalRecordController.Add(medicalPrescription);
			}
		}

		private bool IsPatientAlergic(MedicalRecord medical, Medicine medicine) {
			if (medical.IsAlergic(medicine.Ingredients))
			{
				MessageBox.Show("Patient is alergic to an ingredient", "Error");
				return true;
			}
			return false;
		}

		private bool IsDateVAlid(string date) {
			try
			{
				DateTime dateCheck = DateTime.ParseExact(date, "dd.MM.yyyy.", null);
				return true;
			}
			catch
			{
				MessageBox.Show("Please check date field and enter correct values.", "Error");
				return false;
			}
		}

		private bool IsTimeVAlid(String time)
		{
			try
			{
				DateTime timeTaking = DateTime.ParseExact(time, "HH:mm", null);
				return true;
			}
			catch
			{
				MessageBox.Show("Please check time field and enter correct values.", "Error");
				return false;
			}
		}

		private void CancelClick(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
