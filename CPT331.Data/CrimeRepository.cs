﻿#region Using References

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Dapper;

using CPT331.Core.ObjectModel;

#endregion

namespace CPT331.Data
{
	/// <summary>
	/// Represents a CrimeRepository type, used to manipulate local government area data.
	/// </summary>
	public static class CrimeRepository
	{
		/// <summary>
		/// The Crime.spAddCrime stored procedure name.
		/// </summary>
		public const string CrimeSpAddCrime = "Crime.spAddCrime";

		/// <summary>
		/// The Crime.spGetCrimeByID stored procedure name.
		/// </summary>
		public const string CrimeSpGetCrimeByID = "Crime.spGetCrimeByID";

		/// <summary>
		/// The Crime.spGetCrimesByCoordinate stored procedure name.
		/// </summary>
		public const string CrimeSpGetCrimesByCoordinate = "Crime.spGetCrimesByCoordinate";

		/// <summary>
		/// The Crime.spGetCrime stored procedure name.
		/// </summary>
		public const string CrimeSpGetCrime = "Crime.spGetCrime";

		/// <summary>
		/// The Crime.spUpdateCrime stored procedure name.
		/// </summary>
		public const string CrimeSpUpdateCrime = "Crime.spUpdateCrime";

		/// <summary>
		/// Inserts crime information into the underlying data source.
		/// </summary>
		/// <param name="count">Specifies the count of the offences.</param>
		/// <param name="localGovernmentAreaID">Specified the ID of the associated local government area.</param>
		/// <param name="month">Specifies the month of the crime.</param>
		/// <param name="offenceID">Specifies the ID of the associated offence.</param>
		/// <param name="year">Specifies the year of the crime.</param>
		/// <returns></returns>
		public static int AddCrime(int count, int localGovernmentAreaID, int month, int offenceID, int year)
		{
			int id = 0;

			using (SqlConnection sqlConnection = SqlConnectionFactory.NewSqlConnetion())
			{
				id = (int)SqlMapper
					.Query(sqlConnection, CrimeSpAddCrime, new { Count = count, LocalGovernmentAreaID = localGovernmentAreaID, Month = month, OffenceID = offenceID, Year = year }, commandType: CommandType.StoredProcedure)
					.Select(m => m.NewID)
					.Single();
			}

			return id;
		}

		/// <summary>
		/// Selects crime information from the underlying data source.
		/// </summary>
		/// <param name="id">The ID of the associated crime information.</param>
		/// <returns>Returns a Crime object representing the result of the operation.</returns>
		public static Crime GetCrimeByID(int id)
		{
			Crime crime = null;

			using (SqlConnection sqlConnection = SqlConnectionFactory.NewSqlConnetion())
			{
				crime = SqlMapper
					.Query(sqlConnection, CrimeSpGetCrimeByID, new { ID = id }, commandType: CommandType.StoredProcedure)
					.Select(m => new Crime(m.Count, m.DateCreatedUtc, m.DateUpdatedUtc, m.ID, m.IsDeleted, m.IsVisible, m.LocalGovernmentAreaID, m.Month, m.OffenceID, m.Year))
					.FirstOrDefault();
			}

			return crime;
		}

		/// <summary>
		/// Selects crime information from the underlying data source.
		/// </summary>
		/// <param name="latitude">Specifies the latitude used to look up the corresponding local government area.</param>
		/// <param name="longitude">Specifies the longitude used to look up the corresponding local government area.</param>
		/// <returns>Returns a list of Crime objects representing the result of the operation.</returns>
		public static List<CrimeByCoordinate> GetCrimesByCoordinate(double latitude, double longitude)
		{
			List<CrimeByCoordinate> crimeByCoordinates = null;

			using (SqlConnection sqlConnection = SqlConnectionFactory.NewSqlConnetion())
			{
				crimeByCoordinates = SqlMapper
					.Query(sqlConnection, CrimeSpGetCrimesByCoordinate, commandType: CommandType.StoredProcedure, param: new { Latitude = latitude, Longitude = longitude })
					.Select(m => new CrimeByCoordinate(m.BeginYear, m.EndYear, m.Name, m.OffenceCount, m.OffenceID, m.Offence))
					.ToList();
			}

			return crimeByCoordinates;
		}

		/// <summary>
		/// Gets all crime information from the database.
		/// </summary>
		/// <returns>Returns a list of Crime objects representing the result of the operation.</returns>
		public static List<Crime> GetCrimes()
		{
			List<Crime> crimes = null;

			using (SqlConnection sqlConnection = SqlConnectionFactory.NewSqlConnetion())
			{
				crimes = SqlMapper
					.Query(sqlConnection, CrimeSpGetCrime, commandType: CommandType.StoredProcedure)
					.Select(m => new Crime
					(
						m.Count,
						m.DdateCreatedUtc,
						m.DateUpdatedUtc,
						m.Id,
						m.IsDeleted,
						m.IsVisible,
						m.LocalGovernmentAreaID,
						m.Month,
						m.OffenceID,
						m.Year
					))
					.ToList();
			}

			return crimes;
		}

		/// <summary>
		/// Updates crime information into the underlying data source.
		/// </summary>
		/// <param name="id">The ID of the associated crime.</param>
		/// <param name="localGovernmentAreaID">Specified the ID of the associated local government area.</param>
		/// <param name="offenceID">Specifies the ID of the associated offence.</param>
		/// <param name="count">Specifies the count of the offences.</param>
		/// <param name="month">Specifies the month of the crime.</param>
		/// <param name="year">Specifies the year of the crime.</param>
		/// <param name="isDeleted">Specifies whether the crime information is flagged as deleted.</param>
		/// <param name="isVisible">Specifies whether the crime information is flagged as visible.</param>
		public static void UpdateCrime(int id, int localGovernmentAreaID, int offenceID, int count, int month, int year, bool isDeleted, bool isVisible)
		{
			using (SqlConnection sqlConnection = SqlConnectionFactory.NewSqlConnetion())
			{
				SqlMapper.Execute(sqlConnection, CrimeSpUpdateCrime, new
				{
					ID = id,
					Count = count,
					Month = month,
					Year = year,
					IsDeleted = isDeleted,
					IsVisible = isVisible,
				},
				commandType: CommandType.StoredProcedure);
			}
		}
	}
}
