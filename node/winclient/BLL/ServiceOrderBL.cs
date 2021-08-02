using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Dynamic;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.DAL;
using Sigesoft.Common;
using System.Collections;

namespace Sigesoft.Node.WinClient.BLL
{
    public class ServiceOrderBL
    {
        public List<ServiceOrderList> GetServiceOrderPagedAndFiltered(ref OperationResult pobjOperationResult, int? pintPageIndex, int? pintResultsPerPage, string pstrSortExpression, string pstrFilterExpression, DateTime? pdatBeginDate, DateTime? pdatEndDate)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from A in dbContext.serviceorder
                            join B in dbContext.systemparameter on new { a = A.i_ServiceOrderStatusId.Value, b = 194 } equals new { a = B.i_ParameterId, b = B.i_GroupId }
                            join C in dbContext.serviceorderdetail on  A.v_ServiceOrderId equals C.v_ServiceOrderId

                            join E in dbContext.protocol on C.v_ProtocolId equals E.v_ProtocolId

                            // Empresa / Sede Cliente ******************************************************
                            join oc in dbContext.organization on new { a = E.v_CustomerOrganizationId }
                                    equals new { a = oc.v_OrganizationId } into oc_join
                            from oc in oc_join.DefaultIfEmpty()

                            join lc in dbContext.location on new { a = E.v_CustomerOrganizationId, b = E.v_CustomerLocationId }
                                  equals new { a = lc.v_OrganizationId, b = lc.v_LocationId } into lc_join
                            from lc in lc_join.DefaultIfEmpty()

                            // Empresa / Sede Empleadora *******************************************
                            join oe in dbContext.organization on new { a = E.v_EmployerOrganizationId }
                                  equals new { a = oe.v_OrganizationId } into oe_join
                            from oe in oe_join.DefaultIfEmpty()

                            join le in dbContext.location on new { a = E.v_EmployerOrganizationId, b = E.v_EmployerLocationId }
                                   equals new { a = le.v_OrganizationId, b = le.v_LocationId } into le_join
                            from le in le_join.DefaultIfEmpty()

                            // Empresa / Sede Trabajo *******************************************
                            join ow in dbContext.organization on new { a = E.v_WorkingOrganizationId }
                                  equals new { a = ow.v_OrganizationId } into ow_join
                            from ow in ow_join.DefaultIfEmpty()

                            join lw in dbContext.location on new { a = E.v_WorkingOrganizationId, b = E.v_WorkingLocationId }
                                   equals new { a = lw.v_OrganizationId, b = lw.v_LocationId } into lw_join
                            from lw in lw_join.DefaultIfEmpty()
                            // *****************************************************************************

                            join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                            equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                            equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()
                            where A.i_IsDeleted == 0
                            select new ServiceOrderList
                            {
                             v_ServiceOrderId = A.v_ServiceOrderId,
                             v_CustomServiceOrderId = A.v_CustomServiceOrderId,
                             v_Description = A.v_Description,
                             v_Comentary = A.v_Comentary,
                             i_NumberOfWorker = A.i_NumberOfWorker,
                             r_TotalCost = A.r_TotalCost,
                             d_DeliveryDate= A.d_DeliveryDate,

                             i_ServiceOrderStatusId = A.i_ServiceOrderStatusId,
                             v_ServiceOrderStatusName = B.v_Value1,
                             v_CreationUser = J1.v_UserName,
                             v_UpdateUser = J2.v_UserName,
                             d_CreationDate = A.d_InsertDate,
                             d_UpdateDate = A.d_UpdateDate,
                             v_ProtocolId = C.v_ProtocolId,

                             v_CustomerOrganizationId = E.v_CustomerOrganizationId,
                             v_CustomerLocationId = E.v_CustomerLocationId,
                             v_EmployerOrganizationId = E.v_EmployerOrganizationId,
                             v_EmployerLocationId = E.v_EmployerLocationId,
                             v_WorkingOrganizationId = E.v_WorkingOrganizationId,
                             v_WorkingLocationId = E.v_WorkingLocationId
                            };

                if (!string.IsNullOrEmpty(pstrFilterExpression))
                {
                    query = query.Where(pstrFilterExpression);
                }
                if (pdatBeginDate.HasValue && pdatEndDate.HasValue)
                {
                    query = query.Where("d_DeliveryDate >= @0 && d_DeliveryDate <= @1 || d_DeliveryDate == NULL", pdatBeginDate.Value, pdatEndDate.Value);
                }
                if (!string.IsNullOrEmpty(pstrSortExpression))
                {
                    query = query.OrderBy(pstrSortExpression);
                }
                if (pintPageIndex.HasValue && pintResultsPerPage.HasValue)
                {
                    int intStartRowIndex = pintPageIndex.Value * pintResultsPerPage.Value;
                    query = query.Skip(intStartRowIndex);
                }
                if (pintResultsPerPage.HasValue)
                {
                    query = query.Take(pintResultsPerPage.Value);
                }

                List<ServiceOrderList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return objData;

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public string AddServiceOrder(ref OperationResult pobjOperationResult, serviceorderDto pobjDtoEntity, serviceorderdetailDto pobjDtoEntityDetail , List<string> ClientSession)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                #region Service Order
                serviceorder objEntity = serviceorderAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla
                int intNodeId = int.Parse(ClientSession[0]);
                NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 101), "YY");
                objEntity.v_ServiceOrderId = NewId;

                dbContext.AddToserviceorder(objEntity);
                #endregion

                #region Service Order Detail

                serviceorderdetail objEntityDetail = serviceorderdetailAssembler.ToEntity(pobjDtoEntityDetail);

                objEntityDetail.d_InsertDate = DateTime.Now;
                objEntityDetail.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntityDetail.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla
                objEntityDetail.v_ServiceOrderId = NewId;
                objEntityDetail.v_ServiceOrderDetailId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 102), "YA");

                dbContext.AddToserviceorderdetail(objEntityDetail);
              

                #endregion

                dbContext.SaveChanges();
                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "ORDERN SERVICIO", "v_ServiceOrderId=" + NewId.ToString(), Success.Ok, null);
                return NewId;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "ORDEN SERVICIO", "v_ServiceOrderId=" + NewId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
                return null;
            }
        }

        public void UpdateService(ref OperationResult pobjOperationResult, serviceorderDto pobjDtoEntity, serviceorderdetailDto pobjDtoEntityDetail, List<string> ClientSession)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                #region ServiceOrder
                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.serviceorder
                                       where a.v_ServiceOrderId == pobjDtoEntity.v_ServiceOrderId && a.i_IsDeleted == 0
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                pobjDtoEntity.d_UpdateDate = DateTime.Now;
                pobjDtoEntity.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                serviceorder objEntity = serviceorderAssembler.ToEntity(pobjDtoEntity);

                // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                dbContext.serviceorder.ApplyCurrentValues(objEntity);

                #endregion

                #region ServiceOrderDetail
                // Obtener la entidad fuente
                var objEntitySourceDetail = (from a in dbContext.serviceorderdetail
                                             where a.v_ServiceOrderId == pobjDtoEntityDetail.v_ServiceOrderId && a.i_IsDeleted ==0
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                pobjDtoEntityDetail.d_UpdateDate = DateTime.Now;
                pobjDtoEntityDetail.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                serviceorderdetail objEntityDetail = serviceorderdetailAssembler.ToEntity(pobjDtoEntityDetail);

                // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                dbContext.serviceorderdetail.ApplyCurrentValues(objEntityDetail);

                #endregion
               
                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "ORDEN SERVICIO", "v_ServiceOrderId=" + objEntity.v_ServiceOrderId.ToString(), Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "SERVICIO", "v_ServiceOrderId=" + pobjDtoEntity.v_ServiceOrderId.ToString(), Success.Failed, pobjOperationResult.ExceptionMessage);
                return;
            }
        }

        public serviceorderDto GetServiceOrder(ref OperationResult pobjOperationResult, string pstrServiceId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                serviceorderDto objDtoEntity = null;

                var objEntity = (from a in dbContext.serviceorder
                                 where a.v_ServiceOrderId == pstrServiceId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = serviceorderAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }
        
        public void DeleteServiceOrder(ref OperationResult pobjOperationResult, string v_ServiceOrderId, List<string> ClientSession)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                #region Service Order
                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.serviceorder
                                       where a.v_ServiceOrderId == v_ServiceOrderId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                objEntitySource.i_IsDeleted = 1;

                #endregion

                #region Service Order Detail
                // Obtener la entidad fuente
                var objEntitySourceDetail = (from a in dbContext.serviceorderdetail
                                       where a.v_ServiceOrderId == v_ServiceOrderId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                objEntitySourceDetail.d_UpdateDate = DateTime.Now;
                objEntitySourceDetail.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                objEntitySourceDetail.i_IsDeleted = 1;

                #endregion
               
                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ELIMINACION, "ORDEN DE SERVICIO", "", Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ELIMINACION, "ORDEN DE SERVICIO", "", Success.Failed, pobjOperationResult.ExceptionMessage);
                return;
            }
        }

        public serviceorderdetailDto GetServiceOrderDetail(ref OperationResult pobjOperationResult, string pstrServiceOderderId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                serviceorderdetailDto objDtoEntity = null;

                var objEntity = (from a in dbContext.serviceorderdetail
                                 where a.v_ServiceOrderId == pstrServiceOderderId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = serviceorderdetailAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return null;
            }
        }

        public List<ServiceOrderList> GetReportServiceOrder(string pstServiceOrderId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();

                var query = from A in dbContext.serviceorder
                            join B in dbContext.systemparameter on new { a = A.i_ServiceOrderStatusId.Value, b = 194 } equals new { a = B.i_ParameterId, b = B.i_GroupId }
                            join C in dbContext.serviceorderdetail on A.v_ServiceOrderId equals C.v_ServiceOrderId
                            join D in dbContext.protocol on C.v_ProtocolId equals D.v_ProtocolId
                            join E in dbContext.organization on D.v_CustomerOrganizationId equals E.v_OrganizationId
                            join F in dbContext.systemparameter on new { a = D.i_EsoTypeId.Value, b = 118 } equals new { a = F.i_ParameterId, b = F.i_GroupId }
                            join G in dbContext.protocolcomponent on D.v_ProtocolId equals G.v_ProtocolId
                            join H in dbContext.component on G.v_ComponentId equals H.v_ComponentId
                            join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }

                                                            equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                            from J1 in J1_join.DefaultIfEmpty()

                            join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                            equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                            from J2 in J2_join.DefaultIfEmpty()
                            where A.i_IsDeleted == 0 && A.v_ServiceOrderId == pstServiceOrderId

                            select new ServiceOrderList
                            {
                                v_ServiceOrderId = A.v_ServiceOrderId,
                                v_CustomServiceOrderId = A.v_CustomServiceOrderId,
                                v_Description = A.v_Description,
                                v_Comentary = A.v_Comentary,
                                i_NumberOfWorker = A.i_NumberOfWorker,
                                r_TotalCost = A.r_TotalCost,
                                d_DeliveryDate = A.d_DeliveryDate,

                                i_ServiceOrderStatusId = A.i_ServiceOrderStatusId,
                                v_ServiceOrderStatusName = B.v_Value1,
                                v_CreationUser = J1.v_UserName,
                                v_UpdateUser = J2.v_UserName,
                                d_CreationDate = A.d_InsertDate,
                                d_UpdateDate = A.d_UpdateDate,
                                v_ProtocolId = C.v_ProtocolId,
                                v_Protocol = D.v_Name,
                                v_Organization = E.v_Name,
                                v_ContacName = E.v_ContacName,
                                v_Address = E.v_Address,
                                v_EsoType = F.v_Value1,
                                v_ComponentName = H.v_Name,
                                r_Price = G.r_Price, // H.r_BasePrice.Value,
                                d_InsertDate = A.d_InsertDate.Value,

                            };

                var finalQuery = (from a in query.ToList()
                                  select new ServiceOrderList
                                  {
                                      v_ServiceOrderId = a.v_ServiceOrderId,
                                      v_CustomServiceOrderId = a.v_CustomServiceOrderId,
                                      v_Description = a.v_Description,
                                      v_Comentary = a.v_Comentary,
                                      i_NumberOfWorker = a.i_NumberOfWorker,
                                      r_TotalCost = a.r_TotalCost,
                                      d_DeliveryDate = a.d_DeliveryDate,

                                      i_ServiceOrderStatusId = a.i_ServiceOrderStatusId,
                                      v_ServiceOrderStatusName = a.v_ServiceOrderStatusName,
                                      v_CreationUser = a.v_CreationUser,
                                      v_UpdateUser = a.v_UpdateUser,
                                      d_CreationDate = a.d_CreationDate,
                                      d_UpdateDate = a.d_UpdateDate,
                                      v_ProtocolId = a.v_ProtocolId,
                                      v_Protocol = a.v_Protocol,
                                      v_Organization = a.v_Organization,
                                      v_ContacName = a.v_ContacName,
                                      v_Address = a.v_Address,
                                      v_EsoType = a.v_EsoType,
                                      v_ComponentName = a.v_ComponentName,
                                      r_Price = a.r_Price, // H.r_BasePrice.Value,
                                      d_InsertDate = a.d_InsertDate,

                                      b_Logo = MedicalCenter.b_Image,
                                      EmpresaPropietaria = MedicalCenter.v_Name,
                                      EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                                      EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                                      EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                                  }).ToList();

                return finalQuery;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}