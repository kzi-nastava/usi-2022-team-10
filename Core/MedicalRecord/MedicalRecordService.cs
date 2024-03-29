﻿using System.Collections.Generic;
using HealthCareInfromationSystem.Core.MedicalRecord.MedicalPrescription;
using HealthCareInfromationSystem.Core.MedicalRecord.ReferralLetter;
using HealthCareInfromationSystem.Core.User;
 
using HealthCareInfromationSystem.Core.MedicineManagment;
using HealthCareInfromationSystem.repository;
using HealthCareInfromationSystem.User.Doctor;

namespace HealthCareInfromationSystem.Core.MedicalRecord
{
    class MedicalRecordService
	{
		IMedicalRecordRepo medicalRecordRepo = new MedicalRecordSQL();
		IPersonRepo personRepo = new PersonSQL();
		IReferralLetterRepo referealLeterRepo = new ReferralLetterSQL();
		ISpecialisationRepo specialisationRepo = new SpecialisationSQL();
		IMedicineRepo medicineRepo = new MedicineSQL();
		IPrescriptionRepo prescriptionRepo = new PrescriptionSQL();

		public MedicalRecord GetMedicalRecordByPatient(string patientId) {
			return medicalRecordRepo.GetMedicalRecordByPatient(patientId);
		}
		public void Edit(MedicalRecord medicalRecord) {
			medicalRecordRepo.Edit(medicalRecord);
		}

		public Dictionary<string, string> LoadFullNameAndId(string role) {
			return personRepo.LoadFullNameAndId(role);
		}
		public Person GetPersonById(string id)
		{
			return personRepo.LoadOnePerson(id);
		}

		public List<string> LoadSpecialisations() {
			return specialisationRepo.LoadSpecialisations();
		}
		public void Add(string patientId, string specialisation, string doctorId) {
			referealLeterRepo.Add(patientId, specialisation, doctorId);
		}

		public Dictionary<string, string> LoadMedicineNameAndId() {
			return medicineRepo.LoadNameAndId();
		}

		public Medicine GetMedicine(string id) {
			return medicineRepo.LoadOneById(id);
		}

		public void Add(MedicalPrescription.MedicalPrescription prescription) {
			prescriptionRepo.Add(prescription);
		}
	}
}
