﻿using HealthCareInfromationSystem.models.entity;
using HealthCareInfromationSystem.repository;
using HealthCareInfromationSystem.Servise;
using HealthCareInfromationSystem.utils;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareInfromationSystem.contollers
{
    class EquipmentRequestController
    {
        private EquipmentRequestService equipmentRequestService = new EquipmentRequestService();
        public void SendRequest(int equipmentId, int quantity)
        {
            equipmentRequestService.SendRequest(equipmentId, quantity);
        }



    }
}