using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

		public List<Employee> GetAllEmployees()
		{
			var employees = new List<Employee>();

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();

				SqlDependency.Start(connectionString);

				string commandText = "select Id, Name, Age from dbo.Employees";

				SqlCommand cmd = new SqlCommand(commandText, conn);

				SqlDependency dependency = new SqlDependency(cmd);

				dependency.OnChange += new OnChangeEventHandler(DbChangeNotification);

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

		private async Task DbChangeNotification(object sender, SqlNotificationEventArgs e)
		{
			await _context.Clients.All.SendAsync("refreshEmployees").ConfigureAwait(false);
		}
	}
}