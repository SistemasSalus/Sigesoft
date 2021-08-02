using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.DAL;
using Sigesoft.Common;

namespace Sigesoft.Node.WinClient.BLL
{
    public class PacientBL
    {
        //Devart.Data.PostgreSql.PgSqlMonitor mon = new Devart.Data.PostgreSql.PgSqlMonitor();
        ServiceBL serviceBL = new ServiceBL();

        #region Person

        public personDto GetPerson(ref OperationResult pobjOperationResult, string pstrPersonId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                personDto objDtoEntity = null;

                var objEntity = (from a in dbContext.person
                                 where a.v_PersonId == pstrPersonId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = personAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }      

        public string AddPerson(ref OperationResult pobjOperationResult, personDto pobjPerson, professionalDto pobjProfessional, systemuserDto pobjSystemUser, List<string> ClientSession)
        {
            //mon.IsActive = true;
            int SecuentialId = -1;
            string newId = string.Empty;
            person objEntity1 = null;

            try
            {
                #region Validations
                // Validar el DNI de la persona
                if (pobjPerson != null)
                {
                    OperationResult objOperationResult6 = new OperationResult();
                    string strfilterExpression1 = string.Format("v_DocNumber==\"{0}\"&&i_Isdeleted==0", pobjPerson.v_DocNumber);
                    var _recordCount1 = GetPersonCount(ref objOperationResult6, strfilterExpression1);

                    if (_recordCount1 != 0)
                    {
                        pobjOperationResult.ErrorMessage = "El número de documento " + pobjPerson.v_DocNumber + " ya se encuentra registrado.\nPor favor ingrese otro número de documento.";
                        return "-1";
                    }
                }

                // Validar existencia de UserName en la BD
                if (pobjSystemUser != null)
                {
                    OperationResult objOperationResult7 = new OperationResult();
                    string strfilterExpression2 = string.Format("v_UserName==\"{0}\"&&i_Isdeleted==0", pobjSystemUser.v_UserName);
                    var _recordCount2 = new SecurityBL().GetSystemUserCount(ref objOperationResult7, strfilterExpression2);

                    if (_recordCount2 != 0)
                    {
                        pobjOperationResult.ErrorMessage = "El nombre de usuario  <font color='red'>" + pobjSystemUser.v_UserName + "</font> ya se encuentra registrado.<br> Por favor ingrese otro nombre de usuario.";
                        return "-1";
                    }
                }
                #endregion

                // Grabar Persona
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                objEntity1 = personAssembler.ToEntity(pobjPerson);

                objEntity1.d_InsertDate = DateTime.Now;
                objEntity1.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity1.i_IsDeleted = 0;
                // Autogeneramos el Pk de la tabla
                SecuentialId = Utils.GetNextSecuentialId(Int32.Parse(ClientSession[0]), 8);
                newId = Common.Utils.GetNewId(int.Parse(ClientSession[0]), SecuentialId, "PP");
                objEntity1.v_PersonId = newId;

                dbContext.AddToperson(objEntity1);
                dbContext.SaveChanges();

                // Grabar Profesional
                if (pobjProfessional != null)
                {
                    OperationResult objOperationResult2 = new OperationResult();
                    pobjProfessional.v_PersonId = objEntity1.v_PersonId;
                    AddProfessional(ref objOperationResult2, pobjProfessional, ClientSession);
                }

                // Grabar Usuario
                if (pobjSystemUser != null)
                {
                    OperationResult objOperationResult3 = new OperationResult();
                    pobjSystemUser.v_PersonId = objEntity1.v_PersonId;
                    new SecurityBL().AddSystemUSer(ref objOperationResult3, pobjSystemUser, ClientSession);
                }

                ////Seteamos si el registro es agregado en el DataCenter o en un nodo
                //if (ClientSession[0] == "1")
                //{
                //    objEntity1.i_IsInMaster = 1;
                //}
                //else
                //{
                //    objEntity1.i_IsInMaster = 0;
                //}

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PERSONA", "v_PersonId=" + objEntity1.v_PersonId, Success.Ok, null);
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PERSONA", "v_PersonId=" + objEntity1.v_PersonId, Success.Failed, ex.Message);
            }

            return newId;
        }

        public string UpdatePerson(ref OperationResult pobjOperationResult, bool pbIsChangeDocNumber, personDto pobjPerson, professionalDto pobjProfessional, bool pbIsChangeUserName, systemuserDto pobjSystemUser, List<string> ClientSession)
        {

            try
            {
                #region Validate SystemUSer
                // Validar existencia de UserName en la BD
                if (pobjSystemUser != null && pbIsChangeUserName == true)
                {
                    OperationResult objOperationResult7 = new OperationResult();
                    string strfilterExpression2 = string.Format("v_UserName==\"{0}\"&&i_Isdeleted==0", pobjSystemUser.v_UserName);
                    var _recordCount2 = new SecurityBL().GetSystemUserCount(ref objOperationResult7, strfilterExpression2);

                    if (_recordCount2 != 0)
                    {
                        pobjOperationResult.ErrorMessage = "El nombre de usuario  <font color='red'>" + pobjSystemUser.v_UserName + "</font> ya se encuentra registrado.<br> Por favor ingrese otro nombre de usuario.";
                        return "-1";
                    }
                }

                #endregion

                #region Validate Document Number

                // Validar el DNI de la persona
                if (pobjPerson != null && pbIsChangeDocNumber == true)
                {
                    OperationResult objOperationResult6 = new OperationResult();
                    string strfilterExpression1 = string.Format("v_DocNumber==\"{0}\"&&i_Isdeleted==0", pobjPerson.v_DocNumber);
                    var _recordCount1 = GetPersonCount(ref objOperationResult6, strfilterExpression1);

                    if (_recordCount1 != 0)
                    {
                        pobjOperationResult.ErrorMessage = "El número de documento  <font color='red'>" + pobjPerson.v_DocNumber + "</font> ya se encuentra registrado.<br> Por favor ingrese otro número de documento.";
                        return "-1";
                    }
                }

                #endregion

                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                // Actualiza Persona
                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.person
                                       where a.v_PersonId == pobjPerson.v_PersonId
                                       select a).FirstOrDefault();

                pobjPerson.d_UpdateDate = DateTime.Now;
                pobjPerson.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                person objEntity = personAssembler.ToEntity(pobjPerson);

                // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                dbContext.person.ApplyCurrentValues(objEntity);

                // Guardar los cambios
                dbContext.SaveChanges();

                // Actualiza Profesional
                if (pobjProfessional != null)
                {
                    OperationResult objOperationResult2 = new OperationResult();
                    UpdateProfessional(ref objOperationResult2, pobjProfessional, ClientSession);
                }

                // Actualiza Usuario
                if (pobjSystemUser != null)
                {
                    OperationResult objOperationResult3 = new OperationResult();
                    //new SecurityBL().UpdateSystemUSer(ref objOperationResult3, pobjSystemUser, ClientSession);
                }

                //if (ClientSession[0] == "1")
                //{
                //    objEntitySource.i_IsInMaster = 1;
                //}
                //else
                //{
                //    objEntitySource.i_IsInMaster = 0;
                //}

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "PERSONA", "v_PersonId=" + pobjPerson.v_PersonId, Success.Ok, null);
                return "1";
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "PERSONA", "v_PersonId=" + pobjPerson.v_PersonId, Success.Failed, ex.Message);
                return "-1";
            }
        }

        public void DeletePerson(ref OperationResult pobjOperationResult, string pstrPersonId, List<string> ClientSession)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.person
                                       where a.v_PersonId == pstrPersonId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);
                objEntitySource.i_IsDeleted = 1;

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ELIMINACION, "PERSONA", "v_PersonId=" + objEntitySource.v_PersonId, Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ELIMINACION, "PERSONA", "", Success.Failed, null);
                return;
            }
        }

        public int GetPersonCount(ref OperationResult pobjOperationResult, string filterExpression)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = from a in dbContext.person select a;

                if (!string.IsNullOrEmpty(filterExpression))
                    query = query.Where(filterExpression);

                int intResult = query.Count();

                pobjOperationResult.Success = 1;
                return intResult;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return 0;
            }
        }

        public personDto GetPersonByNroDocument(ref OperationResult pobjOperationResult, string pstNroDocument)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                personDto objDtoEntity = null;

                var objEntity = (from a in dbContext.person
                                 where a.v_DocNumber == pstNroDocument && a.i_IsDeleted == 0
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = personAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public personDto GetPersonImage(string personId)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from a in dbContext.person
                                 where a.v_PersonId == personId
                                 select new personDto
                                 {
                                     b_PersonImage = a.b_PersonImage
                                 }).FirstOrDefault();

                return objEntity;
            }
            catch (Exception)
            {
                return null;
            }
        }      
        


        //public void AddPersonOrganization(ref OperationResult pobjOperationResult, int PersonId, int OrganizationId, List<string> ClientSession)
        //{
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        personorganization objEntity = new PersonOrganization();

        //        objEntity.v_PersonId = PersonId;
        //        objEntity.i_OrganizationId = OrganizationId;
        //        objEntity.d_InsertDate = DateTime.Now;
        //        objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);

        //        dbContext.AddToPersonOrganizations(objEntity);
        //        dbContext.SaveChanges();

        //        pobjOperationResult.Success = 1;
        //        // Llenar entidad Log
        //        //new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.CREACION, "ORGANIZACIÓN", "i_OrganizationId=" + objEntity.i_OrganizationId.ToString(), Constants.Success.Ok, null);
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = ex.Message;
        //        // Llenar entidad Log
        //        //new Utils().SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], Constants.LogEventType.CREACION, "PERSONA", "v_PersonId=" + objEntity1.v_PersonId, Constants.Success.Failed, ex.Message);
        //    }
        //}

        #endregion

        #region Professional

        public professionalDto GetProfessional(ref OperationResult pobjOperationResult, string pstrPersonId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                professionalDto objDtoEntity = null;

                var objEntity = (from a in dbContext.professional
                                 where a.v_PersonId == pstrPersonId
                                 select a).FirstOrDefault();

                if (objEntity != null)
                    objDtoEntity = professionalAssembler.ToDTO(objEntity);

                pobjOperationResult.Success = 1;
                return objDtoEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public void AddProfessional(ref OperationResult pobjOperationResult, professionalDto pobjDtoEntity, List<string> ClientSession)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                professional objEntity = professionalAssembler.ToEntity(pobjDtoEntity);

                objEntity.d_InsertDate = DateTime.Now;
                objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
                objEntity.i_IsDeleted = 0;

                dbContext.AddToprofessional(objEntity);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PROFESIONAL", "i_ProfessionId=" + objEntity.i_ProfessionId, Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PROFESIONAL", "i_ProfessionId=" + pobjDtoEntity.i_ProfessionId, Success.Failed, ex.Message);
                return;
            }
        }

        public void UpdateProfessional(ref OperationResult pobjOperationResult, professionalDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                // Obtener la entidad fuente
                var objEntitySource = (from a in dbContext.professional
                                       where a.v_PersonId == pobjDtoEntity.v_PersonId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados

                pobjDtoEntity.d_UpdateDate = DateTime.Now;
                pobjDtoEntity.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                professional objProfessionalTyped = professionalAssembler.ToEntity(pobjDtoEntity);

                // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                dbContext.professional.ApplyCurrentValues(objProfessionalTyped);

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "PROFESIONAL", "i_ProfessionId=" + pobjDtoEntity.i_ProfessionId, Success.Ok, null);
                return;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "PROFESIONAL", "i_ProfessionId=" + pobjDtoEntity.i_ProfessionId, Success.Failed, ex.Message);
                return;
            }
        }

        #endregion

        #region Pacient

        public string AddPacient(ref OperationResult pobjOperationResult, personDto pobjDtoEntity, List<string> ClientSession)
        {
            //mon.IsActive = true;
            string NewId = "(No generado)";
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                pacient objEntityPacient = pacientAssembler.ToEntity(new pacientDto());

                objEntityPacient.v_PersonId = AddPerson(ref pobjOperationResult, pobjDtoEntity, null, null, ClientSession);

                if (objEntityPacient.v_PersonId == "-1")
                {
                    pobjOperationResult.Success = 0;
                    return "-1";
                }
                pobjDtoEntity = GetPerson(ref pobjOperationResult, objEntityPacient.v_PersonId);

                objEntityPacient.i_IsDeleted = pobjDtoEntity.i_IsDeleted;
                objEntityPacient.d_InsertDate = DateTime.Now;
                objEntityPacient.i_InsertUserId = Int32.Parse(ClientSession[2]);

                NewId = objEntityPacient.v_PersonId;
                
                dbContext.AddTopacient(objEntityPacient);
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PACIENTE", "v_PersonId=" + NewId.ToString(), Success.Ok, null);

                return NewId;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PACIENTE", "v_PersonId=" + NewId, Success.Failed, pobjOperationResult.ExceptionMessage);
                return null;
            }
        }

        public string UpdatePacient(ref OperationResult pobjOperationResult, personDto pobjDtoEntity, List<string> ClientSession ,string NumbreDocument, string _NumberDocument)
        {
            //mon.IsActive = true;
            string resultado;
            try
            {
                //Actualizamos la tabla Person
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                pobjDtoEntity.i_IsDeleted = 0;
                pobjDtoEntity.d_UpdateDate = DateTime.Now;
                pobjDtoEntity.i_UpdateUserId = Int32.Parse(ClientSession[2]);


                if (NumbreDocument == _NumberDocument)
                {
                     resultado = UpdatePerson(ref pobjOperationResult, false, pobjDtoEntity, null, false, null, ClientSession);
                }
                else
                {
                     resultado = UpdatePerson(ref pobjOperationResult, true, pobjDtoEntity, null, false, null, ClientSession);
                }

           

               if (resultado== "-1")
               {
                   pobjOperationResult.Success = 0;
                   return resultado;
               }
                // Obtener la entidad fuente de la tabla Pacient
                var objEntitySource = (from a in dbContext.pacient
                                       where a.v_PersonId == pobjDtoEntity.v_PersonId
                                       select a).FirstOrDefault();

                // Crear la entidad con los datos actualizados
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = Int32.Parse(ClientSession[2]);

                // Copiar los valores desde la entidad actualizada a la Entidad Fuente
                dbContext.pacient.ApplyCurrentValues(objEntitySource);

                // Guardar los cambios
                dbContext.SaveChanges();

                pobjOperationResult.Success = 1;
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ACTUALIZACION, "PACIENTE", "v_PacientId=" + pobjDtoEntity.v_PersonId, Success.Ok, null);
                return "1";

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.CREACION, "PACIENTE", "v_PacientId=" + pobjDtoEntity.v_PersonId, Success.Failed, ex.Message);
                return "-1";
            }
        }

        public void DeletePacient(ref OperationResult pobjOperationResult, string pstrPersonId, List<string> ClientSession)
        {
            //mon.IsActive = true;
            try
            {
                DeletePerson(ref pobjOperationResult, pstrPersonId, ClientSession);

            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                // Llenar entidad Log
                LogBL.SaveLog(ClientSession[0], ClientSession[1], ClientSession[2], LogEventType.ELIMINACION, "PACIENTE", "", Success.Failed, null);
            }

        }

        public int GetPacientsCount(ref OperationResult pobjOperationResult, string pstrFirstLastNameorDocNumber)
        {
            //mon.IsActive = true;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                Int32 intId = -1;
                bool FindById = int.TryParse(pstrFirstLastNameorDocNumber, out intId);
                var Id = intId.ToString();
                var query = (from A in dbContext.pacient
                             join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                             where (B.v_FirstName.Contains(pstrFirstLastNameorDocNumber) || B.v_FirstLastName.Contains(pstrFirstLastNameorDocNumber)
                                    || B.v_SecondLastName.Contains(pstrFirstLastNameorDocNumber)) && B.i_IsDeleted == 0
                             select A).Concat
                             (from A in dbContext.pacient
                              join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                              where B.v_DocNumber.Equals(Id)
                              select A);

                pobjOperationResult.Success = 1;
                return query.Count();
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
                return 0;
            }
        }

        public PacientList GetPacient(ref OperationResult pobjOperationResult, string pstrPacientId,string pstNroDocument)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                //PacientList objDtoEntity = null;

                var objEntity = (from A in dbContext.pacient
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                                       equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()

                                 join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                                 equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                 from J2 in J2_join.DefaultIfEmpty()
                                 where A.v_PersonId == pstrPacientId || B.v_DocNumber == pstNroDocument
                                 select new PacientList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_FirstName = B.v_FirstName,
                                     v_FirstLastName = B.v_FirstLastName,
                                     v_SecondLastName = B.v_SecondLastName,
                                     v_DocNumber = B.v_DocNumber,
                                     v_BirthPlace = B.v_BirthPlace,
                                     i_MaritalStatusId = B.i_MaritalStatusId,
                                     i_LevelOfId = B.i_LevelOfId,
                                     i_DocTypeId = B.i_DocTypeId,
                                     i_SexTypeId = B.i_SexTypeId,
                                     v_TelephoneNumber = B.v_TelephoneNumber,
                                     v_AdressLocation = B.v_AdressLocation,
                                     v_Mail = B.v_Mail,
                                     b_Photo = B.b_PersonImage,
                                     d_Birthdate = B.d_Birthdate,
                                     i_BloodFactorId = B.i_BloodFactorId,
                                     i_BloodGroupId = B.i_BloodGroupId.Value,
                                     b_FingerPrintTemplate = B.b_FingerPrintTemplate,
                                     b_FingerPrintImage = B.b_FingerPrintImage,
                                     b_RubricImage = B.b_RubricImage,
                                     t_RubricImageText = B.t_RubricImageText,

                                     v_CurrentOccupation = B.v_CurrentOccupation,
                                     i_DepartmentId = B.i_DepartmentId,
                                     i_ProvinceId = B.i_ProvinceId,
                                     i_DistrictId = B.i_DistrictId,
                                     i_ResidenceInWorkplaceId = B.i_ResidenceInWorkplaceId,
                                     v_ResidenceTimeInWorkplace = B.v_ResidenceTimeInWorkplace,
                                     i_TypeOfInsuranceId = B.i_TypeOfInsuranceId,
                                     i_NumberLivingChildren = B.i_NumberLivingChildren,
                                     i_NumberDependentChildren = B.i_NumberDependentChildren,
                                     i_Relationship = B.i_Relationship,
                                     v_ExploitedMineral = B.v_ExploitedMineral,
                                     i_AltitudeWorkId = B.i_AltitudeWorkId,
                                     i_PlaceWorkId = B.i_PlaceWorkId,
                                    v_OwnerName = B.v_OwnerName
                                 }).FirstOrDefault();

                pobjOperationResult.Success = 1;
                return objEntity;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public List<PacientList> GetPacientsPagedAndFiltered(ref OperationResult pobjOperationResult, int pintPageIndex, int pintResultsPerPage, string pstrFirstLastNameorDocNumber)
        {
            //mon.IsActive = true;
            try
            {
                int intId = -1;
                bool FindById = int.TryParse(pstrFirstLastNameorDocNumber, out intId);
                var Id = intId.ToString();
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var query = (from A in dbContext.pacient
                             join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                             join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                                   equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                             from J1 in J1_join.DefaultIfEmpty()

                             join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                             equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                             from J2 in J2_join.DefaultIfEmpty()
                             where (B.v_FirstName.Contains(pstrFirstLastNameorDocNumber) || B.v_FirstLastName.Contains(pstrFirstLastNameorDocNumber)
                                    || B.v_SecondLastName.Contains(pstrFirstLastNameorDocNumber) || B.v_PersonId.Contains(pstrFirstLastNameorDocNumber)) && B.i_IsDeleted == 0
                             select new PacientList
                             {
                                 v_PersonId = A.v_PersonId,
                                 v_FirstName = B.v_FirstName,
                                 v_FirstLastName = B.v_FirstLastName,
                                 v_SecondLastName = B.v_SecondLastName,
                                 v_AdressLocation = B.v_AdressLocation,
                                 v_TelephoneNumber = B.v_TelephoneNumber,
                                 v_Mail = B.v_Mail,
                                 v_CreationUser = J1.v_UserName,
                                 v_UpdateUser = J2.v_UserName,
                                 d_CreationDate = A.d_InsertDate,
                                 d_UpdateDate = A.d_UpdateDate,
                                 i_DepartmentId = B.i_DepartmentId,
                                 i_ProvinceId = B.i_ProvinceId,
                                 i_DistrictId = B.i_DistrictId,
                                 i_ResidenceInWorkplaceId = B.i_ResidenceInWorkplaceId,
                                 v_ResidenceTimeInWorkplace = B.v_ResidenceTimeInWorkplace,
                                 i_TypeOfInsuranceId = B.i_TypeOfInsuranceId,
                                 i_NumberLivingChildren = B.i_NumberLivingChildren,
                                 i_NumberDependentChildren = B.i_NumberDependentChildren
                
                             }).Concat
                            (from A in dbContext.pacient
                             join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                             join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                                   equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                             from J1 in J1_join.DefaultIfEmpty()

                             join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                             equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                             from J2 in J2_join.DefaultIfEmpty()
                             where B.v_DocNumber == Id && B.i_IsDeleted == 0
                             select new PacientList
                             {
                                 v_PersonId = A.v_PersonId,
                                 v_FirstName = B.v_FirstName,
                                 v_FirstLastName = B.v_FirstLastName,
                                 v_SecondLastName = B.v_SecondLastName,
                                 v_AdressLocation = B.v_AdressLocation,
                                 v_TelephoneNumber = B.v_TelephoneNumber,
                                 v_Mail = B.v_Mail,
                                 v_CreationUser = J1.v_UserName,
                                 v_UpdateUser = J2.v_UserName,
                                 d_CreationDate = A.d_InsertDate,
                                 d_UpdateDate = A.d_UpdateDate,
                                 i_DepartmentId = B.i_DepartmentId,
                                 i_ProvinceId = B.i_ProvinceId,
                                 i_DistrictId = B.i_DistrictId,
                                 i_ResidenceInWorkplaceId = B.i_ResidenceInWorkplaceId,
                                 v_ResidenceTimeInWorkplace = B.v_ResidenceTimeInWorkplace,
                                 i_TypeOfInsuranceId = B.i_TypeOfInsuranceId,
                                 i_NumberLivingChildren = B.i_NumberLivingChildren,
                                 i_NumberDependentChildren = B.i_NumberDependentChildren
                             }).OrderBy("v_FirstLastName").Take(pintResultsPerPage);

                List<PacientList> objData = query.ToList();
                pobjOperationResult.Success = 1;
                return objData;
            }
            catch (Exception ex)
            {
                pobjOperationResult.Success = 0;
                pobjOperationResult.ExceptionMessage = ex.Message;
                return null;
            }
        }

        public PacientList GetPacientReport(string pstrPacientId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                //PacientList objDtoEntity = null;

                var objEntity = (from A in dbContext.pacient
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.systemparameter on new { a = B.i_MaritalStatusId.Value, b = 101 }
                                                                        equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                                 from C in C_join.DefaultIfEmpty()
                                 join D in dbContext.datahierarchy on new { a = B.i_DocTypeId.Value, b = 106 }
                                                                        equals new { a = D.i_ItemId, b = D.i_GroupId } into D_join
                                 from D in D_join.DefaultIfEmpty()

                                 join E in dbContext.datahierarchy on new { a = B.i_DepartmentId.Value, b = 113 }
                                                                    equals new { a = E.i_ItemId, b = E.i_GroupId } into E_join
                                 from E in E_join.DefaultIfEmpty()


                                 join J1 in dbContext.systemuser on new { i_InsertUserId = A.i_InsertUserId.Value }
                                                                       equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()

                                 join J2 in dbContext.systemuser on new { i_UpdateUserId = A.i_UpdateUserId.Value }
                                                                 equals new { i_UpdateUserId = J2.i_SystemUserId } into J2_join
                                 from J2 in J2_join.DefaultIfEmpty()
                                 where A.v_PersonId == pstrPacientId
                                 select new PacientList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_FirstName = B.v_FirstName,
                                     v_FirstLastName = B.v_FirstLastName,
                                     v_SecondLastName = B.v_SecondLastName,
                                     v_DocNumber = B.v_DocNumber,
                                     v_BirthPlace = B.v_BirthPlace,
                                     i_MaritalStatusId = B.i_MaritalStatusId,
                                     v_MaritalStatus = C.v_Value1,
                                     i_LevelOfId = B.i_LevelOfId,
                                     i_DocTypeId = B.i_DocTypeId,
                                     v_DocTypeName = D.v_Value1,
                                     i_SexTypeId = B.i_SexTypeId,
                                     v_TelephoneNumber = B.v_TelephoneNumber,
                                     v_AdressLocation = B.v_AdressLocation,
                                     v_Mail = B.v_Mail,
                                     b_Photo = B.b_PersonImage,
                                     d_Birthdate = B.d_Birthdate,
                                     i_BloodFactorId = B.i_BloodFactorId.Value,
                                     i_BloodGroupId = B.i_BloodGroupId.Value,
                                     b_FingerPrintTemplate = B.b_FingerPrintTemplate,
                                     b_FingerPrintImage = B.b_FingerPrintImage,
                                     b_RubricImage = B.b_RubricImage,
                                     t_RubricImageText = B.t_RubricImageText,
                                     v_CurrentOccupation = B.v_CurrentOccupation,
                                     i_DepartmentId = B.i_DepartmentId,
                                     v_DepartamentName = E.v_Value1,
                                     i_ProvinceId = B.i_ProvinceId,
                                     v_ProvinceName = E.v_Value1,
                                     i_DistrictId = B.i_DistrictId,
                                     v_DistrictName = E.v_Value1,
                                     i_ResidenceInWorkplaceId = B.i_ResidenceInWorkplaceId,
                                     v_ResidenceTimeInWorkplace = B.v_ResidenceTimeInWorkplace,
                                     i_TypeOfInsuranceId = B.i_TypeOfInsuranceId,
                                     i_NumberLivingChildren = B.i_NumberLivingChildren,
                                     i_NumberDependentChildren = B.i_NumberDependentChildren

                                 }).FirstOrDefault();

                return objEntity;
            }
            catch (Exception ex)
            {
             
                return null;
            }
        }

        public PacientList GetPacientReportEPS(string serviceId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                //PacientList objDtoEntity = null;

                var objEntity = (from s in dbContext.service
                                 join pr in dbContext.protocol on s.v_ProtocolId equals pr.v_ProtocolId                             
                                 join pe in dbContext.person on s.v_PersonId equals pe.v_PersonId

                                 join C in dbContext.systemparameter on new { a = pe.i_TypeOfInsuranceId.Value, b = 188 }  // Tipo de seguro
                                                              equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                                 from C in C_join.DefaultIfEmpty()

                                 join d in dbContext.systemparameter on new { a = pe.i_Relationship.Value, b = 207 }  // Parentesco
                                                              equals new { a = d.i_ParameterId, b = d.i_GroupId } into d_join
                                 from d in d_join.DefaultIfEmpty()

                                 // Grupo sanguineo ****************************************************
                                 join gs in dbContext.systemparameter on new { a = pe.i_BloodGroupId.Value, b = 154 }  // AB
                                                             equals new { a = gs.i_ParameterId, b = gs.i_GroupId } into gs_join
                                 from gs in gs_join.DefaultIfEmpty()

                                 // Factor sanguineo ****************************************************
                                 join fs in dbContext.systemparameter on new { a = pe.i_BloodFactorId.Value, b = 155 }  // NEGATIVO
                                                           equals new { a = fs.i_ParameterId, b = fs.i_GroupId } into fs_join
                                 from fs in fs_join.DefaultIfEmpty()

                                 // Empresa / Sede Trabajo  ********************************************************
                                 join ow in dbContext.organization on new { a = s.v_EmpresaFacturacionId }
                                         equals new { a = ow.v_OrganizationId } into ow_join
                                 from ow in ow_join.DefaultIfEmpty()

                                 join lw in dbContext.location on new { a = pr.v_WorkingOrganizationId, b = pr.v_WorkingLocationId }
                                      equals new { a = lw.v_OrganizationId, b = lw.v_LocationId } into lw_join
                                 from lw in lw_join.DefaultIfEmpty()

                                 //************************************************************************************
                           
                                 where s.v_ServiceId == serviceId
                                 select new PacientList
                                 {
                                     v_PersonId = pe.v_PersonId,
                                     v_FirstName = pe.v_FirstName,
                                     v_FirstLastName = pe.v_FirstLastName,
                                     v_SecondLastName = pe.v_SecondLastName,                                                                                                      
                                     b_Photo = pe.b_PersonImage,                                  
                                     v_TypeOfInsuranceName = C.v_Value1,
                                     v_CurrentOccupation = pe.v_CurrentOccupation,
                                     v_FullWorkingOrganizationName = ow.v_Name ,
                                     v_RelationshipName = d.v_Value1,
                                     v_OwnerName = "",
                                     d_ServiceDate = s.d_ServiceDate,
                                     d_Birthdate = pe.d_Birthdate,
                                     i_DocTypeId = pe.i_DocTypeId,
                                     i_NumberDependentChildren = pe.i_NumberDependentChildren,
                                     i_NumberLivingChildren = pe.i_NumberLivingChildren,
                                     FirmaTrabajador = pe.b_RubricImage,
                                     HuellaTrabajador = pe.b_FingerPrintImage,
                                     v_BloodGroupName = gs.v_Value1,
                                     v_BloodFactorName = fs.v_Value1
                                 });

                // Medico Examen fisico

                //var medico = (from sc in dbContext.servicecomponent
                //              join J1 in dbContext.systemuser on new { i_InsertUserId = sc.i_InsertUserId.Value }
                //                          equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                //              from J1 in J1_join.DefaultIfEmpty()
                //              join pe in dbContext.person on J1.v_PersonId equals pe.v_PersonId
                //              where (sc.v_ServiceId == serviceId) &&
                //                      (sc.v_ComponentId == Constants.EXAMEN_FISICO_ID)
                //              select pe.v_FirstName + " " + pe.v_FirstLastName).SingleOrDefault<string>();

                var sql = (from a in objEntity.ToList()
                           select new PacientList
                            {
                                v_PersonId = a.v_PersonId,
                                  i_DocTypeId = a.i_DocTypeId,
                                v_FirstName = a.v_FirstName,
                                v_FirstLastName = a.v_FirstLastName,
                                v_SecondLastName = a.v_SecondLastName,
                                i_Age = GetAge(a.d_Birthdate.Value),
                                b_Photo = a.b_Photo,
                                v_TypeOfInsuranceName = a.v_TypeOfInsuranceName,
                                v_CurrentOccupation = a.v_CurrentOccupation,
                                v_FullWorkingOrganizationName = a.v_FullWorkingOrganizationName,
                                v_RelationshipName = a.v_RelationshipName,
                                v_OwnerName = a.v_FirstName + " " + a.v_FirstLastName + " " + a.v_SecondLastName,
                                d_ServiceDate = a.d_ServiceDate,
                                i_NumberDependentChildren = a.i_NumberDependentChildren,
                                i_NumberLivingChildren = a.i_NumberLivingChildren,
                                v_OwnerOrganizationName = (from n in dbContext.organization
                                                           where n.v_OrganizationId == Constants.OWNER_ORGNIZATION_ID
                                                           select n.v_Name).SingleOrDefault<string>(),
                                v_DoctorPhysicalExamName = (from sc in dbContext.servicecomponent
                                                            join J1 in dbContext.systemuser on new { i_InsertUserId = sc.i_ApprovedUpdateUserId.Value }
                                                                       equals new { i_InsertUserId = J1.i_SystemUserId } into J1_join
                                                            from J1 in J1_join.DefaultIfEmpty()
                                                            join pe in dbContext.person on J1.v_PersonId equals pe.v_PersonId
                                                            where (sc.v_ServiceId == serviceId) &&
                                                                  (sc.v_ComponentId == Constants.EXAMEN_FISICO_ID)
                                                            select pe.v_FirstName + " " + pe.v_FirstLastName).SingleOrDefault<string>(),
                                FirmaTrabajador = a.FirmaTrabajador,
                                HuellaTrabajador = a.HuellaTrabajador,
                                v_BloodGroupName = a.v_BloodGroupName,
                                v_BloodFactorName = a.v_BloodFactorName

                            }).FirstOrDefault();

                return sql;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public int GetAge(DateTime FechaNacimiento)
        {
            return int.Parse((DateTime.Today.AddTicks(-FechaNacimiento.Ticks).Year - 1).ToString());

        }

        // Alberto
        public List<ServiceList> GetFichaPsicologicaOcupacional(string pstrserviceId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();          

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = "N002-ME000000033" } 
                                                                        equals new { a = E.v_ServiceId , b = E.v_ComponentId }                    

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()                           

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 //*********************************************************************

                                 join H in dbContext.systemparameter on new { a = C.i_EsoTypeId.Value, b = 118 }
                                                 equals new { a = H.i_ParameterId, b = H.i_GroupId }  // TIPO ESO [ESOA,ESOR,ETC]
                            
                                 where A.v_ServiceId == pstrserviceId
                                 select new ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName ,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_BirthPlace = B.v_BirthPlace,
                                     i_DiaN = B.d_Birthdate.Value.Day,
                                     i_MesN = B.d_Birthdate.Value.Month,
                                     i_AnioN = B.d_Birthdate.Value.Year,
                                     i_DiaV = A.d_ServiceDate.Value.Day,
                                     i_MesV = A.d_ServiceDate.Value.Month,
                                     i_AnioV = A.d_ServiceDate.Value.Year,
                                     NivelInstruccion = J1.v_Value1,
                                     LugarResidencia = B.v_AdressLocation,
                                     PuestoTrabajo = B.v_CurrentOccupation,
                                     EmpresaTrabajo = D.v_Name,
                                     v_ServiceComponentId = E.v_ServiceComponentId,
                                     v_ServiceId = A.v_ServiceId,
                                     Rubrica = pme.b_SignatureImage,
                                     RubricaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage,
                                     v_EsoTypeName = H.v_Value1,
                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter(); 

                var sql = (from a in objEntity.ToList()
                           select new ServiceList
                            {
                                v_ServiceId = a.v_ServiceId,
                                v_ServiceComponentId = a.v_ServiceComponentId,                               
                                v_PersonId = a.v_PersonId,
                                v_Pacient = a.v_Pacient,
                                i_Edad = GetAge(a.d_BirthDate.Value),
                                v_BirthPlace = a.v_BirthPlace,
                                i_DiaN = a.i_DiaN,
                                i_MesN =a.i_MesN,
                                i_AnioN = a.i_AnioN,
                                i_DiaV = a.i_DiaV,
                                i_MesV = a.i_MesV,
                                i_AnioV = a.i_AnioV,
                                NivelInstruccion = a.NivelInstruccion,
                                LugarResidencia = a.LugarResidencia,
                                PuestoTrabajo = a.PuestoTrabajo,
                                EmpresaTrabajo = a.EmpresaTrabajo,
                                MotivoEvaluacion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000076","NOCOMBO",0,"NO"),
                                NivelIntelectual = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000630", "NOCOMBO", 0,"NO"),
                                CoordinacionVisomotriz = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000631", "NOCOMBO", 0,"NO"),
                                NivelMemoria = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000632", "NOCOMBO", 0,"NO"),
                                Personalidad = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000633", "NOCOMBO", 0,"NO"),
                                Afectividad = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000634", "NOCOMBO", 0,"NO"),
                                AreaCognitiva = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000336", "SICOMBO", 195,"NO"),
                                AreaEmocional = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000337", "SICOMBO", 195,"NO"),
                                Conclusiones = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000338", "NOCOMBO", 0,"NO"),
                                Restriccion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000081", "NOCOMBO", 0,"NO"),
                                Recomendacion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000080", "SICOMBO", 190, "NO"),
                               
                                Presentacion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000283", "SICOMBO", 175,"SI"),
                                
                                Postura = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000282", "SICOMBO", 173, "SI"),

                                DiscursoRitmo = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000280", "SICOMBO", 214, "SI"),
                                DiscursoTono = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000281", "SICOMBO", 215, "SI"),
                                DiscursoArticulacion = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N002-MF000000279", "SICOMBO", 216, "SI"),

                                OrientacionTiempo = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000077", "SICOMBO", 189, "SI"),
                                OrientacionEspacio = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000078", "SICOMBO", 189, "SI"),
                                OrientacionPersona = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000000079", "SICOMBO", 189, "SI"),
                                Rubrica = a.Rubrica,

                                b_Logo = MedicalCenter.b_Image,
                                EmpresaPropietaria = MedicalCenter.v_Name,
                                EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                                EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                                EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                                AreaPersonal = GetServiceComponentFielValue(a.v_ServiceId, "N002-ME000000033", "N009-MF000001298", "NOCOMBO", 0, "NO"),
                                v_EsoTypeName = a.v_EsoTypeName,
                                RubricaTrabajador = a.RubricaTrabajador,
                                HuellaTrabajador = a.HuellaTrabajador,
                            }).ToList();

                return sql;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        // Alberto
        public List<ServiceList> GetMusculoEsqueletico(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()

                                 join F in dbContext.systemuser on E.i_InsertUserId equals F.i_SystemUserId
                                 join G in dbContext.professional on F.v_PersonId equals G.v_PersonId

                                 where A.v_ServiceId == pstrserviceId
                                 select new ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_Pacient = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     d_ServiceDate = A.d_ServiceDate,                                    
                                     EmpresaTrabajo = D.v_Name,                                   
                                     v_ServiceId = A.v_ServiceId,
                                     v_ComponentId = E.v_ServiceComponentId

                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter(); 

                var sql = (from a in objEntity.ToList()

                           let OsteoMuscular = new ServiceBL().ValoresComponente(pstrserviceId, Constants.OSTEO_MUSCULAR_ID)
                           select new ServiceList
                           {
                               v_PersonId = a.v_PersonId,
                               v_Pacient = a.v_Pacient,
                               d_ServiceDate = a.d_ServiceDate,
                               EmpresaTrabajo = a.EmpresaTrabajo,
                               v_ServiceId = a.v_ServiceId,
                               v_ComponentId = a.v_ComponentId,

                               AbdomenExcelente = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_EXCELENTE_ID).v_Value1,
                               AbdomenPromedio = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_PROMEDIO_ID).v_Value1,
                               AbdomenRegular = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_REGULAR_ID).v_Value1,
                               AbdomenPobre = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_POBRE_ID).v_Value1,
                               AbdomenPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_PUNTOS_ID).v_Value1,
                               AbdomenObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_OBSERVACIONES_ID).v_Value1,
                               CaderaExcelente = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_EXCELENTE_ID).v_Value1,
                               CaderaPromedio = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_PROMEDIO_ID).v_Value1,
                               CaderaRegular = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_REGULAR_ID).v_Value1,
                               CaderaPobre = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_POBRE_ID).v_Value1,
                               CaderaPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_PUNTOS_ID).v_Value1,
                               CaderaObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_CADERA_OBSERVACIONES_ID).v_Value1,                               
                               MusloExcelente = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_EXCELENTE_ID).v_Value1,                               
                               MusloPromedio =  OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_PROMEDIO_ID).v_Value1,                               
                               MusloRegular =  OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_REGULAR_ID).v_Value1,                               
                               MusloPobre = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_POBRE_ID).v_Value1,                               
                               MusloPuntos =  OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_PUNTOS_ID).v_Value1,
                               MusloObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_MUSLO_OBSERVACIONES_ID).v_Value1,                               
                               AbdomenLateralExcelente = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_EXCELENTE_ID).v_Value1,                               
                               AbdomenLateralPromedio = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_PROMEDIO_ID).v_Value1,                               
                               AbdomenLateralRegular = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_REGULAR_ID).v_Value1,                               
                               AbdomenLateralPobre = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_POBRE_ID).v_Value1,                               
                               AbdomenLateralPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_PUNTOS_ID).v_Value1,                               
                               AbdomenLateralObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_OBSERVACIONES_ID).v_Value1,                               
                               AbduccionHombroNormalOptimo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_OPTIMO_ID).v_Value1,                               
                               AbduccionHombroNormalLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_LIMITADO_ID).v_Value1,                               
                               AbduccionHombroNormalMuyLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_MUY_LIMITADO_ID).v_Value1,                               
                               AbduccionHombroNormalPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_PUNTOS_ID).v_Value1,
                               AbduccionHombroNormalObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_DOLOR_ID).v_Value1Name,                               
                               AbduccionHombroOptimo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_OPTIMO_ID).v_Value1,                               
                               AbduccionHombroLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_LIMITADO_ID).v_Value1,                               
                               AbduccionHombroMuyLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_MUY_LIMITADO_ID).v_Value1,                               
                               AbduccionHombroPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_PUNTOS_ID).v_Value1,                               
                               AbduccionHombroObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_DOLOR_ID).v_Value1Name,                               
                               RotacionExternaOptimo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_OPTIMO_ID).v_Value1,                               
                               RotacionExternaLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_LIMITADO_ID).v_Value1,                               
                               RotacionExternaMuyLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_MUY_LIMITADO_ID).v_Value1,                               
                               RotacionExternaPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_PUNTOS_ID).v_Value1,                               
                               RotacionExternaObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_DOLOR_ID).v_Value1Name,                               
                               RotacionExternaHombroInternoOptimo = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_OPTIMO_ID).v_Value1,                               
                               RotacionExternaHombroInternoLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_LIMITADO_ID).v_Value1,
                               RotacionExternaHombroInternoMuyLimitado = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_MUY_LIMITADO_ID).v_Value1,                               
                               RotacionExternaHombroInternoPuntos = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_PUNTOS_ID).v_Value1,                               
                               RotacionExternaHombroInternoObs = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_DOLOR_ID).v_Value1Name,                               
                               Total1 = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOTAL1_ID).v_Value1,                               
                               Total2 = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_TOTAL2_ID).v_Value1,                               
                               AptitudMusculoEsqueletico = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_APTITUD_ID).v_Value1,
                               Conclusiones = OsteoMuscular.Count == 0 ? string.Empty : OsteoMuscular.Find(p => p.v_ComponentFieldId == Constants.OSTEO_MUSCULAR_DESCRIPCION_ID).v_Value1,


                               //AbdomenExcelente = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_EXCELENTE_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenPromedio = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_PROMEDIO_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenRegular = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_REGULAR_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenPobre = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_POBRE_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenPuntos = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_PUNTOS_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenObs = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_OBSERVACIONES_ID, "NOCOMBO", 0, "SI"),
                               //CaderaExcelente = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_CADERA_EXCELENTE_ID, "NOCOMBO", 0, "SI"),
                               //CaderaPromedio = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_CADERA_PROMEDIO_ID, "NOCOMBO", 0, "SI"),
                               //CaderaRegular = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_CADERA_REGULAR_ID, "NOCOMBO", 0, "SI"),
                               //CaderaPobre = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_CADERA_POBRE_ID, "NOCOMBO", 0, "SI"),
                               //CaderaPuntos = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_CADERA_PUNTOS_ID, "NOCOMBO", 0, "SI"),
                               //CaderaObs = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_CADERA_OBSERVACIONES_ID, "NOCOMBO", 0, "SI"),
                               //MusloExcelente = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_MUSLO_EXCELENTE_ID, "NOCOMBO", 0, "SI"),
                               //MusloPromedio = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_MUSLO_PROMEDIO_ID, "NOCOMBO", 0, "SI"),
                               //MusloRegular = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_MUSLO_REGULAR_ID, "NOCOMBO", 0, "SI"),
                               //MusloPobre = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_MUSLO_POBRE_ID, "NOCOMBO", 0, "SI"),
                               //MusloPuntos = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_MUSLO_PUNTOS_ID, "NOCOMBO", 0, "SI"),
                               //MusloObs = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_MUSLO_OBSERVACIONES_ID, "NOCOMBO", 0, "SI"),                               
                               //AbdomenLateralExcelente = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_EXCELENTE_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenLateralPromedio = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_PROMEDIO_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenLateralRegular = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_REGULAR_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenLateralPobre = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_POBRE_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenLateralPuntos = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_PUNTOS_ID, "NOCOMBO", 0, "SI"),
                               //AbdomenLateralObs = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ABDOMEN_LATERAL_OBSERVACIONES_ID, "NOCOMBO", 0, "SI"),
                               //AbduccionHombroNormalOptimo = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_OPTIMO_ID, "NOCOMBO", 0, "SI"),
                               //AbduccionHombroNormalLimitado = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_LIMITADO_ID, "NOCOMBO", 0, "SI"),
                               //AbduccionHombroNormalMuyLimitado = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_MUY_LIMITADO_ID, "NOCOMBO", 0, "SI"),
                               //AbduccionHombroNormalPuntos = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_PUNTOS_ID, "NOCOMBO", 0, "SI"),
                               //AbduccionHombroNormalObs = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_NORMAL_DOLOR_ID, "SICOMBO", 111, "NO"),
                               //AbduccionHombroOptimo = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_OPTIMO_ID, "NOCOMBO", 0, "SI"),
                               //AbduccionHombroLimitado = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_LIMITADO_ID, "NOCOMBO", 0, "SI"),
                               //AbduccionHombroMuyLimitado = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_MUY_LIMITADO_ID, "NOCOMBO", 0, "SI"),
                               //AbduccionHombroPuntos = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_PUNTOS_ID, "NOCOMBO", 0, "SI"),
                               //AbduccionHombroObs = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ADUCCION_HOMBRO_DOLOR_ID, "SICOMBO", 111, "NO"),
                               //RotacionExternaOptimo = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_OPTIMO_ID, "NOCOMBO", 0, "SI"),
                               //RotacionExternaLimitado = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_LIMITADO_ID, "NOCOMBO", 0, "SI"),
                               //RotacionExternaMuyLimitado = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_MUY_LIMITADO_ID, "NOCOMBO", 0, "SI"),
                               //RotacionExternaPuntos = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_PUNTOS_ID, "NOCOMBO", 0, "SI"),
                               //RotacionExternaObs = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_DOLOR_ID, "SICOMBO", 111, "NO"),
                               //RotacionExternaHombroInternoOptimo = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_OPTIMO_ID, "NOCOMBO", 0, "SI"),
                               //RotacionExternaHombroInternoLimitado = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_LIMITADO_ID, "NOCOMBO", 0, "SI"),
                               //RotacionExternaHombroInternoMuyLimitado = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_MUY_LIMITADO_ID, "NOCOMBO", 0, "SI"),
                               //RotacionExternaHombroInternoPuntos = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_PUNTOS_ID, "NOCOMBO", 0, "SI"),
                               //RotacionExternaHombroInternoObs = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_ROTACION_EXTERNA_HOMBRO_INTERNO_DOLOR_ID, "SICOMBO", 111, "NO"),
                               //Total1 = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_TOTAL1_ID, "NOCOMBO", 0, "SI"),
                               //Total2 = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_TOTAL2_ID, "NOCOMBO", 0, "SI"),
                               //AptitudMusculoEsqueletico = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_APTITUD_ID, "NOCOMBO", 0, "SI"),
                               //Conclusiones = GetServiceComponentFielValue(a.v_ServiceId, Constants.OSTEO_MUSCULAR_ID, Constants.OSTEO_MUSCULAR_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),


                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }


        // Alberto
        public List<ReportAlturaEstructural> GetAlturaEstructural(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()
                                 join F in dbContext.systemuser on E.i_ApprovedUpdateUserId equals F.i_SystemUserId
                                 join G in dbContext.professional on F.v_PersonId equals G.v_PersonId
                                 

                                 where A.v_ServiceId == pstrserviceId
                                 select new ReportAlturaEstructural
                                 {
                                     v_ComponentId = E.v_ServiceComponentId,
                                     v_ServiceId = A.v_ServiceId,
                                     NombrePaciente = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     EmpresaTrabajadora = D.v_Name,
                                     Fecha = A.d_ServiceDate.Value,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     PuestoTrabajo = B.v_CurrentOccupation,
                                     ServicioId = A.v_ServiceId,
                                     RubricaMedico = G.b_SignatureImage,
                                     RubricaTrabajador = B.b_RubricImage,
                                     HuellaTrabajador = B.b_FingerPrintImage
                                 });


                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter(); 

                var funcionesVitales = serviceBL.ReportFuncionesVitales(pstrserviceId, Constants.FUNCIONES_VITALES_ID);
                var antropometria = serviceBL.ReportAntropometria(pstrserviceId, Constants.ANTROPOMETRIA_ID);

                var sql = (from a in objEntity.ToList()
                           select new ReportAlturaEstructural
                            {
                                v_ComponentId = a.v_ComponentId,
                                v_ServiceId = a.v_ServiceId,
                               ServicioId = a.ServicioId,
                               NombrePaciente = a.NombrePaciente,
                               EmpresaTrabajadora =a.EmpresaTrabajadora,
                               Fecha = a.Fecha,
                               FechaNacimiento = a.FechaNacimiento,
                               Edad = GetAge(a.FechaNacimiento),
                               PuestoTrabajo = a.PuestoTrabajo,

                               AntecedenteTecSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ANTECEDENTE_TEC_SI_ID, "NOCOMBO", 0, "SI"),
                               AntecedenteTecNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ANTECEDENTE_TEC_NO_ID, "NOCOMBO", 0, "SI"),
                               AntecedenteTecObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ANTECEDENTE_TEC_OBS_ID, "NOCOMBO", 0, "SI"),

                               ConvulsionesSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CONVULSIONES_EPILEPSIA_SI_ID, "NOCOMBO", 0, "SI"),
                               ConvulsionesNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CONVULSIONES_EPILEPSIA_NO_ID, "NOCOMBO", 0, "SI"),
                               ConvulsionesObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CONVULSIONES_EPILEPSIA_OBS_ID, "NOCOMBO", 0, "SI"),

                               MareosSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_MAREOS_SI_ID, "NOCOMBO", 0, "SI"),
                               MareosNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_MAREOS_NO_ID, "NOCOMBO", 0, "SI"),
                               MareosObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_MAREOS_OBS_ID, "NOCOMBO", 0, "SI"),

                               AgorafobiaSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_AGORAFOBIA_SI_ID, "NOCOMBO", 0, "SI"),
                               AgorafobiaNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_AGORAFOBIA_NO_ID, "NOCOMBO", 0, "SI"),
                               AgorafobiaObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_AGORAFOBIA_OBS_ID, "NOCOMBO", 0, "SI"),

                               AcrofobiaSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ACROFOBIA_SI_ID, "NOCOMBO", 0, "SI"),
                               AcrofobiaNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ACROFOBIA_NO_ID, "NOCOMBO", 0, "SI"),
                               AcrofobiaObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ACROFOBIA_OBS_ID, "NOCOMBO", 0, "SI"),

                               InsuficienciaCardiacaSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_INSUFICIENCIA_CARDIACA_SI_ID, "NOCOMBO", 0, "SI"),
                               InsuficienciaCardiacaNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_INSUFICIENCIA_CARDIACA_NO_ID, "NOCOMBO", 0, "SI"),
                               InsuficienciaCardiacaObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_INSUFICIENCIA_CARDIACA_OBS_ID, "NOCOMBO", 0, "SI"),

                               EstereopsiaSI = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ESTEREOPSIA_SI_ID, "NOCOMBO", 0, "SI"),
                               EstereopsiaNO = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ESTEREOPSIA_NO_ID, "NOCOMBO", 0, "SI"),
                               EstereopsiaObs = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ESTEREOPSIA_OBS_ID, "NOCOMBO", 0, "SI"),

                               NistagmusEspontaneo = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_NISTAGMUS_ESPONTANEO_ID, "NOCOMBO", 0, "SI"),
                               NistagmusProvocado = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_NISTAGMUS_PROVOCADO_ID, "NOCOMBO", 0, "SI"),

                               PrimerosAuxilios = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_PRIMEROS_AUXILIOS_ID, "NOCOMBO", 0, "SI"),
                               TrabajoNivelMar = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_TRABAJO_SOBRE_NIVEL_ID, "NOCOMBO", 0, "SI"),

                               Timpanos = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_TIMPANOS_ID, "NOCOMBO", 0, "SI"),
                               Equilibrio = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_EQUILIBRIO_ID, "NOCOMBO", 0, "SI"),
                               SustentacionPie20Seg = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_SUST_PIE_20_ID, "NOCOMBO", 0, "SI"),
                               CaminarLibre3Mts = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CAMINAR_LIBRE_RECTA_3_ID, "NOCOMBO", 0, "SI"),
                               CaminarLibreVendado3Mts = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CAMINAR_LIBRE_OJOS_VENDADOS_3_ID, "NOCOMBO", 0, "SI"),
                               CaminarLibreVendadoPuntaTalon3Mts = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_CAMINAR_LIBRE_OJOS_VENDADOS_PUNTA_TALON_3_ID, "NOCOMBO", 0, "SI"),
                               Rotar = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ROTAR_SILLA_GIRATORIA_ID, "NOCOMBO", 0, "SI"),
                               AdiadocoquinesiaDirecta = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ADIADOCOQUINESIA_DIRECTA_ID, "NOCOMBO", 0, "SI"),
                               AdiadocoquinesiaCruzada = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_ADIADOCOQUINESIA_CRUZADA_ID, "NOCOMBO", 0, "SI"),
                               Apto = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_APTO_ID, "NOCOMBO", 0, "SI"),
                               descripcion = GetServiceComponentFielValue(a.ServicioId, Constants.ALTURA_ESTRUCTURAL_ID, Constants.ALTURA_ESTRUCTURAL_DESCRIPCION_ID, "NOCOMBO", 0, "SI"),
                               RubricaMedico = a.RubricaMedico,
                               RubricaTrabajador = a.RubricaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,

                                b_Logo = MedicalCenter.b_Image,
                                EmpresaPropietaria = MedicalCenter.v_Name,
                                EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                                EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                                EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                                IMC = antropometria.Count == 0 ? string.Empty : antropometria[0].IMC,
                                Peso = antropometria.Count == 0 ? string.Empty : antropometria[0].Peso,
                                FC = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].PA,
                                PA = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].PA,
                                FR = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].FR,
                                Sat = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].Sat,
                                PAD = funcionesVitales.Count == 0 ? string.Empty : funcionesVitales[0].PAD,
                                talla = antropometria.Count == 0 ? string.Empty : antropometria[0].talla,
                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // new
        public List<ServiceList> ReportAlturaFisica(string pstrserviceId, string pstrComponentId)
        {
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 //join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()
                                 //***************************************************************************************

                                 where A.v_ServiceId == pstrserviceId

                                 select new ServiceList
                                 {
                                     v_PersonId = A.v_PersonId,
                                     v_NamePacient = B.v_FirstName,
                                     v_Surnames = B.v_FirstLastName + " " + B.v_SecondLastName,
                                     d_BirthDate = B.d_Birthdate,
                                     d_ServiceDate = A.d_ServiceDate,
                                     v_ServiceId = A.v_ServiceId,
                                     v_DocNumber = B.v_DocNumber,
                                     i_SexTypeId = B.i_SexTypeId.Value,
                                     FirmaTrabajador = B.b_RubricImage, // B.b_RubricImage == null ? new byte[0]  FirmaMedico: 
                                     HuellaTrabajador = B.b_FingerPrintImage,//B.b_FingerPrintImage == null ? new byte[0] : ,
                                     FirmaMedico = pme.b_SignatureImage

                                 }).ToList();

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();

                #region Variable data
                         
                var altFisica = serviceBL.ValoresComponentes(pstrserviceId, Constants.ALTURA_FISICA_M_18);
                
                var xAgrofobia = string.Empty;
                var xAcrofobia = string.Empty;
                var xAlcohol = string.Empty;
                var xDrogas = string.Empty;
                var xTrauEnce = string.Empty;
                var xConvulsiones = string.Empty;
                var xVertigo = string.Empty;
                var xSincope = string.Empty;
                var xMioclonias = string.Empty;
                var xAcatisia = string.Empty;
                var xCefalea = string.Empty;
                var xDiabetes = string.Empty;
                var xInsuficiencia = string.Empty;
                var xHipertension = string.Empty;

                var xAlteraciones = string.Empty;
                var xAmetropia = string.Empty;
                var xEsteropsia = string.Empty;
                var xAsma = string.Empty;
                var xHipoacusia = string.Empty;
                var xEntrPrimAux = string.Empty;
                var xEntrTraAlt = string.Empty;
                var xEmfPsiqui = string.Empty;
                var xObs1 = string.Empty;
                var xTimpanos = string.Empty;
                var xAudicion = string.Empty;
                var xSustentacion = string.Empty;

                var xCamina3mts = string.Empty;
                var xCaminaOjosVendados3mts = string.Empty;
                var xCaminaPuntaTalon = string.Empty;
                var xImitacion = string.Empty;
                var xAdiodocoquinesiaDirecta = string.Empty;
                var xAdiodocoquinesiaCruzada = string.Empty;

                var xNistagmus = string.Empty;
                var xObs2 = string.Empty;
                var xAptoMayorAltura18Mts = string.Empty;

                if (altFisica.Count > 0)
                {
                    var Agrofobia = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_AGORAFOBIA);
                    var Acrofobia = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_ACROFOBIA);
                    var Alcohol = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_CONSUMO_ALCOHOL);
                    var Drogas = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_CONSUMO_DROGAS);
                    var TrauEnce = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_TRAUMA_ENCEFALO);
                    var Convulsiones = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_CONVULSIONES);
                    var Vertigo = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_VERTIGO_MAREO);
                    var Sincope = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_SINCOPE);
                    var Mioclonias = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_MIOCLONIAS);
                    var Acatisia = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_ACATISIA);
                    var Cefalea = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_CEFALEA);
                    var Diabetes = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_DIABETES_NOCONTROLADA);
                    var Insuficiencia = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_INSUFICIENCIA_CARDIACA);
                    var Hipertension = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_HIPERTENSION_NOCONTROLADA);

                    var Alteraciones = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_ALTERACIONESCARDIOVASCULARES);
                    var Ametropia = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_AMETROPIA_LEJOS);
                    var Esteropsia = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_ESTEROPSIA_ALTERADA);
                    var Asma = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_ASMA);
                    var Hipoacusia = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_HIPOACUSIA_SEVERA);
                    var EntrPrimAux = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_ENTR_PRIMEROS_AUX);
                    var EntrTraAlt = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_ENTR_TRABAJO_ALTURA);
                    var EmfPsiqui = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_EMF_PSIQUI);
                    var Obs1 = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_OBS1);
                    var Timpanos = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_TIMPANOS);
                    var Audicion = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_AUDICION);
                    var Sustentacion = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_SUS_PIE_15SEG);

                    var Camina3mts = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_CAMINA_LIBRE_RECTA_3MTS);
                    var CaminaOjosVendados3mts = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_CAMINA_LIBRE_OJOS_VENDADOS);
                    var CaminaPuntaTalon = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_CAMINA_LIBRE_OJOS_VENDADOS_PUNTA_TALON);
                    var Imitacion = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_LIMITACION_FUEZA_EXTREMIEDADES);
                    var AdiodocoquinesiaDirecta = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_ADIADOCOQUINESIA_DIRECTA);
                    var AdiodocoquinesiaCruzada = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_ADIADOCOQUINESIA_CRUZADA);

                    var  Nistagmus = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_NISTAGMUS);
                    var Obs2 = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_OBS2);
                    var AptoMayorAltura18Mts = altFisica.Find(p => p.v_ComponentFieldId == Constants.ALTURA_FISICA_M_18_APTO_PARA_ALTURA_MTS18);

                     if (Agrofobia != null)
                        xAgrofobia = Agrofobia.v_Value1;

                      if (Acrofobia != null)
                        xAcrofobia = Acrofobia.v_Value1;

                      if (Alcohol != null)
                        xAlcohol = Alcohol.v_Value1;

                      if (Drogas != null)
                        xDrogas = Drogas.v_Value1;

                      if (TrauEnce != null)
                        xTrauEnce = TrauEnce.v_Value1;

                      if (Convulsiones != null)
                        xConvulsiones = Convulsiones.v_Value1;

                      if (Vertigo != null)
                        xVertigo = Vertigo.v_Value1;

                      if (Sincope != null)
                        xSincope = Sincope.v_Value1;

                      if (Mioclonias != null)
                        xMioclonias = Mioclonias.v_Value1;

                      if (Acatisia != null)
                        xAcatisia = Acatisia.v_Value1;

                      if (Cefalea != null)
                        xCefalea = Cefalea.v_Value1;

                      if (Diabetes != null)
                        xDiabetes = Diabetes.v_Value1;

                      if (Insuficiencia != null)
                        xInsuficiencia = Insuficiencia.v_Value1;

                      if (Hipertension != null)
                        xHipertension = Hipertension.v_Value1;

                      if (Alteraciones != null)
                        xAlteraciones = Alteraciones.v_Value1;

                      if (Ametropia != null)
                        xAmetropia = Ametropia.v_Value1;

                      if (Esteropsia != null)
                        xEsteropsia = Esteropsia.v_Value1;

                      if (Esteropsia != null)
                        xEsteropsia = Esteropsia.v_Value1;

                      if (Asma != null)
                        xAsma = Asma.v_Value1;

                      if (Hipoacusia != null)
                        xHipoacusia = Hipoacusia.v_Value1;

                      if (EntrPrimAux != null)
                        xEntrPrimAux = EntrPrimAux.v_Value1;

                      if (EntrTraAlt != null)
                        xEntrTraAlt = EntrTraAlt.v_Value1;

                      if (EmfPsiqui != null)
                        xEmfPsiqui = EmfPsiqui.v_Value1;

                      if (Obs1 != null)
                        xObs1 = Obs1.v_Value1;

                      if (Timpanos != null)
                        xTimpanos = Timpanos.v_Value1;

                     if (Audicion != null)
                        xAudicion = Audicion.v_Value1;

                     if (Sustentacion != null)
                        xSustentacion = Sustentacion.v_Value1;

                     if (Camina3mts != null)
                        xCamina3mts = Camina3mts.v_Value1;

                     if (CaminaOjosVendados3mts != null)
                        xCaminaOjosVendados3mts = CaminaOjosVendados3mts.v_Value1;

                     if (CaminaPuntaTalon != null)
                        xCaminaPuntaTalon = CaminaPuntaTalon.v_Value1;

                     if (Imitacion != null)
                        xImitacion = Imitacion.v_Value1;

                      if (AdiodocoquinesiaDirecta != null)
                        xAdiodocoquinesiaDirecta = AdiodocoquinesiaDirecta.v_Value1;

                      if (AdiodocoquinesiaCruzada != null)
                        xAdiodocoquinesiaCruzada = AdiodocoquinesiaCruzada.v_Value1;

                      if (Nistagmus != null)
                        xNistagmus = Nistagmus.v_Value1;

                      if (Obs2 != null)
                        xObs2 = Obs2.v_Value1;

                      if (AptoMayorAltura18Mts != null)
                        xAptoMayorAltura18Mts = AptoMayorAltura18Mts.v_Value1;


                }


                #endregion


                var sql = (from a in objEntity.ToList()
                           select new ServiceList
                           {
                               v_ServiceId = a.v_ServiceId,
                               v_PersonId = a.v_PersonId,
                               v_NamePacient = a.v_NamePacient,
                               v_Surnames = a.v_Surnames,
                               d_BirthDate = a.d_BirthDate,
                               d_ServiceDate = a.d_ServiceDate,
                               i_AgePacient = GetAge(a.d_BirthDate.Value),
                               v_DocNumber = a.v_DocNumber,
                               i_SexTypeId = a.i_SexTypeId,
                               Agrofobia = xAgrofobia,
                               Acrofobia = xAcrofobia,
                               Alcohol = xAlcohol,
                               Drogas = xDrogas,
                               TrauEnce = xTrauEnce,
                               Convulsiones = xConvulsiones,
                               Vertigo = xVertigo,
                               Sincope = xSincope,
                               Mioclonias = xMioclonias,
                               Acatisia = xAcatisia,
                               Cefalea = xCefalea,
                               Diabetes = xDiabetes,
                               Insuficiencia = xInsuficiencia,
                               Hipertension = xHipertension,
                               Alteraciones = xAlteraciones,
                               Ametropia = xAmetropia,
                               Esteropsia = xEsteropsia,
                               Asma = xAsma,
                               Hipoacusia = xHipoacusia,
                               EntrPrimAux = xEntrPrimAux,
                               EntrTraAlt = xEntrTraAlt,
                               EmfPsiqui = xEmfPsiqui,

                               Obs1 = xObs1,
                               Timpanos = xTimpanos,
                               Audicion = xAudicion,
                               Sustentacion = xSustentacion,
                               Camina3mts = xCamina3mts,
                               CaminaOjosVendados3mts = xCaminaOjosVendados3mts,
                               CaminaPuntaTalon = xCaminaPuntaTalon,
                               Imitacion = xImitacion,
                               AdiodocoquinesiaDirecta = xAdiodocoquinesiaDirecta,
                               AdiodocoquinesiaCruzada = xAdiodocoquinesiaCruzada,
                               Nistagmus = xNistagmus,
                               Obs2 = xObs2,
                               AptoMayorAltura18Mts = xAptoMayorAltura18Mts,

                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,
                               FirmaMedico = a.FirmaMedico,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                           }).ToList();

                return sql;
            }
            catch (Exception)
            {

                throw;
            }
        }


        // Alberto
        public List<ReportOftalmologia> GetOftalmologia(string pstrserviceId, string pstrComponentId)
        {
       

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                                 join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId
                                 join D in dbContext.organization on C.v_WorkingOrganizationId equals D.v_OrganizationId
                                 join E in dbContext.servicecomponent on new { a = A.v_ServiceId, b = pstrComponentId }
                                                                        equals new { a = E.v_ServiceId, b = E.v_ComponentId }

                                 join J1 in dbContext.datahierarchy on new { a = B.i_LevelOfId.Value, b = 108 }
                                                                    equals new { a = J1.i_ItemId, b = J1.i_GroupId } into J1_join
                                 from J1 in J1_join.DefaultIfEmpty()                          
                              
                                 // Usuario Medico Evaluador / Medico Aprobador ****************************
                                 join me in dbContext.systemuser on E.i_ApprovedUpdateUserId equals me.i_SystemUserId into me_join
                                 from me in me_join.DefaultIfEmpty()

                                 join pme in dbContext.professional on me.v_PersonId equals pme.v_PersonId into pme_join
                                 from pme in pme_join.DefaultIfEmpty()

                                 // Usuario Tecnologo *************************************
                                 join tec in dbContext.systemuser on E.i_UpdateUserTechnicalDataRegisterId equals tec.i_SystemUserId into tec_join
                                 from tec in tec_join.DefaultIfEmpty()

                                 join prtec in dbContext.professional on tec.v_PersonId equals prtec.v_PersonId into prtec_join
                                 from prtec in prtec_join.DefaultIfEmpty()

                                 join petec in dbContext.person on tec.v_PersonId equals petec.v_PersonId into petec_join
                                 from petec in petec_join.DefaultIfEmpty()
                                 // *******************************************************                            
                               
                                 where A.v_ServiceId == pstrserviceId
                               
                                 select new ReportOftalmologia
                                 {
                                     v_ComponentId =  E.v_ComponentId,
                                     v_ServiceId = A.v_ServiceId,
                                     ServicioId = A.v_ServiceId,
                                     NombrePaciente = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                     EmprresaTrabajadora = D.v_Name,
                                     FechaServicio = A.d_ServiceDate.Value,
                                     FechaNacimiento = B.d_Birthdate.Value,
                                     PuestoTrabajo = B.v_CurrentOccupation,
                                     FirmaDoctor = pme.b_SignatureImage,
                                     FirmaTecnologo = prtec.b_SignatureImage,
                                     NombreTecnologo = petec.v_FirstLastName + " " + petec.v_SecondLastName + " " + petec.v_FirstName,
                                     v_ServiceComponentId = E.v_ServiceComponentId
                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();

               
                              var sql = (from a in objEntity.ToList()

                                         let oftalmo = serviceBL.ValoresComponente(pstrserviceId, Constants.OFTALMOLOGIA_ID)

                                         select new ReportOftalmologia
                                         {
                                             v_ComponentId = a.v_ServiceComponentId,
                                             v_ServiceId = a.v_ServiceId,
                                             ServicioId = a.ServicioId,
                                             NombrePaciente = a.NombrePaciente,
                                             EmprresaTrabajadora = a.EmprresaTrabajadora,
                                             FechaServicio = a.FechaServicio,
                                             FechaNacimiento = a.FechaNacimiento,
                                             Edad = GetAge(a.FechaNacimiento),
                                             PuestoTrabajo = a.PuestoTrabajo,

                                             UsoCorrectoresSi = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORRECTORES_OCULARES_SI_ID).v_Value1,
                                             UsoCorrectoresNo = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORRECTORES_OCULARES_NO_ID).v_Value1,
                                             UltimaRefraccion = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORRECTORES_OCULARES_ULTIMA_REFRACCION_ID).v_Value1,
                                             Hipertension = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_HIPERTENSION_ID).v_Value1,
                                             DiabetesMellitus = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_DIABETES_ID).v_Value1,
                                             Glaucoma = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_GLAUCOMA_ID).v_Value1,
                                             TraumatismoOcular = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TRAUMATISMO_ID).v_Value1,
                                             Ambliopia = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AMBLIOPIA_ID).v_Value1,
                                             SustQuimica = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SUST_QUIMICAS_ID).v_Value1,
                                             Soldadura = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SOLDADURA_ID).v_Value1,
                                             Catarata = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CATARATAS_ID).v_Value1,
                                             Otros = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_OTROS_ESPECIFICAR_ID).v_Value1,
                                             AELejosOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AE_LEJOS_OJO_DERECHO_ID).v_Value1,
                                             SCCercaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SC_CERCA_OJO_IZQUIERDO_ID).v_Value1,
                                             //AELejosOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AE_LEJOS_OJO_IZQUIERDO_ID).v_Value1,
                                             SCLejosOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SC_LEJOS_OJO_IZQUIERDO_ID).v_Value1,
                                             CCLejosOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CC_LEJOS_OJO_DERECHO_ID).v_Value1,
                                             SCCercaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SC_CERCA_OJO_DERECHO_ID).v_Value1,
                                             SCLejosOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_SC_LEJOS_OJO_DERECHO_ID).v_Value1,
                                             CCCercasOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CC_CERCA_OJO_DERECHO_ID).v_Value1,
                                             CCLejosOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CC_LEJOS_OJO_IZQUIERDO_ID).v_Value1,
                                             //AECercaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AE_CERCA_OJO_IZQUIERDO_ID).v_Value1,
                                             //AECercaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_AE_CERCA_OJO_DERECHO_ID).v_Value1,
                                             CCCercasOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CC_CERCA_OJO_IZQUIERDO_ID).v_Value1,
                                             //MaculaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_MACULA_OJO_DERECHO_ID).v_Value1,
                                             //MaculaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_MACULA_OJO_IZQUIERDO_ID).v_Value1,
                                             //RetinaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_RETINA_OJO_DERECHO_ID).v_Value1,
                                             //RetinaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_RETINA_OJO_IZQUIERDO_ID).v_Value1,
                                             //NervioOpticoOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_NERVIO_OPTICO_DERECHO_ID).v_Value1,
                                             //NervioOpticoOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_NERVIO_OPTICO_IZQUIERDO_ID).v_Value1,
                                             ParpadoOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_PARPADO_OJO_DERECHO_ID).v_Value1,
                                             ParpadoOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_PARPADO_OJO_IZQUIERDO_ID).v_Value1,
                                             ConjuntivaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONJUNTIVA_OJO_DERECHO_ID).v_Value1,
                                             ConjuntivaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONJUNTIVA_OJO_IZQUIERDO_ID).v_Value1,
                                             CorneaOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORNEA_OJO_DERECHO_ID).v_Value1,
                                             CorneaOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CORNEA_OJO_IZQUIERDO_ID).v_Value1,
                                             //CristalinoOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CRISTALINO_OJO_DERECHO_ID).v_Value1,
                                             //CristalinoOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CRISTALINO_OJO_IZQUIERDO_ID).v_Value1,
                                             IrisOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_IRIS_OJO_DERECHO_ID).v_Value1,
                                             IrisOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_IRIS_OJO_IZQUIERDO_ID).v_Value1,
                                             MovOcularesOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_MOV_OCULARES_OJO_DERECHO_ID).v_Value1,
                                             MovOcularesOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_MOV_OCULARES_OJO_IZQUIERDO_ID).v_Value1,
                                             ConfrontacionODCompleto = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONFRONTACION_CAMPO_COMPLETO_OJO_DERECHO_ID).v_Value1,
                                             ConfrontacionODRestringido = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONFRONTACION_CAMPO_RESTRINGIDO_OJO_DERECHO_ID).v_Value1,
                                             //ConfrontacionOICompleto = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONFRONTACION_CAMPO_COMPLETO_OJO_IZQUIERDO_ID).v_Value1,
                                             //ConfrontacionOIRestringido = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_CONFRONTACION_CAMPO_RESTRINGIDO_OJO_IZQUIERDO_ID).v_Value1,
                                             TestIshiharaNormal = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ISHIHARA_NORMAL_ID).v_Value1,
                                             TestIshiharaAnormal = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ISHIHARA_ANORMAL_ID).v_Value1,
                                             Discromatopsia = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_DICROMATOPSIA_ID).v_Value1Name,
                                             TestEstereopsisTiempo = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ESTEREOPSIS_TIEMPO_ID).v_Value1,
                                             TestEstereopsisNormal = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ESTEREOPSIS_NORMAL_ID).v_Value1,
                                             TestEstereopsisAnormal = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_TEST_ESTEREOPSIS_ANORMAL_ID).v_Value1,
                                             PresionIntraocularOD = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_PRESION_INTRAOCULAR_OJO_DERECHO_ID).v_Value1,
                                             //PresionIntraocularOI = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_PRESION_INTRAOCULAR_OJO_IZQUIERDO_ID).v_Value1,
                                             Hallazgos = oftalmo.Count == 0 ? string.Empty : oftalmo.Find(p => p.v_ComponentFieldId == Constants.OFTALMOLOGIA_HALLAZGOS_ID).v_Value1,
                                             Diagnosticos = GetDisgnosticsByServiceIdAndComponentConcatec(a.ServicioId, Constants.OFTALMOLOGIA_ID),
                                             Recomendaciones = GetRecomendationByServiceIdAndComponentConcatec(a.ServicioId, Constants.OFTALMOLOGIA_ID),
                                             FirmaDoctor = a.FirmaDoctor,
                                             FirmaTecnologo = a.FirmaTecnologo,
                                             NombreTecnologo = a.NombreTecnologo,

                                             b_Logo = MedicalCenter.b_Image,
                                             EmpresaPropietaria = MedicalCenter.v_Name,
                                             EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                                             EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                                             EmpresaPropietariaEmail = MedicalCenter.v_Mail,

                                         }).ToList();

                return sql;
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetDisgnosticsByServiceIdAndComponentConcatec(string pstrServiceId, string pstrComponentId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
               var query = (from ccc in dbContext.diagnosticrepository
                                                        join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos                                                  

                                                       
                                                        where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponentId &&
                                                              ccc.i_IsDeleted == 0

                                                        select new 
                                                        {
                                                         
                                                            v_DiseasesName = ddd.v_Name,

                                                        }).ToList();


               return string.Join(", ", query.Select(p => p.v_DiseasesName));
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        public string GetRecomendationByServiceIdAndComponentConcatec(string pstrServiceId, string pstrComponentId)
        {
            //mon.IsActive = true;

            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
                var query = (from ccc in dbContext.recommendation
                             join ddd in dbContext.masterrecommendationrestricction on ccc.v_MasterRecommendationId equals ddd.v_MasterRecommendationRestricctionId                                                  


                             where ccc.v_ServiceId == pstrServiceId && ccc.v_ComponentId == pstrComponentId &&
                                   ccc.i_IsDeleted == 0

                             select new
                             {

                                 Recomendations = ddd.v_Name,

                             }).ToList();


                return string.Join(", ", query.Select(p => p.Recomendations));
            }
            catch (Exception ex)
            {

                return null;
            }
        }




        // Alberto
        public string GetServiceComponentFielValue(string pstrServiceId, string pstrComponentId, string pstrFieldId, string Type , int pintParameter, string pstrConX )
        {
            try
            {
                ServiceBL oServiceBL = new ServiceBL();
                List<ServiceComponentFieldValuesList> oServiceComponentFieldValuesList = new List<ServiceComponentFieldValuesList>();
                string xx = "" ;
                if (Type == "NOCOMBO")
                {
                   oServiceComponentFieldValuesList = oServiceBL.ValoresComponente(pstrServiceId, pstrComponentId);
                   xx = oServiceComponentFieldValuesList.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;
                }
                else
                {
                    oServiceComponentFieldValuesList = oServiceBL.ValoresExamenComponete(pstrServiceId, pstrComponentId, pintParameter);
                    if (pstrConX == "SI")
                    {
                        xx = oServiceComponentFieldValuesList.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1;
                    }
                    else
                    {
                        xx = oServiceComponentFieldValuesList.Count() == 0 ? string.Empty : ((ServiceComponentFieldValuesList)oServiceComponentFieldValuesList.Find(p => p.v_ComponentFieldId == pstrFieldId)).v_Value1Name;
                    }
                    
                }
               
                return xx;
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        //Alberto
        public List<ReportConsentimiento> GetReportConsentimiento(string pstrServiceId)
        {
            //mon.IsActive = true;
            var groupUbigeo = 113;
            try
            {
                SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

                var objEntity = (from A in dbContext.service
                                 join B in dbContext.protocol on A.v_ProtocolId equals B.v_ProtocolId into B_join
                                 from B in B_join.DefaultIfEmpty()

                                 join C in dbContext.organization on A.v_EmpresaFacturacionId equals C.v_OrganizationId into C_join
                                 from C in C_join.DefaultIfEmpty()
                      
                                 join P1 in dbContext.person on new { a = A.v_PersonId }
                                         equals new { a = P1.v_PersonId } into P1_join
                                 from P1 in P1_join.DefaultIfEmpty()

                                 join p in dbContext.person on A.v_PersonId equals p.v_PersonId

                                 // Ubigeo de la persona *******************************************************
                                 join dep in dbContext.datahierarchy on new { a = p.i_DepartmentId.Value, b = groupUbigeo }
                                                      equals new { a = dep.i_ItemId, b = dep.i_GroupId } into dep_join
                                 from dep in dep_join.DefaultIfEmpty()

                                 join prov in dbContext.datahierarchy on new { a = p.i_ProvinceId.Value, b = groupUbigeo }
                                                       equals new { a = prov.i_ItemId, b = prov.i_GroupId } into prov_join
                                 from prov in prov_join.DefaultIfEmpty()

                                 join distri in dbContext.datahierarchy on new { a = p.i_DistrictId.Value, b = groupUbigeo }
                                                       equals new { a = distri.i_ItemId, b = distri.i_GroupId } into distri_join
                                 from distri in distri_join.DefaultIfEmpty()
                                 //*********************************************************************************************

                                 let varDpto = dep.v_Value1 == null ? "" : dep.v_Value1
                                 let varProv = prov.v_Value1 == null ? "" : prov.v_Value1
                                 let varDistri = distri.v_Value1 == null ? "" : distri.v_Value1

                                 where A.v_ServiceId == pstrServiceId

                                 select new ReportConsentimiento
                                 {
                                     NombreTrabajador = P1.v_FirstName + " " + P1.v_FirstLastName +  " " + P1.v_SecondLastName,
                                     NroDocumento = P1.v_DocNumber,
                                     Ocupacion = P1.v_CurrentOccupation,
                                     Empresa = C.v_Name,
                                     FirmaTrabajador = P1.b_RubricImage,
                                     HuellaTrabajador = P1.b_FingerPrintImage,
                                     LugarProcedencia = varDistri + "-" + varProv + "-" + varDpto, // Santa Anita - Lima - Lima
                                     v_AdressLocation = p.v_AdressLocation,
                                     d_ServiceDate = A.d_ServiceDate,

                                 });

                var serviceBL = new ServiceBL();
                var MedicalCenter = serviceBL.GetInfoMedicalCenter();

                var sql = (from a in objEntity.ToList()
                           select new ReportConsentimiento
                           {
                               Fecha = DateTime.Now.ToShortDateString(),
                               Logo = MedicalCenter.b_Image,
                               NombreTrabajador = a.NombreTrabajador,
                               NroDocumento = a.NroDocumento,
                               Ocupacion = a.Ocupacion,
                               Empresa = a.Empresa,
                               FirmaTrabajador = a.FirmaTrabajador,
                               HuellaTrabajador = a.HuellaTrabajador,

                               b_Logo = MedicalCenter.b_Image,
                               EmpresaPropietaria = MedicalCenter.v_Name,
                               EmpresaPropietariaDireccion = MedicalCenter.v_Address,
                               EmpresaPropietariaTelefono = MedicalCenter.v_PhoneNumber,
                               EmpresaPropietariaEmail = MedicalCenter.v_Mail,
                               LugarProcedencia = a.LugarProcedencia,
                               v_AdressLocation = a.v_AdressLocation,
                               v_ServiceDate = a.d_ServiceDate == null ? string.Empty : a.d_ServiceDate.Value.ToShortDateString(),

                           }).ToList();

                return sql;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion       

        public List<JerarquiaServicioCamposValores> DevolverValorCampoPorServicioMejorado(List<string> ListaServicioIds)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            int isDeleted = (int)SiNo.NO;

            try
            {
                int rpta = 0;
                var PreQuery = (from A in dbContext.service
                                join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
                                join F in dbContext.componentfields on C.v_ComponentFieldId equals F.v_ComponentFieldId
                                join G in dbContext.componentfield on C.v_ComponentFieldId equals G.v_ComponentFieldId
                                join H in dbContext.component on F.v_ComponentId equals H.v_ComponentId
                                where B.i_IsDeleted == isDeleted
                                     && C.i_IsDeleted == isDeleted
                                     && ListaServicioIds.Contains(A.v_ServiceId)
                                //&& A.d_ServiceDate < FechaFin && A.d_ServiceDate > FechaIni

                                orderby A.v_ServiceId
                                select new ValorComponenteList
                                {
                                    ServicioId = A.v_ServiceId,
                                    Valor = D.v_Value1,
                                    NombreComponente = H.v_Name,
                                    IdComponente = H.v_ComponentId,
                                    NombreCampo = G.v_TextLabel,
                                    IdCampo = C.v_ComponentFieldId,
                                    i_GroupId = G.i_GroupId.Value
                                }

                            );

                var finalQuery = (from a in PreQuery.ToList()

                                  let value1 = int.TryParse(a.Valor, out rpta)
                                  join sp in dbContext.systemparameter on new { a = a.i_GroupId, b = rpta }
                                                  equals new { a = sp.i_GroupId, b = sp.i_ParameterId } into sp_join
                                  from sp in sp_join.DefaultIfEmpty()

                                  select new ValorComponenteList
                                  {
                                      ServicioId = a.ServicioId,
                                      Valor = a.Valor,
                                      NombreComponente = a.NombreComponente,
                                      IdComponente = a.IdComponente,
                                      NombreCampo = a.NombreCampo,
                                      IdCampo = a.IdCampo,
                                      ValorName = sp == null ? "" : sp.v_Value1
                                  }).ToList();



                var ListaJerarquizada = (from A in dbContext.service
                                         where ListaServicioIds.Contains(A.v_ServiceId)

                                         //A.d_ServiceDate < FechaFin && A.d_ServiceDate > FechaIni
                                         select new JerarquiaServicioCamposValores
                                         {
                                             ServicioId = A.v_ServiceId
                                         }).ToList();

                ListaJerarquizada.ForEach(a =>
                {
                    a.CampoValores = finalQuery.FindAll(p => p.ServicioId == a.ServicioId);
                });


                return ListaJerarquizada;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public string ObtenerMedicoPersonales(string PersonId)
        {
            try
            {
                using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
                {
                    var ListaMedicosPersonales = (from A in dbContext.personmedicalhistory
                                                  join B in dbContext.systemparameter on new { a = A.v_DiseasesId, b = 147 }
                                                     equals new { a = B.v_Value1, b = B.i_GroupId } into B_join
                                                  from B in B_join.DefaultIfEmpty()

                                                  join C in dbContext.systemparameter on new { a = B.i_ParentParameterId.Value, b = 147 }
                                                                                    equals new { a = C.i_ParameterId, b = C.i_GroupId } into C_join
                                                  from C in C_join.DefaultIfEmpty()

                                                  join D in dbContext.diseases on A.v_DiseasesId equals D.v_DiseasesId

                                                  where A.i_IsDeleted == 0
                                                  && A.v_PersonId == PersonId
                                                  orderby A.v_PersonId
                                                  select new PersonMedicalHistoryList
                                                  {
                                                      v_PersonId = A.v_PersonId,
                                                      v_DiseasesId = D.v_DiseasesId,
                                                      v_DiseasesName = D.v_Name,
                                                      i_Answer = A.i_AnswerId.Value,
                                                      v_GroupName = C.v_Value1 == null ? "ENFERMEDADES OTROS" : C.v_Value1,
                                                  }).ToList();

                    return string.Join(",", ListaMedicosPersonales.Select(p =>p.v_DiseasesName + "(" + p.v_GroupName  + ")"));
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public List<DiagnosticosRecomendaciones> DevolverJerarquiaDxMejorado(List<string> ServicioIds)
        {
            try
            {
                int isDeleted = (int)SiNo.NO;
                int definitivo = (int)FinalQualification.Definitivo;
                int presuntivo = (int)FinalQualification.Presuntivo;
                int descartado = (int)FinalQualification.Descartado;

                List<DiagnosticosRecomendaciones> ListaTotalJerarquizada = new List<DiagnosticosRecomendaciones>();
                DiagnosticosRecomendaciones ListaJerarquizadaDxRecomendaciones = new DiagnosticosRecomendaciones();
                List<DiagnosticosRecomendacionesList> ListaDxRecomendacionesPorServicio;

                using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
                {
                    var ListaDxRecoTodos = (from ccc in dbContext.diagnosticrepository
                                            join bbb in dbContext.component on ccc.v_ComponentId equals bbb.v_ComponentId into J7_join
                                            from bbb in J7_join.DefaultIfEmpty()
                                            join ddd in dbContext.diseases on ccc.v_DiseasesId equals ddd.v_DiseasesId  // Diagnosticos 
                                            join eee in dbContext.service on ccc.v_ServiceId equals eee.v_ServiceId
                                            where (ccc.i_IsDeleted == isDeleted) &&
                                                (ccc.i_FinalQualificationId == definitivo ||
                                                ccc.i_FinalQualificationId == presuntivo || ccc.i_FinalQualificationId == descartado)
                                                && ServicioIds.Contains(eee.v_ServiceId)
                                            //&& eee.d_ServiceDate < FeFin && eee.d_ServiceDate > FeIni
                                            orderby eee.v_ServiceId
                                            select new DiagnosticosRecomendacionesList
                                            {
                                                ServicioId = eee.v_ServiceId,
                                                Descripcion = ddd.v_Name,
                                                IdCampo = ccc.v_ComponentFieldId,
                                                Tipo = "D",
                                                IdComponente = bbb.v_ComponentId,
                                                IdDeseases = ddd.v_DiseasesId,
                                                i_FinalQualiticationId = ccc.i_FinalQualificationId,
                                                DiseasesName = ddd.v_Name,
                                                i_DiagnosticTypeId = ccc.i_DiagnosticTypeId
                                            }).Union(from ccc in dbContext.recommendation
                                                     join ddd in dbContext.masterrecommendationrestricction on ccc.v_MasterRecommendationId equals ddd.v_MasterRecommendationRestricctionId  // Diagnosticos      
                                                     join eee in dbContext.service on ccc.v_ServiceId equals eee.v_ServiceId
                                                     where ccc.i_IsDeleted == isDeleted
                                                       && ServicioIds.Contains(eee.v_ServiceId)
                                                     orderby eee.v_ServiceId
                                                     select new DiagnosticosRecomendacionesList
                                                     {
                                                         ServicioId = eee.v_ServiceId,
                                                         Descripcion = ddd.v_Name,
                                                         IdCampo = "sin nada",
                                                         Tipo = "R",
                                                         IdComponente = "sin nada",
                                                         IdDeseases = "sin nada",
                                                         i_FinalQualiticationId = 0,
                                                         DiseasesName = "sin nada",
                                                         i_DiagnosticTypeId = 0
                                                     }).ToList();



                    var ListaJerarquizada = (from A in dbContext.service
                                             where ServicioIds.Contains(A.v_ServiceId)
                                             //A.d_ServiceDate < FeFin && A.d_ServiceDate > FeIni
                                             select new DiagnosticosRecomendaciones
                                             {
                                                 ServicioId = A.v_ServiceId
                                             }).ToList();

                    ListaJerarquizada.ForEach(a =>
                    {
                        a.DetalleDxRecomendaciones = ListaDxRecoTodos.FindAll(p => p.ServicioId == a.ServicioId);
                    });



                    foreach (var item in ListaJerarquizada)
                    {
                        ListaJerarquizadaDxRecomendaciones = new DiagnosticosRecomendaciones();
                        ListaDxRecomendacionesPorServicio = new List<DiagnosticosRecomendacionesList>();

                        ListaJerarquizadaDxRecomendaciones.ServicioId = item.ServicioId;


                        var DetalleTodos = ListaJerarquizada.SelectMany(p => p.DetalleDxRecomendaciones).ToList();

                        //Lista Dx
                        var DetalleDx = DetalleTodos.FindAll(p => p.ServicioId == item.ServicioId && p.Tipo == "D");

                        for (int i = 0; i < 18; i++)
                        {
                            if (i < DetalleDx.Count())
                            {
                                if (i == 17)
                                {
                                    int Contador = DetalleDx.Count - 17;
                                    var x = DetalleDx.GetRange(17, Contador);

                                    DetalleDx[i].Descripcion = string.Join(", ", x.Select(p => p.Descripcion));
                                    ListaDxRecomendacionesPorServicio.Add(DetalleDx[i]);
                                }
                                else
                                {
                                    DetalleDx[i].Descripcion = DetalleDx[i].Descripcion;
                                    ListaDxRecomendacionesPorServicio.Add(DetalleDx[i]);
                                }
                            }
                            else
                            {
                                ListaDxRecomendacionesPorServicio.Add(new DiagnosticosRecomendacionesList());
                            }
                        }

                        //Lista Recomendaciones
                        var DetalleReco = DetalleTodos.FindAll(p => p.ServicioId == item.ServicioId && p.Tipo == "R");

                        for (int i = 0; i < 14; i++)
                        {
                            if (i < DetalleReco.Count())
                            {
                                if (i == 13)
                                {
                                    int Contador = DetalleReco.Count - 13;
                                    var x = DetalleReco.GetRange(13, Contador);

                                    DetalleReco[i].Descripcion = string.Join(", ", x.Select(p => p.Descripcion));
                                    ListaDxRecomendacionesPorServicio.Add(DetalleReco[i]);
                                }
                                else
                                {
                                    DetalleReco[i].Descripcion = DetalleReco[i].Descripcion;
                                    ListaDxRecomendacionesPorServicio.Add(DetalleReco[i]);
                                }
                            }
                            else
                            {
                                ListaDxRecomendacionesPorServicio.Add(new DiagnosticosRecomendacionesList());
                            }
                        }
                        ListaJerarquizadaDxRecomendaciones.DetalleDxRecomendaciones = ListaDxRecomendacionesPorServicio;

                        ListaTotalJerarquizada.Add(ListaJerarquizadaDxRecomendaciones);
                    }
                }
                return ListaTotalJerarquizada;


            }
            catch (Exception)
            {

                throw;
            }
        }

        private string ConcatenateRestrictionByService(string pstrServiceId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from a in dbContext.restriction  // RESTRICCIONES POR Diagnosticos
                       join eee in dbContext.masterrecommendationrestricction on a.v_MasterRestrictionId equals eee.v_MasterRecommendationRestricctionId
                       where a.v_ServiceId == pstrServiceId &&
                       a.i_IsDeleted == 0 && eee.i_TypifyingId == (int)Typifying.Restricciones
                       select new
                       {
                           v_RestrictionsName = eee.v_Name
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_RestrictionsName));
        }
        private string ConcatenateRecommendationByService(string pstrServiceId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var qry = (from a in dbContext.recommendation  // RECOMENDACIONES
                       join eee in dbContext.masterrecommendationrestricction on a.v_MasterRecommendationId equals eee.v_MasterRecommendationRestricctionId
                       join b in dbContext.diagnosticrepository on a.v_DiagnosticRepositoryId equals b.v_DiagnosticRepositoryId
                       where a.v_ServiceId == pstrServiceId &&
                       a.i_IsDeleted == 0 && eee.i_TypifyingId == (int)Typifying.Recomendaciones && b.i_FinalQualificationId != (int)FinalQualification.Descartado
                       select new
                       {
                           v_RecommendationName = eee.v_Name
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_RecommendationName));
        }

        public List<Matriz> ReporteMatrizExcel(DateTime? FechaInicio, DateTime? FechaFin, string Empresa, string pstrFilterExpression)
        {
            try
            {
                using (SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel())
                {
                    List<string> ServiciosIds = new List<string>();
                    List<string> PersonIds = new List<string>();

                    var objEntity = from A in dbContext.service
                                    join B in dbContext.person on A.v_PersonId equals B.v_PersonId

                                    join C in dbContext.protocol on A.v_ProtocolId equals C.v_ProtocolId into C_join
                                    from C in C_join.DefaultIfEmpty()

                                    join D in dbContext.organization on C.v_CustomerOrganizationId equals D.v_OrganizationId into D_join
                                    from D in D_join.DefaultIfEmpty()

                                    join E in dbContext.location on new { a = C.v_CustomerOrganizationId, b = C.v_CustomerLocationId }
                                                                      equals new { a = E.v_OrganizationId, b = E.v_LocationId } into E_join
                                    from E in E_join.DefaultIfEmpty()

                                    join F in dbContext.systemparameter on new { a = B.i_MaritalStatusId.Value, b = 101 }
                                          equals new { a = F.i_ParameterId, b = F.i_GroupId } into F_join
                                    from F in F_join.DefaultIfEmpty()

                                    join I in dbContext.systemparameter on new { a = C.i_EsoTypeId.Value, b = 118 }
                                                    equals new { a = I.i_ParameterId, b = I.i_GroupId } into I_join
                                    from I in I_join.DefaultIfEmpty()

                                    join J in dbContext.groupoccupation on C.v_GroupOccupationId equals J.v_GroupOccupationId

                                    join J4 in dbContext.systemparameter on new { a = A.i_AptitudeStatusId.Value, b = 124 }
                                       equals new { a = J4.i_ParameterId, b = J4.i_GroupId } into J4_join
                                    from J4 in J4_join.DefaultIfEmpty()

                                    where A.d_ServiceDate >= FechaInicio && A.d_ServiceDate <= FechaFin
                                    select new Matriz
                                    {
                                        ServiceId = A.v_ServiceId,
                                        PersonId = B.v_PersonId,
                                        FechaNacimiento = B.d_Birthdate.Value,
                                        Proveedor = "SALUS LABORIS",
                                        Colaborador = B.v_FirstLastName + " " + B.v_SecondLastName + " " + B.v_FirstName,
                                        Dni = B.v_DocNumber,
                                        Sexo = B.i_SexTypeId == 1 ? "M" : "F",
                                        FechaEvaluacion = A.d_ServiceDate.Value,
                                        EmpresaEmpleadora = D.v_Name + " / " + E.v_Name,
                                        Ruc = D.v_IdentificationNumber,
                                        TipoEvaluacion = I.v_Value1,
                                        PerfiExamenOcupacional = C.v_Name,
                                        PuestoTrabajo = B.v_CurrentOccupation,
                                        Area = "N/A",
                                        Actividad = "N/A",
                                        Aptitud = J4.v_Value1,
                                        v_CustomerOrganizationId=  D.v_OrganizationId
                                    };

                    if (!string.IsNullOrEmpty(pstrFilterExpression))
                    {
                        objEntity = objEntity.Where(pstrFilterExpression);
                    }

                    foreach (var item in objEntity)
                    {
                        PersonIds.Add(item.PersonId);
                        ServiciosIds.Add(item.ServiceId);
                    }

                    var varValores = DevolverValorCampoPorServicioMejorado(ServiciosIds);
                    var varDx = DevolverJerarquiaDxMejorado(ServiciosIds);

                    string ValorOD_VC_SC = "", ValorOI_VC_SC = "", ValorOD_VC_CC = "", ValorOI_VC_CC = "";
                    string ValorOD_VL_SC = "", ValorOI_VL_SC = "", ValorOD_VL_CC = "", ValorOI_VL_CC = "";

                    var sql = (from a in objEntity.ToList()

                               let ImcDx = varDx.Find(p => p.ServicioId == a.ServiceId).DetalleDxRecomendaciones.FindAll(o => o.IdComponente == Constants.ANTROPOMETRIA_ID)
                               let IMC_Con = ImcDx != null ? string.Join("/ ", ImcDx.Select(p => p.Descripcion)) : "Normal"

                               let Grupo = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID && o.IdCampo == Constants.GRUPO_SANGUINEO_ID).Valor == "" ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID && o.IdCampo == Constants.GRUPO_SANGUINEO_ID).ValorName
                               let Factor = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID && o.IdCampo == Constants.FACTOR_SANGUINEO_ID).Valor == "" ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.GRUPO_Y_FACTOR_SANGUINEO_ID && o.IdCampo == Constants.FACTOR_SANGUINEO_ID).ValorName

                               //let EKGDx = varDx.Find(p => p.ServicioId == a.ServiceId).DetalleDxRecomendaciones.FindAll(o => o.IdComponente == Constants.ELECTROCARDIOGRAMA_ID)
                               //let EKG_Con = EKGDx != null ? string.Join("/ ", EKGDx.Select(p => p.Descripcion)) : "Normal"

                               let VISUALDx = varDx.Find(p => p.ServicioId == a.ServiceId).DetalleDxRecomendaciones.FindAll(o => o.IdComponente == Constants.AGUDEZA_VISUAL)
                               let VISUAL_Con = VISUALDx != null ? string.Join("/ ", VISUALDx.Select(p => p.Descripcion)) : "Normal"

                               let COLORESDx = varDx.Find(p => p.ServicioId == a.ServiceId).DetalleDxRecomendaciones.FindAll(o => o.IdComponente == Constants.VISION_DE_COLORES_ID)
                               let COLORES_Con = COLORESDx != null ? string.Join("/ ", COLORESDx.Select(p => p.Descripcion)) : "Normal"

                               let ESTEREOPSISDx = varDx.Find(p => p.ServicioId == a.ServiceId).DetalleDxRecomendaciones.FindAll(o => o.IdComponente == Constants.VISION_ESTEREOSCOPICA_ID)
                               let ESTEREOPSIS_Con = ESTEREOPSISDx != null ? string.Join("/ ", ESTEREOPSISDx.Select(p => p.Descripcion)) : "Normal"

                               let OTROSOFTDx = varDx.Find(p => p.ServicioId == a.ServiceId).DetalleDxRecomendaciones.FindAll(o => o.IdComponente == Constants.EXPLORACIÓN_CLÍNICA_ID)
                               let OTROSOFT_Con = OTROSOFTDx != null ? string.Join("/ ", OTROSOFTDx.Select(p => p.Descripcion)) : "Normal"

                               let AUDIOMETDx = varDx.Find(p => p.ServicioId == a.ServiceId).DetalleDxRecomendaciones.FindAll(o => o.IdComponente == Constants.AUDIOMETRIA_ID)
                               let AUDIOMET_Con = AUDIOMETDx != null ? string.Join("/ ", AUDIOMETDx.Select(p => p.Descripcion)) : "Normal"
                               //Para obtener datos del User Control
                               let OftalmoValores = ValoresComponenteOdontogramaValue1(a.ServiceId, Constants.VISION_DE_COLORES_ID)

                               select new Matriz
                               {
                                   ServiceId = a.ServiceId,
                                   PersonId = a.PersonId,
                                   FechaNacimiento = a.FechaNacimiento,
                                   Proveedor = a.Proveedor,
                                   Colaborador = a.Colaborador,
                                   Dni = a.Dni,
                                   Edad = GetAge(a.FechaNacimiento.Value),
                                   Sexo = a.Sexo,
                                   FechaEvaluacion = a.FechaEvaluacion,
                                   EmpresaEmpleadora = a.EmpresaEmpleadora,
                                   Ruc = a.Ruc,
                                   TipoEvaluacion = a.TipoEvaluacion,
                                   PerfiExamenOcupacional = a.PerfiExamenOcupacional,
                                   PuestoTrabajo = a.PuestoTrabajo,
                                   Area = a.Area,
                                   Actividad = a.Actividad,
                                   AntecedentesPersonales = ObtenerMedicoPersonales(a.PersonId),
                                   Alergias = "NIEGA",

                                   DxEKG = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ELECTROCARDIOGRAMA_ID && o.IdCampo == Constants.ELECTROCARDIOGRAMA_DESCRIPCION_INTERPRETACION_ID) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ELECTROCARDIOGRAMA_ID && o.IdCampo == Constants.ELECTROCARDIOGRAMA_DESCRIPCION_INTERPRETACION_ID).Valor,

                                   PASistolica = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.FUNCIONES_VITALES_ID && o.IdCampo == Constants.FUNCIONES_VITALES_PAS_ID) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.FUNCIONES_VITALES_ID && o.IdCampo == Constants.FUNCIONES_VITALES_PAS_ID).Valor,
                                   PADiastolica = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.FUNCIONES_VITALES_ID && o.IdCampo == Constants.FUNCIONES_VITALES_PAD_ID) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.FUNCIONES_VITALES_ID && o.IdCampo == Constants.FUNCIONES_VITALES_PAD_ID).Valor,

                                   Peso = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ANTROPOMETRIA_ID && o.IdCampo == Constants.ANTROPOMETRIA_PESO_ID) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ANTROPOMETRIA_ID && o.IdCampo == Constants.ANTROPOMETRIA_PESO_ID).Valor,
                                   Talla = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ANTROPOMETRIA_ID && o.IdCampo == Constants.ANTROPOMETRIA_TALLA_ID) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ANTROPOMETRIA_ID && o.IdCampo == Constants.ANTROPOMETRIA_TALLA_ID).Valor,
                                   Imc = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ANTROPOMETRIA_ID && o.IdCampo == Constants.ANTROPOMETRIA_IMC_ID) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ANTROPOMETRIA_ID && o.IdCampo == Constants.ANTROPOMETRIA_IMC_ID).Valor,
                                   DXImc = IMC_Con,
                                   Hb = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.HEMOGRAMA_COMPLETO_ID && o.IdCampo == Constants.HEMOGRAMA_COMPLETO_HEMOGLOBINA) == null ? "N/R" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.HEMOGRAMA_COMPLETO_ID && o.IdCampo == Constants.HEMOGRAMA_COMPLETO_HEMOGLOBINA).Valor,

                                   GrupoSanguineo = Grupo,
                                   FactorSanguineo = Factor,
                                   Glicemia = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.GLUCOSA_ID && o.IdCampo == Constants.GLUCOSA_GLUCOSA_ID) == null ? "N/R" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.GLUCOSA_ID && o.IdCampo == Constants.GLUCOSA_GLUCOSA_ID).Valor,
                                   Colesterol = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.COLESTEROL_ID && o.IdCampo == Constants.COLESTEROL_COLESTEROL_TOTAL_ID) == null ? "N/R" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.COLESTEROL_ID && o.IdCampo == Constants.COLESTEROL_COLESTEROL_TOTAL_ID).Valor,
                                   Triglicerido = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.TRIGLICERIDOS_ID && o.IdCampo == Constants.TRIGLICERIDOS_BIOQUIMICA_TRIGLICERIDOS) == null ? "N/R" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.TRIGLICERIDOS_ID && o.IdCampo == Constants.TRIGLICERIDOS_BIOQUIMICA_TRIGLICERIDOS).Valor,

                                   ExOrina = "NORMAL N/A",
                                   Toxicologico = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID && o.IdCampo == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_MUESTRA) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID && o.IdCampo == Constants.COCAINA_MARIHUANA_TOXICOLOGICOS_MUESTRA).Valor,

                                   //DxEKG = EKG_Con,

                                   AguVisualLejosSinCorreOD = OftalmoValores.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OD) == null ? "" : OftalmoValores.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OD).v_Value1,
                                   AguVisualLejosSinCorreOI = OftalmoValores.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OI) == null ? "" : OftalmoValores.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OI).v_Value1,
                                   AguVisualLejosConCorreOD = OftalmoValores.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OD) == null ? "" : OftalmoValores.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_SC_OD).v_Value1,
                                   AguVisualLejosConCorreOI = OftalmoValores.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OI) == null ? "" : OftalmoValores.Find(p => p.v_ComponentFieldId == Sigesoft.Common.Constants.TXT_OFT_CC_OI).v_Value1,

                                   DxAgudezaVisual = VISUAL_Con,
                                   DxVisionColores = COLORES_Con,
                                   DxEsteropsis = ESTEREOPSIS_Con,
                                   DxOftalmicoOtros = OTROSOFT_Con,

                                   AudiometriaOD = AUDIOMET_Con,

                                   OtoscopiaOD = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.AUDIOMETRIA_ID && o.IdCampo == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_DERECHO) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.AUDIOMETRIA_ID && o.IdCampo == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_DERECHO).Valor,
                                   OtoscopiaOI = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.AUDIOMETRIA_ID && o.IdCampo == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_IZQUIERDO) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.AUDIOMETRIA_ID && o.IdCampo == Constants.AUDIOMETRIA_OTOSCOPIA_OIDO_IZQUIERDO).Valor,

                                   DxEspirometrico = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ESPIROMETRIA_ID && o.IdCampo == Constants.ESPIROMETRIA_CONCLUSION_2017) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ESPIROMETRIA_ID && o.IdCampo == Constants.ESPIROMETRIA_CONCLUSION_2017).Valor,

                                   DxRadiografiaTorax = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.RX_ID && o.IdCampo == Constants.RX_CONCLUSIONES_RADIOGRAFICAS_DESCRIPCION_ID) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.RX_ID && o.IdCampo == Constants.RX_CONCLUSIONES_RADIOGRAFICAS_DESCRIPCION_ID).Valor,

                                   DxPsicologia = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.PSICOLOGIA_ID_2017 && o.IdCampo == Constants.PSICOLOGIA_CELIMA_CONCLUSIONES) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.PSICOLOGIA_ID_2017 && o.IdCampo == Constants.PSICOLOGIA_CELIMA_CONCLUSIONES).Valor,

                                   TestAlturaMayor18Mts = varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ALTURA_FISICA_M_18 && o.IdCampo == Constants.ALTURA_FISICA_M_18_APTO_PARA_ALTURA_MTS18) == null ? "NO APLICA" : varValores.Find(p => p.ServicioId == a.ServiceId).CampoValores.Find(o => o.IdComponente == Constants.ALTURA_FISICA_M_18 && o.IdCampo == Constants.ALTURA_FISICA_M_18_APTO_PARA_ALTURA_MTS18).Valor=="1"?"SI":"NO",

                                   Recomendaciones = ConcatenateRecommendationByService(a.ServiceId),
                                   Restricciones = ConcatenateRestrictionByService(a.ServiceId),
                                   Aptitud = a.Aptitud

                               }
                                   ).ToList();
                    return sql;
                }



            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ServiceComponentFieldValuesList> ValoresComponenteOdontogramaValue1(string pstrServiceId, string pstrComponentId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            try
            {
                List<ServiceComponentFieldValuesList> serviceComponentFieldValues = (from A in dbContext.service
                                                                                     join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                                                                                     join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                                                                                     join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId

                                                                                     where (A.v_ServiceId == pstrServiceId)
                                                                                           && (B.v_ComponentId == pstrComponentId)
                                                                                           //&& (B.i_IsDeleted == 0)
                                                                                           //&& (C.i_IsDeleted == 0)

                                                                                     select new ServiceComponentFieldValuesList
                                                                                     {
                                                                                         //v_ComponentId = B.v_ComponentId,
                                                                                         v_ComponentFieldId = C.v_ComponentFieldId,
                                                                                         //v_ComponentFieldId = G.v_ComponentFieldId,
                                                                                         //v_ComponentFielName = G.v_TextLabel,
                                                                                         v_ServiceComponentFieldsId = C.v_ServiceComponentFieldsId,
                                                                                         v_Value1 = D.v_Value1
                                                                                     }).ToList();


                return serviceComponentFieldValues;
            }
            catch (Exception)
            {

                throw;
            }

        }


        private static string DEvolver237string(string pstrIdParameter)
        {
            if (pstrIdParameter == "1")
            {
                return "20/20";

            }
            else if (pstrIdParameter == "2")
            {
                return "20/25";

            }
            else if (pstrIdParameter == "3")
            {
                return "20/30";

            }
            else if (pstrIdParameter == "4")
            {
                return "20/40";

            }
            else if (pstrIdParameter == "5")
            {
                return "20/50";

            }
            else if (pstrIdParameter == "6")
            {
                return "20/60";

            }
            else if (pstrIdParameter == "7")
            {
                return "20/80";

            }
            else if (pstrIdParameter == "8")
            {
                return "20/100";

            }
            else if (pstrIdParameter == "9")
            {
                return "20/140";

            }
            else if (pstrIdParameter == "10")
            {
                return "20/200";

            }
            else if (pstrIdParameter == "11")
            {
                return "20/400";

            }
            else if (pstrIdParameter == "12")
            {
                return "CD 3M";

            }
            else if (pstrIdParameter == "13")
            {
                return "CD 1M";

            }
            else if (pstrIdParameter == "14")
            {
                return "CD 0.3M";

            }
            else if (pstrIdParameter == "15")
            {
                return "MM";

            }
            else if (pstrIdParameter == "16")
            {
                return "PL";

            }
            else if (pstrIdParameter == "17")
            {
                return "NPL";

            }
            else if (pstrIdParameter == "18")
            {
                return "---";
            }
            else
            { return ""; }
        }
        private static string DEvolver262string(string pstrIdParameter)
        {
            if (pstrIdParameter == "1")
            {
                return "20/20";
            }
            else if (pstrIdParameter == "2")
            {
                return "20/30";

            }
            else if (pstrIdParameter == "3")
            {
                return "20/40";

            }
            else if (pstrIdParameter == "4")
            {
                return "20/50";

            }
            else if (pstrIdParameter == "5")
            {
                return "20/60";

            }
            else if (pstrIdParameter == "6")
            {
                return "20/70";

            }
            else if (pstrIdParameter == "7")
            {
                return "20/80";

            }
            else if (pstrIdParameter == "8")
            {
                return "20/100";

            }
            else if (pstrIdParameter == "9")
            {
                return "20/160";

            }
            else if (pstrIdParameter == "10")
            {
                return "20/200";

            }

            else
            { return ""; }
        }
    }
}
