﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareInfromationSystem.Core.MedicineManagment
{
	interface IMedicineRepo
	{
		List<Medicine> LoadAll();
		Dictionary<string, string> LoadNameAndId();
        Medicine LoadOneById(string medicineId);
		void Save(Medicine medicine);
		void Edit(Medicine medicine);
		List<Medicine> LoadDenied();
	}
}
