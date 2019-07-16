using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;

namespace Simple.Hubs
{
	public class Employee
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Age { get; set; }
	}

	public interface IEmployeeRepository
	{
		List<Employee> GetAllEmployees();
	}

	public class EmployeeRepository : IEmployeeRepository
	{
		public EmployeeRepository(
			IConfiguration configuration,
			IHubContext<SignalServer> context)
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
			_context = context;
		}

		private readonly IHubContext<SignalServer> _context;
		readonly string connectionString = "";

		void Initialization()
		{
			// Create a dependency connection.
			//SqlDependency.Start(connectionString, queueName);
			SqlDependency.Start(connectionString);
		}

		public List<Employee> GetAllEmployees()
		{
			var employees = new List<Employee>();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();

				//SqlDependency.Start(connectionString);
				Initialization();

				string commandText = "select Id, Name, Age from dbo.Employees";

				SqlCommand cmd = new SqlCommand(commandText, conn);

				// Create a dependency and associate it with the SqlCommand.
				SqlDependency dependency = new SqlDependency(cmd);
				// Maintain the reference in a class member.

				// Subscribe to the SqlDependency event.
				dependency.OnChange += new OnChangeEventHandler(OnChangeNotification);
				dependency.OnChange += new OnChangeEventHandler(OnDependencyChange);

				// Execute the command.
				using (SqlDataReader reader = command.ExecuteReader())
				{
					// Process the DataReader.
				}


				var reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					var employee = new Employee
					{
						Id = Convert.ToInt32(reader["Id"]),
						Name = reader["Name"].ToString(),
						Age = Convert.ToInt32(reader["Age"])
					};

					employees.Add(employee);
				}
			}

			return employees;
		}

		private void OnChangeNotification(object sender, SqlNotificationEventArgs e)
		{
			_context.Clients.All.SendAsync("refreshEmployees");
		}


		// Handler method
		void OnDependencyChange(object sender, SqlNotificationEventArgs e)
		{
			// Handle the event (for example, invalidate this cache entry).
		}

		void Termination()
		{
			// Release the dependency.
			SqlDependency.Stop(connectionString, queueName);
		}

	}
}