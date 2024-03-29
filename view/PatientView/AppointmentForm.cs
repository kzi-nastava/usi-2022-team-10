﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
 
using HealthCareInfromationSystem.utils;
using HealthCareInfromationSystem.Core.User;
using HealthCareInfromationSystem.Core.Appointment.AppointmentRequest;
using HealthCareInfromationSystem.Core.Appointment;
using static HealthCareInfromationSystem.Core.User.Person;
using HealthCareInfromationSystem.Core.PremiseManagment;
using static HealthCareInfromationSystem.Core.Appointment.Appointment;

namespace HealthCareInfromationSystem
{
    public partial class AppointmentForm : Form
    {
        private DateTime dateTime;
        private int doctor = -1;
        private readonly bool edit;
        private Appointment appointment;

        public AppointmentForm()
        {
            InitializeComponent();
            edit = false;
        }

        public AppointmentForm(Appointment app)
        {
            InitializeComponent();
            edit = true;
            appointment = app;
            FillForm(app);
        }

        public AppointmentForm(int doctor)
        {
            InitializeComponent();
            edit = false;
            this.doctor = doctor;
        }

        private void FillForm(Appointment app)
        {
            dateTimeTxt.Text = MyConverter.ToString(app.Beginning);
            dateTime = app.Beginning;
            doctor = app.Doctor.Id;
            FindAvailableDoctors();
        }

        private void CheckDateTime(string text)
        {
            try
            {
                dateTime = MyConverter.ToDateTime(text);
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Date format should be like: " + Constants.DateFmt);
            }
        }

        private bool ChangeableAppointment()
        {
            DateTime now = DateTime.Now;
            DateTime appDate = appointment.Beginning;
            return now.AddDays(2).CompareTo(appDate) <= 0;
        }



        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (edit == true)
            {
                AppointmentRequest request = new AppointmentRequest(BaseFunctions.GenerateId("appointment_request", "request_id"), LoggedInUser.loggedIn.Id.ToString(),
                        appointment.Id.ToString(), "edit", doctor.ToString(), MyConverter.ToString(dateTime));
                if (ChangeableAppointment())
                {
                    appointment.Beginning = MyConverter.ToDateTime(dateTimeTxt.Text);
                    appointment.Doctor = PersonController.LoadOnePerson(Constants.connectionString,
                            "select * from users where id=\"" + doctor.ToString() + "\"");
                    UpdateAppointment(appointment);
                    MessageBox.Show("You have successfully edited the appointment.");
                }
                else
                {
                    MessageBox.Show("Request change has been sent.");
                }
                InsertNewRequest(request);
            }
            else
            {
                int id = int.Parse(BaseFunctions.GenerateId("appointments", "ID"));
                Person patient = PersonController.SearchPerson(LoggedInUser.loggedIn.Id.ToString());
                Person doctor = PersonController.SearchPerson(this.doctor.ToString());
                Premise premise = PremiseController.SearchPremise("1");
                DateTime beginning = MyConverter.ToDateTime(dateTimeTxt.Text);
                AppointmentType type = AppointmentType.physical;
                int duration = 15;
                Appointment newApp = new Appointment(id, doctor, patient, premise, beginning, duration, type);
                InsertNewAppointment(newApp);
                MessageBox.Show("You have successfully created appointment.");
            }
        }


        private void UpdateAppointment(Appointment app)
        {
            using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
            {
                string query = @"UPDATE appointments
                            SET doctorId = @DoctorId,
                                beginning = @Beginning
                            WHERE ID = @AID";

                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@DoctorId", app.Doctor.Id.ToString());
                command.Parameters.AddWithValue("@Beginning", MyConverter.ToString(app.Beginning));
                command.Parameters.AddWithValue("@AID", app.Id.ToString());

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void InsertNewAppointment(Appointment app)
        {
            using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
            {
                string query = $"insert into appointments values (\"{app.Id}\", \"{app.Doctor.Id}\", \"{app.Patient.Id}\", " +
                    $"\"{app.Premise.Id}\", \"{MyConverter.ToString(app.Beginning)}\", \"{app.Duration}\", \"{app.Type}\", \"{app.Comment}\")";

                OleDbCommand command = new OleDbCommand(query, connection);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        private void InsertNewRequest(AppointmentRequest req)
        {
            using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
            {
                string query = $"insert into appointment_request values (\"{req.ID}\", \"{req.PatientId}\", \"{req.AppointmentId}\", " +
                    $"\"{req.Type}\", \"{req.NewDoctorId}\", \"{req.NewBeginning}\", \"{req.ReqDateTime}\", \"{req.State}\", \"{req.SecretaryId}\")";

                OleDbCommand command = new OleDbCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }



        private void FindAvailableDoctors()
        {
            string queryString = "select * from appointments";
            Dictionary<int, bool> doctors = new Dictionary<int, bool>();
            
            using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
            {
                OleDbCommand command = new OleDbCommand(queryString, connection);
                connection.Open();

                FindReservedDoctors(command, doctors);
                ShowAvailableDoctors(connection, doctors);

                connection.Close();
            }
            
        }

        private void ShowAvailableDoctors(OleDbConnection connection, Dictionary<int, bool> doctors)
        {
            string reservedDoctors = "(";
            foreach (KeyValuePair<int, bool> entry in doctors)
                if (entry.Value == false)
                    reservedDoctors += "\"" + entry.Key + "\",";
            if (reservedDoctors.Length > 1)
                reservedDoctors.Remove(reservedDoctors.Length - 1, 1);

            reservedDoctors += ")";
            Roles role = Roles.doctor;
            string query = reservedDoctors.Equals("()") ? $"select id, name, last_name from users where role=\"doctor\""
                : $"select id, name, last_name from users where role=\"doctor\" and id not in " + reservedDoctors;

            OleDbCommand command = new OleDbCommand(query, connection);
            OleDbDataAdapter da = new OleDbDataAdapter(command);
            DataTable dt = new DataTable();
            doctorsGrid.DataSource = dt;
            da.Fill(dt);
        }

        private void FindReservedDoctors(OleDbCommand command, Dictionary<int, bool> doctors)
        {
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int doctorId = Convert.ToInt32(reader[1]);
                DateTime appDateTime1 = MyConverter.ToDateTime(reader[4].ToString());   // appointment begin
                int duration = Convert.ToInt32(reader[5].ToString());

                DateTime appDateTime2 = appDateTime1.AddMinutes(duration);      // appointment end
                DateTime dateTime2 = this.dateTime.AddMinutes(15);          // newAppointment end

                if ( ((DateTime.Compare(dateTime, appDateTime1) >= 0) && (DateTime.Compare(dateTime, appDateTime2) <= 0))
                    || ((DateTime.Compare(dateTime2, appDateTime1) >= 0) && (DateTime.Compare(dateTime2, appDateTime2) <= 0)) )
                {
                    doctors[doctorId] = false;   // doctor is not available
                }
                else
                {
                    if (!doctors.ContainsKey(doctorId))
                        doctors.Add(doctorId, true);
                }
            }
            reader.Close();
        }

        private void ShowDoctorsButton_Click(object sender, EventArgs e)
        {
            CheckDateTime(dateTimeTxt.Text);
            FindAvailableDoctors();
        }

        private void DoctorsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.doctorsGrid.Rows[e.RowIndex];
                doctor = Convert.ToInt32(row.Cells["id"].Value);
                MessageBox.Show("Doctor selected.");
            }
        }
    }
}
