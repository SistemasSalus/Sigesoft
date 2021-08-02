using Sigesoft.Common;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace ReportesOtrasClinicas
{
   public class EnviarCorreo
    {
        private string _archivo { get; set; }
        private string _sede { get; set; }

        public EnviarCorreo(string archivo, string sede)
        {
            _archivo = archivo;
            _sede = sede;
        }


        public bool EnviarExcel()
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                //var correos = ObtenerCorreosPorSede();

                var configEmail = GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");
                //if (correos == "")
                //{
                //    return false; 
                //}
                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                string subject = "Resultados Covid-19";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos tardes, se adjunta excel con resultados COVID19");
                var atachFiles = new List<string>();
                string atachFile = _archivo;
                atachFiles.Add(atachFile);
                Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, "", "francisco.collantes@saluslaboris.com.pe", subject, message, atachFiles);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string ObtenerCorreosPorSede()
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
   
            var resultados = (from a in dbContext.location where a.v_OrganizationId == Constants.EMPRESA_BACKUS_ID && a.v_Name.ToUpper().Trim() == _sede.ToUpper().Trim() select a).ToList();

            var correos = "";
            foreach (var item in resultados)
            {
                if (item.v_Email != null)
                {
                    correos += item.v_Email + ";";    
                }
                
            }

            return correos;
           
        }

        public static List<KeyValueDTO> GetSystemParameterForCombo(ref OperationResult pobjOperationResult, int pintGroupId, string pstrSortExpression)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = from a in dbContext.systemparameter
                            where a.i_GroupId == pintGroupId && a.i_IsDeleted == 0
                            select a;

                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }
                else
                {
                    query = query.OrderBy("v_Value1");
                }

                var query2 = query.AsEnumerable()
                            .Select(x => new KeyValueDTO
                            {
                                Id = x.i_ParameterId.ToString(),
                                Value1 = x.v_Value1,
                                Value2 = x.v_Value2
                            }).ToList();

                pobjOperationResult.Success = 1;
                return query2;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }
    }
}
