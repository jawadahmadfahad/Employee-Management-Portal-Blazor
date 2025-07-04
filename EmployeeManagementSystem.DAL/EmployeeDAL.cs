using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EmployeeManagementSystem.Entities;

namespace EmployeeManagementSystem.DAL
{
    public class EmployeeDAL
    {
        private readonly string _connectionString;

        public EmployeeDAL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ GET ALL
        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", conn);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                employees.Add(new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    FullName = reader["FullName"].ToString(),
                    Email = reader["Email"].ToString(),
                    HireDate = reader["HireDate"] as DateTime?
                });
            }

            return employees;
        }

        // ✅ INSERT
        public void AddEmployee(Employee emp)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("INSERT INTO Employees (FullName, Email, HireDate) VALUES (@FullName, @Email, @HireDate)", conn);
            cmd.Parameters.AddWithValue("@FullName", emp.FullName);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@HireDate", emp.HireDate ?? (object)DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ✅ UPDATE
        public void UpdateEmployee(Employee emp)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("UPDATE Employees SET FullName = @FullName, Email = @Email, HireDate = @HireDate WHERE EmployeeId = @EmployeeId", conn);
            cmd.Parameters.AddWithValue("@FullName", emp.FullName);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@HireDate", emp.HireDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ✅ DELETE
        public void DeleteEmployee(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("DELETE FROM Employees WHERE EmployeeId = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ✅ GET BY ID (Optional, for edit form)
        public Employee GetEmployeeById(int id)
        {
            Employee emp = null;

            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE EmployeeId = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                emp = new Employee
                {
                    EmployeeId = (int)reader["EmployeeId"],
                    FullName = reader["FullName"].ToString(),
                    Email = reader["Email"].ToString(),
                    HireDate = reader["HireDate"] as DateTime?
                };
            }

            return emp;
        }
    }
}
