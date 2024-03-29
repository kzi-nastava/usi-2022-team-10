﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthCareInfromationSystem.view.SecretaryView
{
    public partial class SecretaryMainForm : Form
    {
        public SecretaryMainForm()
        {
            InitializeComponent();
        }

        private void ManagePatientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManagePatientsForm managePatientsForm = new ManagePatientsForm();
            managePatientsForm.Show();
        }

        private void BlockedPatientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BlockedPatientsForm blockedPatientsForm = new BlockedPatientsForm();
            blockedPatientsForm.Show();
        }

        private void RequestsForChangeAndCancellationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppointmentRequestForm appointmentRequestForm = new AppointmentRequestForm();
            appointmentRequestForm.Show();
        }

        private void ByReferralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookingByReferralForm bookingByReferralForm = new BookingByReferralForm();
            bookingByReferralForm.Show();
        }

        private void EmergencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookingEmergency bookingEmergencyForm = new BookingEmergency();
            bookingEmergencyForm.Show();
        }

        private void AquirementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DynamicEquipmentAquirementForm dynamicEquipmentAquirementForm = new DynamicEquipmentAquirementForm();
            dynamicEquipmentAquirementForm.Show();
        }

        private void PlacementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArrangingDynamicEquipmentForm arrangingDynamicEquipmentForm = new ArrangingDynamicEquipmentForm();
            arrangingDynamicEquipmentForm.Show();
        }

        private void VacationRequestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VacationRequestForm vacationRequestForm = new VacationRequestForm();
            vacationRequestForm.Show();
        }
    }
}
