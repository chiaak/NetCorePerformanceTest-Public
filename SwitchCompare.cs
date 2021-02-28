using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace NetCorePerformanceTest
{
    public class SwitchCompare : PerformanceBase, IDisposable
    {
        public void Dispose()
        {

        }
        public SwitchCompare()
        {
            arrTimes = new List<int>()
            { 5000, 50000, 100000, 500000 };
            iExecTimes = 20;
            arrTestFunction = new List<Tuple<string, emTestType, Func<int, Func<object>, string>>>()
            {
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Old_Generial" ,emTestType.Performance, OldGenerial) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("SwitchFunction" ,emTestType.Performance, SwitchFunction) ,
            };
        }

        public void Run()
        {
            PerformanceRunner runner = new PerformanceRunner(
                arrTimes, iExecTimes, null, arrTestFunction, null, Message);
            string[][] retResult = runner.Run();
            ShowResult(retResult);

            Console.ReadKey();

        }
        #region // OldGenerial

        private string OldGenerial(int iT, Func<object> GetDate)
        {
            SwitchNormal(iT);
            return "";
        }

        private void SwitchNormalNo(int iT)
        {
            SqlParameter _DataParameter;
            for (int i = 0; i < iT; i++)
            {
                string fieldName = rnd.Next(1, 5).ToString();
                string _DataParameterName = fieldName;
                switch (fieldName)
                {
                    case "1":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.NVarChar, 50);
                        _DataParameter.Direction = ParameterDirection.Input;
                        break;
                    case "2":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.NVarChar, 50);
                        _DataParameter.Direction = ParameterDirection.Input;
                        break;
                    case "3":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.DateTime);
                        _DataParameter.Direction = ParameterDirection.Input;
                        break;
                    case "4":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.Bit);
                        _DataParameter.Direction = ParameterDirection.Input;
                        break;
                    case "5":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.NVarChar, 50);
                        _DataParameter.Direction = ParameterDirection.Input;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(string.Format("傳入參數「fieldName」不存在「{0}」", fieldName));
                }
            }
        }
        #endregion

        private void SwitchNormal(int iT)
        {
            SqlParameter _DataParameter;
            for (int i = 0; i < iT; i++)
            {
                string fieldName = rnd.Next(1, 5).ToString();
                string _DataParameterName = fieldName;
                switch (fieldName)
                {
                    case "1":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.NVarChar, 50)
                        {
                            Direction = ParameterDirection.Input
                        };
                        break;
                    case "2":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.NVarChar, 50)
                        {
                            Direction = ParameterDirection.Input
                        };
                        break;
                    case "3":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.DateTime)
                        {
                            Direction = ParameterDirection.Input
                        };
                        break;
                    case "4":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Input
                        };
                        break;
                    case "5":
                        _DataParameter = new SqlParameter(_DataParameterName, SqlDbType.NVarChar, 50)
                        {
                            Direction = ParameterDirection.Input
                        };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(string.Format("傳入參數「fieldName」不存在「{0}」", fieldName));
                }
            }
        }
        Random rnd = new Random();

        #region // SwitchFunction

        private string SwitchFunction(int iT, Func<object> GetDate)
        {
            SwitchFunction(iT);
            return "";
        }

        private void SwitchFunction(int iT)
        {
            SqlParameter _DataParameter;
            for (int i = 0; i < iT; i++)
            {
                string fieldName = rnd.Next(1, 5).ToString();
                string _DataParameterName = fieldName;
                _DataParameter = fieldName switch
                {
                    "1" => new SqlParameter(_DataParameterName, SqlDbType.NVarChar, 50)
                    {
                        Direction = ParameterDirection.Input
                    },
                    "2" => new SqlParameter(_DataParameterName, SqlDbType.NVarChar, 50)
                    {
                        Direction = ParameterDirection.Input
                    },
                    "3" => new SqlParameter(_DataParameterName, SqlDbType.DateTime)
                    {
                        Direction = ParameterDirection.Input
                    },
                    "4" => new SqlParameter(_DataParameterName, SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Input
                    },
                    "5" => new SqlParameter(_DataParameterName, SqlDbType.NVarChar, 50)
                    {
                        Direction = ParameterDirection.Input
                    },
                    _ => throw new ArgumentOutOfRangeException(string.Format("傳入參數「fieldName」不存在「{0}」", fieldName)),
                };
            }
        }
        #endregion
    }
}
