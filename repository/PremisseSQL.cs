﻿using HealthCareInfromationSystem.models.entity;
using HealthCareInfromationSystem.utils;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HealthCareInfromationSystem.repository
{
	class PremisseSQL : IPremisseRepo
	{
		public void Delete(string id)
		{
			using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
			{
				connection.Open();
				String query = $"select old_premises_id, new_premises_id, move_date from equipment where old_premises_id = \"{id}\" or new_premises_id = \"{id}\"";
				OleDbCommand command = new OleDbCommand(query, connection);
				OleDbDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					String oldPremisesId = reader[0].ToString();
					String newPremisesId = reader[1].ToString();
					String date = reader[2].ToString();
					String today = $"{DateTime.Today.Day.ToString()}.{DateTime.Today.Month.ToString()}.{DateTime.Today.Year.ToString()}.";
					DateTime dateFromRepository = DateTime.Parse(date);
					DateTime dateToday = DateTime.Parse(today);

					int result = DateTime.Compare(dateFromRepository, dateToday);

					if (
						((result == -1 || result == 0) && newPremisesId == id) ||
						(result == 1 && oldPremisesId == id)
						)
					{
						MessageBox.Show("There is equipment in premise so you can't delete it.");
						return;
					}

					else if (result == 1 && newPremisesId == id)
					{
						MessageBox.Show("You can't delete premise because equipment is scheduled to be moved in it.");
						return;
					}
				}

				reader.Close();

				query = $"delete from premises where premises_id = \"{id}\"";
				command = new OleDbCommand(query, connection);
				command.ExecuteNonQuery();
			}
		}

		public void Edit(Premise premise)
		{
			using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
			{
				connection.Open();
				String query = $"update premises set name=\"{premise.Name}\", type=\"{premise.Type}\" where premises_id=\"{premise.Id}\"";
				OleDbCommand command = new OleDbCommand(query, connection);
				command.ExecuteNonQuery();
			}
		}

		public Premise GetPremiseById(string id)
		{
			using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
			{
				string query = $"select * from premises where premises_id=\"{id}\"";
				OleDbCommand command = new OleDbCommand(query, connection);

				connection.Open();
				OleDbDataReader reader = command.ExecuteReader();
				Premise premise = null;

				while (reader.Read())
				{
					premise = Premise.Parse(reader);

				}
				reader.Close();
				return premise;
			}
		}

		public Dictionary<string, string> LoadNameAndId()
		{
			using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
			{

				OleDbCommand command = new OleDbCommand("select premises_id, name from premises", connection);

				connection.Open();
				OleDbDataReader reader = command.ExecuteReader();
				Dictionary<string, string> premise = new Dictionary<string, string>();

				while (reader.Read())
				{
					premise.Add(reader[0].ToString(), reader[1].ToString());

				}
				reader.Close();
				return premise;
			}
		}

		public void Save(Premise premise)
		{
			using (OleDbConnection connection = new OleDbConnection(Constants.connectionString))
			{
				connection.Open();
				String query = $"insert into premises values (\"{premise.Id}\", \"{premise.Name}\", \"{premise.Type}\")";
				OleDbCommand command = new OleDbCommand(query, connection);
				command.ExecuteNonQuery();
			}
		}
	}
}