using System;
using Beyova.ProgrammingIntelligence;
using Beyova.ApiTracking;
using Beyova.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
using Beyova.ExceptionSystem;

namespace Beyova.Common.UnitTest
{
    [TestClass]
    public class SqlExceptionHandleUnitTest
    {
        const string sqlConnection = "Data Source=.;Initial Catalog=SqlTest;Integrated Security=False;";

        #region Test Classes

        public class SqlTextObject
        {
            public int Column1;
            public double Column2;
            public string Column3;
            public DateTime? Column4;
        }

        class TestDataAccessController : SqlDataAccessController<SqlTextObject>
        {
            public TestDataAccessController() : base(sqlConnection) { }

            protected override SqlTextObject ConvertEntityObject(SqlDataReader sqlDataReader)
            {
                return new SqlTextObject
                {
                    Column1 = sqlDataReader["Column1"].ObjectToInt32(),
                    Column2 = sqlDataReader["Column2"].ObjectToDouble(),
                    Column3 = sqlDataReader["Column3"].ObjectToString(),
                    Column4 = sqlDataReader["Column4"].ObjectToDateTime()
                };
            }

            public List<SqlTextObject> TestReader(bool withException)
            {
                string spName = "sp_ExecuteReaderTest" + (withException ? "WithException" : "");

                try
                {
                    return ExecuteReader(spName, null);
                }
                catch (Exception ex)
                {
                    throw ex.Handle("TestReader");
                }
            }

            public DateTime? TestScalar(bool withException)
            {
                string spName = "sp_ExecuteScalar" + (withException ? "WithException" : "");

                try
                {
                    return ExecuteScalar(spName, null).ObjectToDateTime();
                }
                catch (Exception ex)
                {
                    throw ex.Handle("TestScalar");
                }
            }

            public void TestNonQuery(bool withException)
            {
                string spName = "sp_ExecuteNonQuery" + (withException ? "WithException" : "");

                try
                {
                    ExecuteNonQuery(spName, null);
                }
                catch (Exception ex)
                {
                    throw ex.Handle("TestNonQuery");
                }
            }
        }

        #endregion

        [TestInitialize]
        public void Prepare()
        {
            SqlDataAccessController.InitializeCustomizedSqlErrorStoredProcedure(sqlConnection);
        }

        [TestMethod]
        public void Test()
        {
            using (var controller = new TestDataAccessController())
            {
                var objs = controller.TestReader(false);
                Assert.IsNotNull(objs);
                Assert.IsNotNull(objs.SafeFirstOrDefault());
            }

            try
            {
                using (var controller = new TestDataAccessController())
                {
                    var objs = controller.TestReader(true);
                    Assert.IsNull(objs);
                }
            }
            catch (Exception ex)
            {
                var sqlEx = ex.RootException() as SqlStoredProcedureException;
                Assert.IsNotNull(sqlEx);
                Assert.AreEqual(ExceptionCode.MajorCode.OperationFailure, sqlEx.Code.Major);
            }

            using (var controller = new TestDataAccessController())
            {
                var obj = controller.TestScalar(false);
                Assert.IsNotNull(obj);
            }

            try
            {
                using (var controller = new TestDataAccessController())
                {
                    var obj = controller.TestScalar(true);
                    Assert.IsNull(obj);
                }
            }
            catch (Exception ex)
            {
                var sqlEx = ex.RootException() as SqlStoredProcedureException;
                Assert.IsNotNull(sqlEx);
                Assert.AreEqual(ExceptionCode.MajorCode.OperationForbidden, sqlEx.Code.Major);
            }

            using (var controller = new TestDataAccessController())
            {
                controller.TestNonQuery(false);
            }

            try
            {
                using (var controller = new TestDataAccessController())
                {
                    controller.TestNonQuery(true);
                }
            }
            catch (Exception ex)
            {
                var sqlEx = ex.RootException() as SqlStoredProcedureException;
                Assert.IsNotNull(sqlEx);
                Assert.AreEqual(ExceptionCode.MajorCode.OperationFailure, sqlEx.Code.Major);
            }
        }
    }
}