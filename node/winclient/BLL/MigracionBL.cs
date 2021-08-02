using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BLL
{
    public class MigracionBL
    {
        //public List<OrganizationList> DevolverDatosEmpresaOLD()
        //{
        //    //mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.organization_old                          
        //                    where A.i_IsDeleted == 0
        //                    select new OrganizationList
        //                    {      
        //                        v_OrganizationId = A.v_OrganizationId,
        //                        i_OrganizationTypeId = (int)A.i_OrganizationTypeId,
        //                        i_SectorTypeId = (int)A.i_SectorTypeId,
        //                        v_SectorName = A.v_SectorName,
        //                        v_SectorCodigo = A.v_SectorCodigo,
        //                        v_IdentificationNumber = A.v_IdentificationNumber,
        //                        v_Name = A.v_Name,
        //                        v_Address = A.v_Address,
        //                        v_PhoneNumber = A.v_PhoneNumber,
        //                        v_Mail =A.v_Mail,
        //                        v_ContacName = A.v_ContacName,
        //                        v_Contacto = A.v_Contacto,
        //                        v_EmailContacto = A.v_EmailContacto,
        //                        v_Observation = A.v_Observation,
        //                        i_NumberQuotasOrganization = A.i_NumberQuotasOrganization,
        //                        i_NumberQuotasMen = A.i_NumberQuotasMen,
        //                        i_DepartmentId = A.i_DepartmentId.Value,
        //                        i_ProvinceId = A.i_ProvinceId.Value,
        //                        i_DistrictId = A.i_DistrictId.Value,
        //                        i_IsDeleted = A.i_IsDeleted.Value,
        //                        i_InsertUserId = A.i_InsertUserId.Value,
        //                        d_InsertDate = A.d_InsertDate,
        //                        i_UpdateUserId = A.i_UpdateUserId.Value,
        //                        d_UpdateDate = A.d_UpdateDate,
        //                        b_Image = A.b_Image,
        //                        v_ContactoMedico = A.v_ContactoMedico,
        //                        v_EmailMedico = A.v_EmailMedico                           
        //                    };                              

        //        List<OrganizationList> objData = query.ToList();

        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public bool VerificarSiExisteEmpresaAntigua(string psrtRUC)
        //{
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = (from A in dbContext.organization
        //                     where A.i_IsDeleted == 0 && A.v_IdentificationNumber == psrtRUC
        //                     select new OrganizationList
        //                     {
        //                         v_IdentificationNumber = A.v_IdentificationNumber
        //                     }).FirstOrDefault();

        //        if (query != null)
        //         {
        //             return true;
        //         }
        //         else
        //         {
        //             return false;
        //         }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public EmpresaMigracionList VerificarSiExisteEmpresaAntigua_(string psrtRUC)
        //{
        //    try
        //    {
        //        EmpresaMigracionList oEmpresaMigracionList = new EmpresaMigracionList();
        //        LocationMigracionList oLocationMigracionList = new LocationMigracionList();

        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var qEmpresa = (from A in dbContext.organization
        //                     where A.i_IsDeleted == 0 && A.v_IdentificationNumber == psrtRUC
        //                     select new EmpresaMigracionList
        //                     {
        //                         v_OrganizationId = A.v_OrganizationId,
        //                         v_Name = A.v_Name,
        //                         v_IdentificationNumber = A.v_IdentificationNumber                          

        //                     }).FirstOrDefault();


        //        var qSedes = (from A in dbContext.location
        //                      where A.i_IsDeleted == 0 && A.v_OrganizationId == qEmpresa.v_OrganizationId
        //                      select new LocationMigracionList
        //                        {
        //                            v_LocationId = A.v_LocationId,
        //                            v_OrganizationId = A.v_OrganizationId,
        //                            v_Name = A.v_Name
        //                        }).ToList();

        //        var x = qSedes[0].v_LocationId;

        //        var qGESOS= (from A in dbContext.groupoccupation
        //                     where A.i_IsDeleted == 0 && A.v_LocationId == x
        //                     select new groupoccupationMigracionList
        //                      {
        //                          v_GroupOccupationId = A.v_GroupOccupationId,
        //                          v_LocationId = A.v_LocationId,                                 
        //                          v_Name = A.v_Name
        //                      }).ToList();

        //        oEmpresaMigracionList.v_OrganizationId = qEmpresa.v_OrganizationId;
        //        oEmpresaMigracionList.v_Name = qEmpresa.v_Name;
        //        oEmpresaMigracionList.v_IdentificationNumber = qEmpresa.v_IdentificationNumber;
        //        oEmpresaMigracionList.Sedes = qSedes;
        //        oEmpresaMigracionList.GESOS = qGESOS;

        //        return oEmpresaMigracionList;

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<LocationList> DevolverSedesAntiguasPorIdEmpresa(string pstrEmpresaId)
        //{
        //    //mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.location_old                           
        //                    where A.i_IsDeleted == 0 && A.v_OrganizationId == pstrEmpresaId
        //                    select new LocationList
        //                    {
        //                        v_LocationId = A.v_LocationId,
        //                        v_OrganizationId = A.v_OrganizationId,
        //                        v_Name = A.v_Name,
        //                        i_IsDeleted = A.i_IsDeleted,
        //                        i_InsertUserId = A.i_InsertUserId,
        //                        d_InsertDate = A.d_InsertDate,
        //                        i_UpdateUserId = A.i_UpdateUserId,
        //                        d_UpdateDate = A.d_UpdateDate
        //                    };

        //        List<LocationList> objData = query.ToList();

        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public List<GroupOccupationList> DevolverGESOporLocationId(string pstrLocationId)
        //{
        //    //mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.groupoccupation_old
        //                    where A.i_IsDeleted == 0 && A.v_LocationId == pstrLocationId
        //                    select new GroupOccupationList
        //                    {
        //                        v_LocationId = A.v_LocationId,
        //                        v_Name = A.v_Name,
        //                        i_IsDeleted = A.i_IsDeleted.Value,
        //                        i_InsertUserId = A.i_InsertUserId.Value,
        //                        d_InsertDate = A.d_InsertDate,
        //                        i_UpdateUserId = A.i_UpdateUserId.Value,
        //                        d_UpdateDate = A.d_UpdateDate
        //                    };

        //        List<GroupOccupationList> objData = query.ToList();

        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public List<PacientList> DevolverDatosPacientLD()
        //{
        //    //mon.IsActive = true;
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.pacient_old
        //                    join B in dbContext.person_old on A.v_PersonId equals B.v_PersonId
        //                    where A.i_IsDeleted == 0
        //                    select new PacientList
        //                    {
        //                        v_PersonId = A.v_PersonId,
        //                        i_IsDeleted = A.i_IsDeleted.Value,
        //                        d_UpdateDate = A.d_UpdateDate,
        //                        i_UpdateNodeId = A.i_UpdateNodeId.Value,
        //                        v_DocNumber = B.v_DocNumber,
        //                        v_FirstName = B.v_FirstName,
        //                        v_FirstLastName = B.v_FirstLastName,
        //                        v_SecondLastName = B.v_SecondLastName,
        //                        i_DocTypeId = B.i_DocTypeId.Value,

        //                        d_Birthdate = B.d_Birthdate ,
        //                         v_BirthPlace = B.v_BirthPlace      ,
        //                        i_SexTypeId = B.i_SexTypeId.Value,
        //                        i_MaritalStatusId = B.i_MaritalStatusId.Value,
        //                        i_LevelOfId = B.i_LevelOfId.Value,

        //                        v_TelephoneNumber = B.v_TelephoneNumber      ,
        //                        v_AdressLocation = B.v_AdressLocation      ,
        //                        v_GeografyLocationId = B.v_GeografyLocationId      ,
        //                        v_ContactName = B.v_ContactName      ,
        //                        v_EmergencyPhone = B.v_EmergencyPhone      ,
        //                        b_PersonImage = B.b_PersonImage      ,
        //                        v_Mail = B.v_Mail,
        //                        i_BloodGroupId = B.i_BloodGroupId.Value      ,
        //                        i_BloodFactorId = B.i_BloodFactorId.Value,
        //                        b_FingerPrintTemplate = B.b_FingerPrintTemplate      ,
        //                        b_RubricImage = B.b_RubricImage      ,
        //                        b_FingerPrintImage = B.b_FingerPrintImage      ,
        //                        t_RubricImageText = B.t_RubricImageText      ,
        //                        v_CurrentOccupation = B.v_CurrentOccupation      ,
        //                        i_DepartmentId = B.i_DepartmentId.Value,
        //                        i_ProvinceId = B.i_ProvinceId.Value,
        //                        i_DistrictId = B.i_DistrictId.Value,
        //                        i_ResidenceInWorkplaceId = B.i_ResidenceInWorkplaceId.Value,
        //                        v_ResidenceTimeInWorkplace = B.v_ResidenceTimeInWorkplace      ,
        //                        i_TypeOfInsuranceId = B.i_TypeOfInsuranceId.Value,
        //                        i_NumberLivingChildren = B.i_NumberLivingChildren.Value,
        //                        i_NumberDependentChildren = B.i_NumberDependentChildren.Value,
        //                        i_OccupationTypeId = B.i_OccupationTypeId.Value,
        //                        v_OwnerName = B.v_OwnerName      ,
        //                        i_NumberLiveChildren = B.i_NumberLiveChildren.Value,
        //                        i_NumberDeadChildren = B.i_NumberDeadChildren.Value,

        //                        i_InsertUserId = B.i_InsertUserId.Value,
        //                        d_InsertDate = B.d_InsertDate.Value,
        //                        i_UpdateUserId = B.i_UpdateUserId.Value,

        //                        i_InsertNodeId = B.i_InsertNodeId.Value,

        //                        i_Relationship = B.i_Relationship.Value      ,
        //                        v_ExploitedMineral = B.v_ExploitedMineral      ,
        //                        i_AltitudeWorkId = B.i_AltitudeWorkId.Value,
        //                        i_PlaceWorkId = B.i_PlaceWorkId.Value,
        //                        v_NroPoliza = B.v_NroPoliza      ,
        //                        v_Deducible = B.v_Deducible.Value      ,
        //                        i_NroHermanos = B.i_NroHermanos.Value,
        //                        v_Password = B.v_Password

        //                    };

        //        List<PacientList> objData = query.ToList();

        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}


        //#region Historia
        //public List<HistoryList> ObtenerLisatHistoriaPorPersonId(string pstrPersonId)
        //{
        //    //mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.history_old
        //                    where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId

        //                    select new HistoryList
        //                    {
        //                        v_HistoryId = A.v_HistoryId,
        //                        d_StartDate = A.d_StartDate,
        //                        d_EndDate = A.d_EndDate,
        //                        v_Organization = A.v_Organization,
        //                        v_TypeActivity = A.v_TypeActivity,
        //                        i_GeografixcaHeight = A.i_GeografixcaHeight,
        //                        v_workstation = A.v_workstation,
        //                        b_RubricImage = A.b_RubricImage,
        //                        b_FingerPrintImage = A.b_FingerPrintImage,
        //                        t_RubricImageText = A.t_RubricImageText,
        //                        i_TypeOperationId = A.i_TypeOperationId.Value
        //                    };

        //        List<HistoryList> objData = query.ToList();


        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public List<WorkstationDangersList> ObtenerListaPeligrosPorHistoryId(string pstrHistorytId)
        //{
        //    //mon.IsActive = true;
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.workstationdangers_old
        //                    where A.i_IsDeleted == 0 && A.v_HistoryId == pstrHistorytId

        //                    select new WorkstationDangersList
        //                    {
        //                        v_WorkstationDangersId = A.v_WorkstationDangersId,
        //                        i_DangerId = A.i_DangerId,
        //                        i_NoiseSource = A.i_NoiseSource,
        //                        i_NoiseLevel = A.i_NoiseLevel,
        //                        v_TimeOfExposureToNoise = A.v_TimeOfExposureToNoise
        //                    };

        //        List<WorkstationDangersList> objData = query.ToList();
        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public List<TypeOfEEPList> ObtenerListaEPPSPorHistoryId(string pstrHistorytId)
        //{
        //    //mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.typeofeep
        //                    where A.i_IsDeleted == 0 && A.v_HistoryId == pstrHistorytId

        //                    select new TypeOfEEPList
        //                    {
        //                        i_TypeofEEPId = A.i_TypeofEEPId,
        //                        r_Percentage = A.r_Percentage

        //                    };

        //        List<TypeOfEEPList> objData = query.ToList();

        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public List<PersonMedicalHistoryList> DevolverListaMedicoPersonalesPorPersonId(string pstrPersonId)
        //{
        //    //mon.IsActive = true;
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.personmedicalhistory_old     
        //                    join B in dbContext.diseases_old on A.v_DiseasesId equals B.v_DiseasesId
        //                    where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId 
        //                    select new PersonMedicalHistoryList
        //                    {
        //                        v_PersonMedicalHistoryId = A.v_PersonMedicalHistoryId,
        //                        v_PersonId = A.v_PersonId,
        //                        v_DiseasesId = A.v_DiseasesId,
        //                        i_TypeDiagnosticId = A.i_TypeDiagnosticId,
        //                        d_StartDate = A.d_StartDate.Value,
        //                        v_TreatmentSite = A.v_TreatmentSite,                               
        //                        v_DiagnosticDetail = A.v_DiagnosticDetail,                              
        //                        i_Answer = A.i_AnswerId.Value,
        //                        v_CIE10Id = B.v_CIE10Id,
        //                        v_Name =  B.v_Name
        //                    };

        //        List<PersonMedicalHistoryList> objData = query.ToList();

        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {               
        //        return null;
        //    }
        //}

        //public List<FamilyMedicalAntecedentsList> DevolverListaMedicoFamiliaresPorPersonId(string pstrPersonId)
        //{
        //    //mon.IsActive = true;
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.familymedicalantecedents_old
        //                    join B in dbContext.diseases_old on A.v_DiseasesId equals B.v_DiseasesId
        //                    where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId

        //                    select new FamilyMedicalAntecedentsList
        //                    {
        //                        v_FamilyMedicalAntecedentsId = A.v_FamilyMedicalAntecedentsId,
        //                        v_PersonId = A.v_PersonId,
        //                        v_DiseasesId = A.v_DiseasesId,
        //                        i_TypeFamilyId = A.i_TypeFamilyId.Value,                                
        //                        v_Comment = A.v_Comment,
        //                        v_CIE10Id = B.v_CIE10Id,
        //                        v_Name = B.v_Name
        //                    };

        //        List<FamilyMedicalAntecedentsList> objData = query.ToList();

        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {              
        //        return null;
        //    }
        //}

        //public List<NoxiousHabitsList> DevolverListaHabitosPorPersonId(string pstrPersonId)
        //{
        //    //mon.IsActive = true;
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = from A in dbContext.noxioushabits_old                          
        //                    where A.i_IsDeleted == 0 && A.v_PersonId == pstrPersonId

        //                    select new NoxiousHabitsList
        //                    {
        //                        v_NoxiousHabitsId = A.v_NoxiousHabitsId,                               
        //                        v_PersonId = A.v_PersonId,
        //                        v_Frequency = A.v_Frequency,
        //                        v_Comment = A.v_Comment                              
        //                    };


        //        List<NoxiousHabitsList> objData = query.ToList();

        //        return objData;

        //    }
        //    catch (Exception ex)
        //    {             
        //        return null;
        //    }
        //}



        //#endregion

        //public string ValidarDiseaseSiExiste(string pstrCie10Id, string pstrName, List<string> ClientSession)
        //{
        //      OperationResult _objOperationResult = new OperationResult();
        //    SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //    var objEntity = (from a in dbContext.diseases
        //                     where a.v_CIE10Id == pstrCie10Id && a.v_Name == pstrName
        //                     select a).FirstOrDefault();

        //    if (objEntity != null)
        //    {
        //        return objEntity.v_DiseasesId;
        //    }
        //    else
        //    {
        //        diseasesDto odiseasesDto = new diseasesDto();
        //        odiseasesDto.v_CIE10Id = pstrCie10Id;
        //        odiseasesDto.v_Name = pstrName;
        //        var DiseaseId =  new MedicalExamFieldValuesBL().AddDiseases(ref _objOperationResult, odiseasesDto, ClientSession);

        //        return DiseaseId;
        //    }

        //}

        //#region Service
        //public List<CalendarList> DevolverListaCalendarOLD(string pstrServiceOLD)
        //{
        //      SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //      var query = (from A in dbContext.calendar_old
        //                   where A.i_IsDeleted == 0 && A.v_ServiceId == pstrServiceOLD
        //                   select new CalendarList
        //                   {
        //                       v_PersonId = A.v_PersonId,
        //                       v_ProtocolId = A.v_ProtocolId,
        //                       v_ServiceId = A.v_ServiceId,
        //                        d_CircuitStartDate = A.d_CircuitStartDate.Value,
        //                        d_DateTimeCalendar = A.d_DateTimeCalendar.Value,
        //                        d_EntryTimeCM = A.d_EntryTimeCM.Value,
        //                        d_SalidaCM = A.d_SalidaCM.Value,
        //                        i_CalendarStatusId = A.i_CalendarStatusId.Value,
        //                        i_IsVipId = A.i_IsVipId.Value,
        //                        i_LineStatusId = A.i_LineStatusId.Value,
        //                        i_NewContinuationId = A.i_NewContinuationId.Value,
        //                        i_ServiceId = A.i_ServiceId.Value,
        //                        i_ServiceTypeId = A.i_ServiceTypeId.Value,

        //                   }).ToList();


        //      return query;
        //}

        //public List<ServiceList> DevolverListaServiciosOLD()
        //{
        //    //mon.IsActive = true;

        //    DateTime fechaInicio =  DateTime.Parse("2015-12-20");
        //    DateTime fechaIFin = DateTime.Parse("2015-12-31");
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        var query = (from A in dbContext.service_old
        //                     join B in dbContext.person_old on A.v_PersonId     equals B.v_PersonId
        //                     where A.i_IsDeleted == 0 && A.d_ServiceDate > fechaInicio && A.d_ServiceDate < fechaIFin
        //                     select new ServiceList {        
        //                            v_ServiceId = A.v_ServiceId,
        //                            v_ProtocolId = A.v_ProtocolId,
        //                            v_DocNumber = B.v_DocNumber,
        //                            d_EndDateRestriction = A.d_EndDateRestriction,
        //                            d_FechaEntrega = A.d_FechaEntrega,
        //                            d_Fur = A.d_Fur,
        //                            d_GlobalExpirationDate = A.d_GlobalExpirationDate,                                 
        //                            d_InsertDateMedicalAnalyst = A.d_InsertDateMedicalAnalyst,
        //                            d_InsertDateOccupationalMedical = A.d_InsertDateOccupationalMedical,
        //                            d_Mamografia = A.d_Mamografia,
        //                            d_MedicalBreakEndDate = A.d_MedicalBreakEndDate,
        //                            d_MedicalBreakStartDate = A.d_MedicalBreakStartDate,
        //                            d_NextAppointment = A.d_NextAppointment,
        //                            d_ObsExpirationDate = A.d_ObsExpirationDate,
        //                            d_PAP = A.d_PAP,
        //                            d_ServiceDate = A.d_ServiceDate,
        //                            d_StartDateRestriction = A.d_StartDateRestriction,
        //                            d_UpdateDate = A.d_UpdateDate,
        //                            d_UpdateDateMedicalAnalyst = A.d_UpdateDateMedicalAnalyst,
        //                            d_UpdateDateOccupationalMedical = A.d_UpdateDateOccupationalMedical,
        //                            i_AppetiteId = A.i_AppetiteId,
        //                            i_AptitudeStatusId = A.i_AptitudeStatusId,
        //                            i_CursoEnf = A.i_CursoEnf,
        //                            i_DepositionId = A.i_DepositionId,
        //                            i_DestinationMedicationId = A.i_DestinationMedicationId,
        //                            i_DreamId = A.i_DreamId,
        //                            i_Evolucion = A.i_Evolucion,
        //                            i_FlagAgentId = A.i_FlagAgentId,
        //                            i_HasMedicalBreakId = A.i_HasMedicalBreakId,
        //                            i_HasRestrictionId = A.i_HasRestrictionId,
        //                            i_HasSymptomId = A.i_HasSymptomId,
        //                            i_HazInterconsultationId = A.i_HazInterconsultationId,
        //                            i_InicioEnf = A.i_InicioEnf,
        //                            i_InsertUserMedicalAnalystId = A.i_InsertUserMedicalAnalystId,
        //                            i_InsertUserOccupationalMedicalId = A.i_InsertUserOccupationalMedicalId,
        //                            i_IsDeleted = A.i_IsDeleted,
        //                            i_IsFac = A.i_IsFac,
        //                            i_IsNewControl = A.i_IsNewControl,
        //                            i_MacId = A.i_MacId,
        //                            i_MasterServiceId = A.i_MasterServiceId,
        //                            i_ModalityOfInsurance = A.i_ModalityOfInsurance,
        //                            i_SendToTracking = A.i_SendToTracking,
        //                            i_ServiceStatusId = A.i_ServiceStatusId,
        //                            i_ServiceTypeOfInsurance = A.i_ServiceTypeOfInsurance,
        //                            i_StatusLiquidation = A.i_StatusLiquidation,
        //                            i_ThirstId = A.i_ThirstId,
        //                            i_TimeOfDisease = A.i_TimeOfDisease,
        //                            i_TimeOfDiseaseTypeId = A.i_TimeOfDiseaseTypeId,
        //                            i_TransportMedicationId = A.i_TransportMedicationId,                                   
        //                            i_UpdateUserMedicalAnalystId = A.i_UpdateUserMedicalAnalystId,
        //                            i_UpdateUserOccupationalMedicaltId = A.i_UpdateUserOccupationalMedicaltId,
        //                            i_UrineId = A.i_UrineId,
        //                            //r_Costo = A.r_Costo.Value,
        //                            v_AreaId = A.v_AreaId,
        //                            v_CatemenialRegime = A.v_CatemenialRegime,
        //                            v_CiruGine = A.v_CiruGine,
        //                            v_ExaAuxResult = A.v_ExaAuxResult,
        //                            v_FechaUltimaMamo = A.v_FechaUltimaMamo,
        //                            v_FechaUltimoPAP = A.v_FechaUltimoPAP,
        //                            v_Findings = A.v_Findings,
        //                            v_GeneralRecomendations = A.v_GeneralRecomendations,
        //                            v_Gestapara = A.v_Gestapara,
        //                            v_LocationId = A.v_LocationId,
        //                            v_MainSymptom = A.v_MainSymptom,
        //                            v_Menarquia = A.v_Menarquia,
        //                            v_Motive = A.v_Motive,
        //                            v_ObsStatusService = A.v_ObsStatusService,
        //                            v_OrganizationId = A.v_OrganizationId,
        //                            v_PersonId = A.v_PersonId,
        //                            v_ResultadoMamo = A.v_ResultadoMamo,
        //                            v_ResultadosPAP = A.v_ResultadosPAP,
        //                            v_Story = A.v_Story

        //                     }).ToList();

        //        return query;

        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public location_oldDto GetLocation_OLD(ref OperationResult pobjOperationResult, string pstrEmpresaIdOLD)
        //{
        //    //mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        location_oldDto objDtoEntity = null;

        //        var objEntity = (from a in dbContext.location_old
        //                         where a.v_OrganizationId == pstrEmpresaIdOLD
        //                         select a).FirstOrDefault();

        //        if (objEntity != null)
        //            objDtoEntity = location_oldAssembler.ToDTO(objEntity);

        //        pobjOperationResult.Success = 1;
        //        return objDtoEntity;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
        //        return null;
        //    }
        //}

        //public groupoccupation_oldDto GetGroupOccupation_OLD(ref OperationResult pobjOperationResult, string pstrLocationIdOLD)
        //{
        //    //mon.IsActive = true;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        groupoccupation_oldDto objDtoEntity = null;

        //        var objEntity = (from a in dbContext.groupoccupation_old
        //                         where a.v_LocationId == pstrLocationIdOLD
        //                         select a).FirstOrDefault();

        //        if (objEntity != null)
        //            objDtoEntity = groupoccupation_oldAssembler.ToDTO(objEntity);

        //        pobjOperationResult.Success = 1;
        //        return objDtoEntity;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
        //        return null;
        //    }
        //}

        //public string DevolverProtocoloOLD(string pstrProtocolOLDId, List<string> ClientSession)
        //{
        //    OperationResult _objOperationResult = new OperationResult();
        //    string ProtocolId = "";
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

        //        //obtner datos del protocolo antiguo para buscar en la nueva bd
        //        var objEntityOLD = (from a in dbContext.protocol_old
        //                         where a.v_ProtocolId == pstrProtocolOLDId
        //                         select a).FirstOrDefault();

        //        //buscar el protocolo antiguo en la nueva BD por su nombre
        //        var objEntityNuevo = (from a in dbContext.protocol
        //                              where a.v_Name == objEntityOLD.v_Name.Trim()
        //                         select a).FirstOrDefault();

        //        if (objEntityNuevo == null)// si no lo encuentra crear un nuevo protocolo con detalle y devolver el Id
        //        {
        //            ProtocolBL oProtocolBL = new ProtocolBL();
        //            protocolDto oprotocolDto = new protocolDto();                  
        //            List<protocolcomponentDto> oListaprotocolcomponentDto = new List<protocolcomponentDto>();
        //            protocolcomponentDto oprotocolcomponentDto;

        //            oprotocolDto.v_Name = objEntityOLD.v_Name.ToString().Trim();

        //            var JerarquiaEmpresaEmpleado = DevolverEmpresaIDNuevo(objEntityOLD.v_EmployerOrganizationId);
        //            oprotocolDto.v_EmployerOrganizationId = JerarquiaEmpresaEmpleado.v_OrganizationId;

        //            var LocationEmployerOLD = GetLocation_OLD(ref _objOperationResult, objEntityOLD.v_EmployerOrganizationId);
        //            var aa = JerarquiaEmpresaEmpleado.Sedes.Find(p => p.v_Name.ToString().Trim() == LocationEmployerOLD.v_Name.ToString().Trim());
        //            var v_EmployerLocationId_Nuevo = aa.v_LocationId;
        //            oprotocolDto.v_EmployerLocationId = v_EmployerLocationId_Nuevo;

        //            oprotocolDto.i_EsoTypeId = objEntityOLD.i_EsoTypeId;

        //            var JerarquiaEmpresaCliente = DevolverEmpresaIDNuevo(objEntityOLD.v_CustomerOrganizationId);
        //            oprotocolDto.v_CustomerOrganizationId = JerarquiaEmpresaCliente.v_OrganizationId;

        //            var LocationClienteOLD = GetLocation_OLD(ref _objOperationResult, objEntityOLD.v_CustomerOrganizationId);
        //            var v_CustomerLocationIdCliente_Nuevo = JerarquiaEmpresaCliente.Sedes.Find(p => p.v_Name.ToString().Trim() == LocationClienteOLD.v_Name.ToString().Trim()).v_LocationId;
        //           oprotocolDto.v_CustomerLocationId = v_CustomerLocationIdCliente_Nuevo;



        //            oprotocolDto.v_NombreVendedor = objEntityOLD.v_NombreVendedor;
        //            var JerarquiaEmpresaTrabajo = DevolverEmpresaIDNuevo(objEntityOLD.v_WorkingOrganizationId);
        //            oprotocolDto.v_WorkingOrganizationId = JerarquiaEmpresaTrabajo.v_OrganizationId;

        //            var LocationTrabajoOLD = GetLocation_OLD(ref _objOperationResult, objEntityOLD.v_WorkingOrganizationId);
        //            var v_LocationIdTrabajo_Nuevo = JerarquiaEmpresaTrabajo.Sedes.Find(p => p.v_Name.ToString().Trim() == LocationTrabajoOLD.v_Name.ToString().Trim()).v_LocationId;
        //            oprotocolDto.v_WorkingLocationId = v_LocationIdTrabajo_Nuevo;


        //            var v_GroupOccupationOLD = GetGroupOccupation_OLD(ref _objOperationResult, objEntityOLD.v_EmployerLocationId);
        //            var v_GroupOccupationId_Nuevo = JerarquiaEmpresaEmpleado.GESOS.Find(p => p.v_Name.ToString().Trim() == v_GroupOccupationOLD.v_Name.ToString().Trim()).v_GroupOccupationId;
        //            oprotocolDto.v_GroupOccupationId = v_GroupOccupationId_Nuevo;



        //            oprotocolDto.v_CostCenter = objEntityOLD.v_CostCenter;
        //            oprotocolDto.i_MasterServiceTypeId = objEntityOLD.i_MasterServiceTypeId;
        //            oprotocolDto.i_MasterServiceId = objEntityOLD.i_MasterServiceId;
        //            oprotocolDto.i_HasVigency = objEntityOLD.i_HasVigency;
        //            oprotocolDto.i_ValidInDays = objEntityOLD.i_ValidInDays;
        //            oprotocolDto.i_IsActive = objEntityOLD.i_IsActive;

        //            //Obtener Detalle protocolo

        //         var ListaProtocolComponentOLD =  DevolverProtocomponentOLD(objEntityOLD.v_ProtocolId);

        //         foreach (var item in ListaProtocolComponentOLD)
        //         {
        //             oprotocolcomponentDto = new protocolcomponentDto();
        //             oprotocolcomponentDto.v_ComponentId = item.v_ComponentId;
        //             oprotocolcomponentDto.r_Price = item.r_Price;
        //             oprotocolcomponentDto.i_OperatorId = item.i_OperatorId;
        //             oprotocolcomponentDto.i_Age = item.i_Age;
        //             oprotocolcomponentDto.i_GenderId = item.i_GenderId;

        //             oprotocolcomponentDto.i_IsConditionalId = item.i_IsConditionalId;
        //             oprotocolcomponentDto.i_IsConditionalIMC = item.i_IsConditionalIMC;
        //             oprotocolcomponentDto.r_Imc = item.r_Imc;
        //             oprotocolcomponentDto.i_IsAdditional = item.i_isAdditional;
        //             oListaprotocolcomponentDto.Add(oprotocolcomponentDto);

        //         }       

        //            ProtocolId = oProtocolBL.AddProtocol(ref _objOperationResult, oprotocolDto, oListaprotocolcomponentDto, ClientSession);


        //        }
        //        else// devolver el nuevo id del protocolo
        //        {
        //            ProtocolId = objEntityNuevo.v_ProtocolId;
        //        }


        //        return ProtocolId;
        //    }
        //    catch (Exception ex)
        //    {               
        //        return null;
        //    }
        //}

        //public EmpresaMigracionList DevolverEmpresaIDNuevo(string pstrEmpresaIdOLD)
        //{
        //    SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //    var query = (from A in dbContext.organization_old
        //                 where A.i_IsDeleted == 0 && A.v_OrganizationId == pstrEmpresaIdOLD
        //                 select new OrganizationList
        //                 {
        //                     v_IdentificationNumber = A.v_IdentificationNumber
        //                 }).FirstOrDefault();

        //    return (VerificarSiExisteEmpresaAntigua_(query.v_IdentificationNumber));           

        //}

        //public List<ProtocolComponentList> DevolverProtocomponentOLD(string pstrProtocolId_OLD)
        //{
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

        //        var objEntity = (from a in dbContext.protocolcomponent_old

        //                         where a.v_ProtocolId == pstrProtocolId_OLD
        //                         && a.i_IsDeleted == 0

        //                         select new ProtocolComponentList
        //                         {
        //                             v_ProtocolComponentId =a.v_ProtocolComponentId,
        //                             v_ProtocolId =a.v_ProtocolId,
        //                             v_ComponentId = a.v_ComponentId,                    
        //                             r_Price = a.r_Price,
        //                             i_OperatorId = a.i_OperatorId,
        //                             i_Age = a.i_Age,
        //                             i_GenderId = a.i_GenderId,
        //                             i_IsConditionalId = a.i_IsConditionalId,
        //                             i_IsConditionalIMC = a.i_IsConditionalIMC,
        //                             r_Imc = a.r_Imc,
        //                             i_isAdditional = a.i_IsAdditional                                   
        //                         }).ToList();


        //        return objEntity;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public List<ServiceComponentList> GetServiceComponents_OLD(string pstrServiceId_OLD)
        //{
        //    int isDeleted = (int)SiNo.NO;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

        //        var query = (from A in dbContext.servicecomponent_old
        //                     where A.v_ServiceId == pstrServiceId_OLD &&
        //                           A.i_IsDeleted == isDeleted

        //                     select new ServiceComponentList
        //                     {
        //                         v_ComponentId = A.v_ComponentId,                                
        //                         i_ServiceComponentStatusId = A.i_ServiceComponentStatusId.Value,                                 
        //                         d_StartDate = A.d_StartDate.Value,
        //                         d_EndDate = A.d_EndDate.Value,
        //                         i_QueueStatusId = A.i_QueueStatusId.Value,
        //                        v_ServiceComponentId = A.v_ServiceComponentId,
        //                         d_ApprovedInsertDate = A.d_ApprovedInsertDate,
        //                         d_ApprovedUpdateDate = A.d_ApprovedUpdateDate,
        //                         d_CalledDate = A.d_CalledDate,                              
        //                         d_InsertDateMedicalAnalyst = A.d_InsertDateMedicalAnalyst,
        //                         d_InsertDateTechnicalDataRegister = A.d_InsertDateTechnicalDataRegister,                               
        //                         d_UpdateDateMedicalAnalyst = A.d_UpdateDateMedicalAnalyst,
        //                         d_UpdateDateTechnicalDataRegister = A.d_UpdateDateTechnicalDataRegister,
        //                         i_ApprovedInsertUserId = A.i_ApprovedInsertUserId,
        //                         i_ApprovedUpdateUserId = A.i_ApprovedUpdateUserId,
        //                         i_ExternalInternalId = A.i_ExternalInternalId,
        //                         i_index = A.i_index,                              
        //                         i_InsertUserMedicalAnalystId = A.i_InsertUserMedicalAnalystId,
        //                         i_InsertUserTechnicalDataRegisterId = A.i_InsertUserTechnicalDataRegisterId,
        //                         i_IsApprovedId = A.i_IsApprovedId,
        //                         i_Iscalling = A.i_Iscalling,
        //                         i_Iscalling_1 = A.i_Iscalling_1,
        //                         i_IsInheritedId = A.i_IsInheritedId,
        //                         i_IsInvoicedId = A.i_IsInvoicedId,
        //                         i_IsManuallyAddedId = A.i_IsManuallyAddedId,
        //                         i_IsRequiredId = A.i_IsRequiredId,
        //                         i_IsVisibleId = A.i_IsVisibleId,
        //                         i_ServiceComponentTypeId = A.i_ServiceComponentTypeId,
        //                         i_UpdateUserMedicalAnalystId = A.i_UpdateUserMedicalAnalystId,
        //                         i_UpdateUserTechnicalDataRegisterId = A.i_UpdateUserTechnicalDataRegisterId,
        //                         r_Price = A.r_Price,
        //                         v_Comment = A.v_Comment,
        //                         v_NameOfice = A.v_NameOfice,
        //                         v_ServiceId = A.v_ServiceId
        //                     });

        //        List<ServiceComponentList> obj = query.ToList();

        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public List<ServiceComponentFieldsList> GetServiceComponentFields_Y_Values_OLD(string pstrServiceComponentId_OLD, string pstrServiceComponentId_NEW, List<string> ClientSession)
        //{
        //    int isDeleted = (int)SiNo.NO;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

        //        var query = (from A in dbContext.servicecomponentfields_old
        //                     join B in dbContext.servicecomponentfieldvalues_old on A.v_ServiceComponentFieldsId equals B.v_ServiceComponentFieldsId
        //                     where A.v_ServiceComponentId == pstrServiceComponentId_OLD &&
        //                           A.i_IsDeleted == isDeleted

        //                     select new ServiceComponentFieldsList
        //                     {
        //                         v_ServiceComponentFieldsId = A.v_ServiceComponentFieldsId,
        //                         v_ServiceComponentId = A.v_ServiceComponentId,
        //                         v_ComponentId = A.v_ComponentId,
        //                         v_ComponentFieldId = A.v_ComponentFieldId,
        //                         v_ServiceComponentFieldValuesId = B.v_ServiceComponentFieldValuesId,
        //                         v_ComponentFieldValuesId = B.v_ComponentFieldValuesId,
        //                         v_Value1 = B.v_Value1,

        //                     }).ToList();

        //        List<ServiceComponentFieldsList> obj = query.ToList();
        //        int intNodeId = int.Parse(ClientSession[0]);
        //        string NewIdSCF = "";
        //        foreach (var item in query)
        //        {

        //            servicecomponentfields objEntity = new servicecomponentfields();

        //            objEntity.v_ComponentFieldId = item.v_ComponentFieldsId;
        //            objEntity.v_ServiceComponentId = pstrServiceComponentId_NEW;
        //            objEntity.d_InsertDate = DateTime.Now;
        //            objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
        //            objEntity.i_IsDeleted = 0;

        //            // Autogeneramos el Pk de la tabla               
        //            NewIdSCF = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 35), "CF");
        //            objEntity.v_ServiceComponentFieldsId = NewIdSCF;

        //            dbContext.AddToservicecomponentfields(objEntity);
        //            //dbContext.SaveChanges();

        //            servicecomponentfieldvalues objEntity1 = new servicecomponentfieldvalues();

        //            objEntity1.v_ComponentFieldValuesId = null;//item.v_ComponentFieldValuesId;
        //            objEntity1.v_Value1 = item.v_Value1;
        //            objEntity1.d_InsertDate = DateTime.Now;
        //            objEntity1.i_InsertUserId = Int32.Parse(ClientSession[2]);
        //            objEntity1.i_IsDeleted = 0;

        //            // Autogeneramos el Pk de la tabla               
        //            var NewId1 = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 36), "CV");
        //            objEntity1.v_ServiceComponentFieldValuesId = NewId1;
        //            objEntity1.v_ServiceComponentFieldsId = NewIdSCF;

        //            dbContext.AddToservicecomponentfieldvalues(objEntity1);
        //            //dbContext.SaveChanges();
        //        }

        //        dbContext.SaveChanges();

        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        //public string AddServiceComponent(ref OperationResult pobjOperationResult, servicecomponentDto pobjDtoEntity, List<string> ClientSession)
        //{
        //    //mon.IsActive = true;
        //    string NewId = "(No generado)";
        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //        servicecomponent objEntity = servicecomponentAssembler.ToEntity(pobjDtoEntity);

        //        objEntity.d_InsertDate = DateTime.Now;
        //        objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
        //        objEntity.i_IsDeleted = 0;
        //        // Autogeneramos el Pk de la tabla
        //        int intNodeId = int.Parse(ClientSession[0]);
        //        NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 24), "SC");
        //        objEntity.v_ServiceComponentId = NewId;

        //        dbContext.AddToservicecomponent(objEntity);
        //        dbContext.SaveChanges();

        //        pobjOperationResult.Success = 1;
        //        // Llenar entidad Log
        //        return NewId;
        //    }
        //    catch (Exception ex)
        //    {
        //        pobjOperationResult.Success = 0;
        //        pobjOperationResult.ExceptionMessage = Common.Utils.ExceptionFormatter(ex);
        //        // Llenar entidad Log
        //        return null;
        //    }
        //}

        ////public string  AddServiceComponentField(List<servicecomponentfieldsDto> lpobjDtoEntity, List<string> ClientSession)
        ////{
        ////    //mon.IsActive = true;
        ////    string NewId = "(No generado)";
        ////    try
        ////    {
        ////        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        ////        servicecomponentfields objEntity = servicecomponentfieldsAssembler.ToEntity(pobjDtoEntity);

        ////        objEntity.d_InsertDate = DateTime.Now;
        ////        objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
        ////        objEntity.i_IsDeleted = 0;
        ////        // Autogeneramos el Pk de la tabla
        ////        int intNodeId = int.Parse(ClientSession[0]);
        ////        NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 35), "CF");
        ////        objEntity.v_ServiceComponentFieldsId = NewId;

        ////        dbContext.AddToservicecomponentfields(objEntity);
        ////        dbContext.SaveChanges();
        ////        return NewId;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////         // Llenar entidad Log
        ////           return null;
        ////    }
        ////}

        //public List<ServiceComponentFieldValuesList> GetServiceComponentFieldsValues_OLD(string pstrServiceComponentFieldsId_OLD)
        //{
        //    int isDeleted = (int)SiNo.NO;

        //    try
        //    {
        //        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

        //        var query = (from A in dbContext.servicecomponentfieldvalues_old
        //                     where A.v_ServiceComponentFieldsId == pstrServiceComponentFieldsId_OLD &&
        //                           A.i_IsDeleted == isDeleted

        //                     select new ServiceComponentFieldValuesList
        //                     {
        //                         v_ComponentFieldValuesId = A.v_ComponentFieldValuesId,
        //                         v_ServiceComponentFieldsId = A.v_ServiceComponentFieldsId,
        //                         v_Value1 = A.v_Value1,
        //                         v_Value2 = A.v_Value2,
        //                         i_Index = A.i_Index,
        //                         i_Value1 = A.i_Value1
        //                     });

        //        List<ServiceComponentFieldValuesList> obj = query.ToList();

        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        ////public string AddServiceComponentFieldValues(List<servicecomponentfieldvaluesDto> ListapobjDtoEntity, List<string> ClientSession)
        ////{
        ////    //mon.IsActive = true;
        ////    string NewId = "(No generado)";
        ////    try
        ////    {
        ////        SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        ////        foreach (var fv in ListapobjDtoEntity)
        ////        {                    
        ////            servicecomponentfieldvalues objEntity1 = new servicecomponentfieldvalues();

        ////            objEntity1.v_ComponentFieldValuesId = fv.v_ComponentFieldValuesId;
        ////            objEntity1.v_Value1 = fv.v_Value1;
        ////            objEntity1.d_InsertDate = DateTime.Now;
        ////            objEntity1.i_InsertUserId = Int32.Parse(ClientSession[2]);
        ////            objEntity1.i_IsDeleted = 0;
        ////            int intNodeId = int.Parse(ClientSession[0]);
        ////            // Autogeneramos el Pk de la tabla               
        ////            var NewId1 = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 36), "CV");
        ////            objEntity1.v_ServiceComponentFieldValuesId = NewId1;
        ////            objEntity1.v_ServiceComponentFieldsId = NewId;

        ////            dbContext.AddToservicecomponentfieldvalues(objEntity1);
        ////        }

        ////        dbContext.SaveChanges();


        ////        //SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        ////        //servicecomponentfieldvalues objEntity = servicecomponentfieldvaluesAssembler.ToEntity(pobjDtoEntity);

        ////        //objEntity.d_InsertDate = DateTime.Now;
        ////        //objEntity.i_InsertUserId = Int32.Parse(ClientSession[2]);
        ////        //objEntity.i_IsDeleted = 0;
        ////        //// Autogeneramos el Pk de la tabla
        ////        //int intNodeId = int.Parse(ClientSession[0]);

        ////        //NewId = Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 36), "CV");
        ////        //objEntity.v_ServiceComponentFieldValuesId = NewId;

        ////        //dbContext.AddToservicecomponentfieldvalues(objEntity);
        ////        //dbContext.SaveChanges();
        ////        //return NewId;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        // Llenar entidad Log
        ////          return null;
        ////    }
        ////}


        //#endregion

        //public List< DiagnosticRepositoryList> DevolverListaDiagnosticOLD(string pstrServiceOLD)
        //{
        //    SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
        //    var query = (from A in dbContext.diagnosticrepository_old
        //                 join B in dbContext.diseases on A.v_DiseasesId equals B.v_DiseasesId
        //                 where A.i_IsDeleted == 0 && A.v_ServiceId == pstrServiceOLD
        //                 select new DiagnosticRepositoryList
        //                 {

        //                     v_ServiceId = A.v_ServiceId,
        //                     v_DiseasesId = A.v_DiseasesId,

        //                     v_ComponentId = A.v_ComponentId,
        //                     v_ComponentFieldId = A.v_ComponentFieldId,
        //                     i_AutoManualId = A.i_AutoManualId.Value,
        //                     i_PreQualificationId = A.i_PreQualificationId.Value,
        //                     i_FinalQualificationId = A.i_FinalQualificationId.Value,

        //                     i_DiagnosticTypeId = A.i_DiagnosticTypeId.Value,
        //                     i_IsSentToAntecedent = A.i_IsSentToAntecedent.Value,
        //                     d_ExpirationDateDiagnostic = A.d_ExpirationDateDiagnostic,
        //                     i_GenerateMedicalBreak = A.i_GenerateMedicalBreak.Value,
        //                     v_Recomendations = A.v_Recomendations,

        //                     i_DiagnosticSourceId = A.i_DiagnosticSourceId.Value,
        //                     i_ShapeAccidentId = A.i_ShapeAccidentId.Value,
        //                     i_BodyPartId = A.i_BodyPartId.Value,
        //                     i_ClassificationOfWorkAccidentId = A.i_ClassificationOfWorkAccidentId.Value,
        //                     i_RiskFactorId = A.i_RiskFactorId.Value,

        //                     i_ClassificationOfWorkdiseaseId = A.i_ClassificationOfWorkdiseaseId.Value ,
        //                     i_SendToInterconsultationId = A.i_SendToInterconsultationId.Value,
        //                     i_InterconsultationDestinationId = A.i_InterconsultationDestinationId.Value,
        //                     v_InterconsultationDestinationId = A.v_InterconsultationDestinationId,

        //                     v_Cie10 = B.v_CIE10Id,
        //                        v_Name = B.v_Name

        //                 }).ToList();


        //    return query;
        //}
    }
}
