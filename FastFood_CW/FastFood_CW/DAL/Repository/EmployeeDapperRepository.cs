using Dapper;
using FastFood_CW.DAL.Interface;
using FastFood_CW.DAL.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FastFood_CW.DAL.Repository
{
    // Students ID: 00013836, 00014725, 00014896
    public class EmployeeDapperRepository : IRepository<Employee>
    {
        private readonly string? _connStr;

        // Constructor
        public EmployeeDapperRepository(string? connStr)
        {
            _connStr = connStr;
        }

        // Create
        public async Task<int> CreateAsync(Employee entity)
        {
            using var conn = new SqlConnection(_connStr);
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                entity.FName,
                entity.LName,
                entity.Telephone,
                entity.Job,
                entity.Age,
                entity.Salary,
                entity.HireDate,
                entity.Image,
                entity.FullTime
            });
            parameters.Add(
                "@Errors",
                direction: ParameterDirection.Output,
                dbType: DbType.String,
                size: 1000);
            parameters.Add(
                "RetVal",
                direction: ParameterDirection.ReturnValue,
                dbType: DbType.Int32);

            int id = await conn.ExecuteScalarAsync<int>(
                "pEmployee_Create",
                commandType: CommandType.StoredProcedure,
                param: parameters
            );

            if (parameters.Get<int>("RetVal") != 1)
            {
                throw new Exception(parameters.Get<string>("Errors"));
            }

            return id;
        }

        // Delete
        public async Task DeleteAsync(Employee entity)
        {
            using var conn = new SqlConnection(_connStr);
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { entity.Employee_ID });
            parameters.Add(
                "@Errors",
                direction: ParameterDirection.Output,
                dbType: DbType.String,
                size: 1000);

            await conn.ExecuteAsync(
                "pEmployeeDelete",
                commandType: CommandType.StoredProcedure,
                param: parameters
            );

            string? errors = parameters.Get<string>("Errors");
            if (!string.IsNullOrWhiteSpace(errors))
            {
                throw new Exception(errors);
            }
        }

        // Get All
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connStr);
            return await conn.QueryAsync<Employee>(
                "pEmployee_GetAll", 
                commandType: CommandType.StoredProcedure);

        }

        // Get By Id
        public async Task<Employee> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connStr);
            var parameters = new DynamicParameters();
            parameters.Add("@Employee_ID", id); 

            Employee? employee = await conn.QueryFirstOrDefaultAsync<Employee>(
                "pEmployee_GetById",
                parameters,
                commandType: CommandType.StoredProcedure);

            return employee;
        }

        // Update
        public async Task UpdateAsync(Employee entity)
        {
            using var conn = new SqlConnection(_connStr);
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeID", entity.Employee_ID);
            parameters.Add("@FName", entity.FName);
            parameters.Add("@LName", entity.LName);
            parameters.Add("@Telephone", entity.Telephone);
            parameters.Add("@Job", entity.Job);
            parameters.Add("@Age", entity.Age);
            parameters.Add("@Salary", entity.Salary);
            parameters.Add("@HireDate", entity.HireDate);

            if (entity.Image != null)
            { 
                parameters.Add("@Image", entity.Image);
            }
            else
            {
                parameters.Add("@Image", DBNull.Value); 
            }

            parameters.Add("@FullTime", entity.FullTime);
            parameters.Add("@Errors", direction: ParameterDirection.Output, dbType: DbType.String, size: 1000);

            await conn.ExecuteAsync("pEmployee_Update", parameters, commandType: CommandType.StoredProcedure);

            string? errors = parameters.Get<string>("Errors");
            if (!string.IsNullOrWhiteSpace(errors))
            {
                throw new Exception(errors);
            }
        }
    }
}
