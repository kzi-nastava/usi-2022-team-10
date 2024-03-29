﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCareInfromationSystem.Core.User;
using System.Data.OleDb;
using System.Windows.Forms;
using HealthCareInfromationSystem.utils;
using HealthCareInfromationSystem.Servise;
using HealthCareInfromationSystem.Core.PremiseManagment;

namespace HealthCareInfromationSystem.Core.Appointment
{
	
    public class Appointment
    {
        
        public enum AppointmentType
        {
            physical,
            operation
        }

        private int _id;
        private Person _patient;
        private Person _doctor;
        private Premise _premise;
        private DateTime _beginning;
        private AppointmentType _type;
        private int _duration;
        private string _comment;



        public Appointment()
        {
            
        }

        public Appointment(int id, Person doctor, Person patient, Premise premise,
			DateTime beginning, int duration, AppointmentType operation, string comment="")
		  {
            _id = id;
            _patient = patient;
            _doctor = doctor;
            _premise = premise;
            _beginning = beginning;
            _type = operation;
            _duration = duration;
            _comment = comment;
        }


        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public AppointmentType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public DateTime Beginning
        {
            get { return _beginning; }
            set { _beginning = value; }
        }

        public Person Doctor
        {
            get { return _doctor; }
            set { _doctor = value; }
        }


        public Person Patient
        {
            get { return _patient; }
            set { _patient = value; }
        }

        public Premise Premise
        {
            get { return _premise; }
            set { _premise = value; }
        }


        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }


        public String Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        

        public static Appointment Parse(OleDbDataReader reader)
        {
            AppointmentService appointmentService = new AppointmentService();
            int id = int.Parse(reader[0].ToString());
            Person doctor = appointmentService.GetPersonById(reader[1].ToString());
            Person patient = appointmentService.GetPersonById(reader[2].ToString());
            Premise premise = appointmentService.GetPremiseById(reader[3].ToString());
            DateTime beginning = DateTime.ParseExact(reader[4].ToString(), "dd.MM.yyyy. HH:mm", null);
            int duration = int.Parse(reader[5].ToString());
            Enum.TryParse(reader[6].ToString(), out AppointmentType type);
            string comment = reader[7].ToString();
            //Console.WriteLine(comment);
            return new Appointment(id, doctor, patient, premise, beginning, duration, type, comment);
        }

        public static Appointment Parse(DataGridViewRow row)
        {
            int id = int.Parse(row.Cells["ID"].Value.ToString());
            Person doctor = PersonController.LoadOnePerson(Constants.connectionString,
                            "select * from users where id=\"" + row.Cells["doctorId"].Value.ToString() + "\"");
            Person patient = PersonController.LoadOnePerson(Constants.connectionString,
                            "select * from users where id=\"" + row.Cells["patientId"].Value.ToString() + "\"");
            Premise premise = PremiseController.LoadOnePremise(Constants.connectionString,
                            "select * from premises where premises_id=\"" + row.Cells["premiseId"].Value.ToString() + "\"");
            DateTime beginning = DateTime.ParseExact(row.Cells["beginning"].Value.ToString(), "dd.MM.yyyy. HH:mm", null);
            int duration = int.Parse(row.Cells["duration"].Value.ToString());
            Enum.TryParse(row.Cells["type"].Value.ToString(), out AppointmentType type);
            string comment = row.Cells["comment"].Value.ToString();
            Console.WriteLine(comment);
            return new Appointment(id, doctor, patient, premise, beginning, duration, type, comment);
        }


        public override string ToString()
        {
            return _beginning + "  -  " + _doctor;
        }

    }

}
