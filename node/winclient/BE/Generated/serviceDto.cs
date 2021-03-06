//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:03
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    [DataContract()]
    public partial class serviceDto
    {
        [DataMember()]
        public String v_ServiceId { get; set; }

        [DataMember()]
        public String v_ProtocolId { get; set; }

        [DataMember()]
        public String v_PersonId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_MasterServiceId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_ServiceStatusId { get; set; }

        [DataMember()]
        public String v_Motive { get; set; }

        [DataMember()]
        public Nullable<Int32> i_AptitudeStatusId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_ServiceDate { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_GlobalExpirationDate { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_ObsExpirationDate { get; set; }

        [DataMember()]
        public Nullable<Int32> i_FlagAgentId { get; set; }

        [DataMember()]
        public String v_OrganizationId { get; set; }

        [DataMember()]
        public String v_LocationId { get; set; }

        [DataMember()]
        public String v_MainSymptom { get; set; }

        [DataMember()]
        public Nullable<Int32> i_TimeOfDisease { get; set; }

        [DataMember()]
        public Nullable<Int32> i_TimeOfDiseaseTypeId { get; set; }

        [DataMember()]
        public String v_Story { get; set; }

        [DataMember()]
        public Nullable<Int32> i_DreamId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_UrineId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_DepositionId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_AppetiteId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_ThirstId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_Fur { get; set; }

        [DataMember()]
        public String v_CatemenialRegime { get; set; }

        [DataMember()]
        public Nullable<Int32> i_MacId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_IsNewControl { get; set; }

        [DataMember()]
        public Nullable<Int32> i_HasMedicalBreakId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_MedicalBreakStartDate { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_MedicalBreakEndDate { get; set; }

        [DataMember()]
        public String v_GeneralRecomendations { get; set; }

        [DataMember()]
        public Nullable<Int32> i_DestinationMedicationId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_TransportMedicationId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_StartDateRestriction { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_EndDateRestriction { get; set; }

        [DataMember()]
        public Nullable<Int32> i_HasRestrictionId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_HasSymptomId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_UpdateDate { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_NextAppointment { get; set; }

        [DataMember()]
        public Nullable<Int32> i_IsDeleted { get; set; }

        [DataMember()]
        public Nullable<Int32> i_InsertUserId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_InsertDate { get; set; }

        [DataMember()]
        public Nullable<Int32> i_UpdateUserId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_SendToTracking { get; set; }

        [DataMember()]
        public Nullable<Int32> i_InsertUserMedicalAnalystId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_UpdateUserMedicalAnalystId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_InsertDateMedicalAnalyst { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_UpdateDateMedicalAnalyst { get; set; }

        [DataMember()]
        public Nullable<Int32> i_InsertUserOccupationalMedicalId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_UpdateUserOccupationalMedicaltId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_InsertDateOccupationalMedical { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_UpdateDateOccupationalMedical { get; set; }

        [DataMember()]
        public Nullable<Int32> i_HazInterconsultationId { get; set; }

        [DataMember()]
        public String v_Gestapara { get; set; }

        [DataMember()]
        public String v_Menarquia { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_PAP { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_Mamografia { get; set; }

        [DataMember()]
        public String v_CiruGine { get; set; }

        [DataMember()]
        public String v_Findings { get; set; }

        [DataMember()]
        public Int32 i_StatusLiquidation { get; set; }

        [DataMember()]
        public Nullable<Int32> i_ServiceTypeOfInsurance { get; set; }

        [DataMember()]
        public Nullable<Int32> i_ModalityOfInsurance { get; set; }

        [DataMember()]
        public Nullable<Int32> i_MaritalStatusId { get; set; }

        [DataMember()]
        public String v_Mail { get; set; }

        [DataMember()]
        public Nullable<Int32> i_LevelOfId { get; set; }

        [DataMember()]
        public String v_TelephoneNumber { get; set; }

        [DataMember()]
        public Nullable<Int32> i_TypeOfInsuranceId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_ResidenceInWorkplaceId { get; set; }

        [DataMember()]
        public String v_ResidenceTimeInWorkplace { get; set; }

        [DataMember()]
        public Nullable<Int32> i_NumberLivingChildren { get; set; }

        [DataMember()]
        public String v_AdressLocation { get; set; }

        [DataMember()]
        public Nullable<Int32> i_NumberDeadChildren { get; set; }

        [DataMember()]
        public String v_CurrentOccupation { get; set; }

        [DataMember()]
        public Nullable<Int32> i_DepartmentId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_ProvinceId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_DistrictId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_Relationship { get; set; }

        [DataMember()]
        public String v_OwnerName { get; set; }

        [DataMember()]
        public String v_ExploitedMineral { get; set; }

        [DataMember()]
        public Nullable<Int32> i_AltitudeWorkId { get; set; }

        [DataMember()]
        public Nullable<Int32> i_PlaceWorkId { get; set; }

        [DataMember()]
        public String v_EmpresaFacturacionId { get; set; }

        [DataMember()]
        public String v_Sede { get; set; }

        [DataMember()]
        public String GeneracionReporte { get; set; }

        [DataMember()]
        public Nullable<Int32> TipoEmpresaCovidId { get; set; }

        [DataMember()]
        public String EmpresaPrincipal { get; set; }

        [DataMember()]
        public String EmpresaEmpleadora { get; set; }

        [DataMember()]
        public String TecnicoCovid { get; set; }

        [DataMember()]
        public Nullable<Int32> ClinicaExternad { get; set; }

        [DataMember()]
        public Nullable<Int32> CorreoEnviado { get; set; }

        [DataMember()]
        public Nullable<Int32> EncuestaCulminada { get; set; }

        [DataMember()]
        public Nullable<Int32> LaboratorioCulminada { get; set; }

        [DataMember()]
        public Nullable<Int32> ReasonExamId { get; set; }

        [DataMember()]
        public Nullable<Int32> PlaceExamId { get; set; }

        [DataMember()]
        public List<auxiliaryexamDto> auxiliaryexam { get; set; }

        [DataMember()]
        public List<calendarDto> calendar { get; set; }

        [DataMember()]
        public List<diagnosticrepositoryDto> diagnosticrepository { get; set; }

        [DataMember()]
        public List<medicationDto> medication { get; set; }

        [DataMember()]
        public List<procedurebyserviceDto> procedurebyservice { get; set; }

        [DataMember()]
        public List<recommendationDto> recommendation { get; set; }

        [DataMember()]
        public List<restrictionDto> restriction { get; set; }

        [DataMember()]
        public personDto person { get; set; }

        [DataMember()]
        public protocolDto protocol { get; set; }

        [DataMember()]
        public List<servicecomponentDto> servicecomponent { get; set; }

        [DataMember()]
        public List<servicemultimediaDto> servicemultimedia { get; set; }

        public serviceDto()
        {
        }

        public serviceDto(String v_ServiceId, String v_ProtocolId, String v_PersonId, Nullable<Int32> i_MasterServiceId, Nullable<Int32> i_ServiceStatusId, String v_Motive, Nullable<Int32> i_AptitudeStatusId, Nullable<DateTime> d_ServiceDate, Nullable<DateTime> d_GlobalExpirationDate, Nullable<DateTime> d_ObsExpirationDate, Nullable<Int32> i_FlagAgentId, String v_OrganizationId, String v_LocationId, String v_MainSymptom, Nullable<Int32> i_TimeOfDisease, Nullable<Int32> i_TimeOfDiseaseTypeId, String v_Story, Nullable<Int32> i_DreamId, Nullable<Int32> i_UrineId, Nullable<Int32> i_DepositionId, Nullable<Int32> i_AppetiteId, Nullable<Int32> i_ThirstId, Nullable<DateTime> d_Fur, String v_CatemenialRegime, Nullable<Int32> i_MacId, Nullable<Int32> i_IsNewControl, Nullable<Int32> i_HasMedicalBreakId, Nullable<DateTime> d_MedicalBreakStartDate, Nullable<DateTime> d_MedicalBreakEndDate, String v_GeneralRecomendations, Nullable<Int32> i_DestinationMedicationId, Nullable<Int32> i_TransportMedicationId, Nullable<DateTime> d_StartDateRestriction, Nullable<DateTime> d_EndDateRestriction, Nullable<Int32> i_HasRestrictionId, Nullable<Int32> i_HasSymptomId, Nullable<DateTime> d_UpdateDate, Nullable<DateTime> d_NextAppointment, Nullable<Int32> i_IsDeleted, Nullable<Int32> i_InsertUserId, Nullable<DateTime> d_InsertDate, Nullable<Int32> i_UpdateUserId, Nullable<Int32> i_SendToTracking, Nullable<Int32> i_InsertUserMedicalAnalystId, Nullable<Int32> i_UpdateUserMedicalAnalystId, Nullable<DateTime> d_InsertDateMedicalAnalyst, Nullable<DateTime> d_UpdateDateMedicalAnalyst, Nullable<Int32> i_InsertUserOccupationalMedicalId, Nullable<Int32> i_UpdateUserOccupationalMedicaltId, Nullable<DateTime> d_InsertDateOccupationalMedical, Nullable<DateTime> d_UpdateDateOccupationalMedical, Nullable<Int32> i_HazInterconsultationId, String v_Gestapara, String v_Menarquia, Nullable<DateTime> d_PAP, Nullable<DateTime> d_Mamografia, String v_CiruGine, String v_Findings, Int32 i_StatusLiquidation, Nullable<Int32> i_ServiceTypeOfInsurance, Nullable<Int32> i_ModalityOfInsurance, Nullable<Int32> i_MaritalStatusId, String v_Mail, Nullable<Int32> i_LevelOfId, String v_TelephoneNumber, Nullable<Int32> i_TypeOfInsuranceId, Nullable<Int32> i_ResidenceInWorkplaceId, String v_ResidenceTimeInWorkplace, Nullable<Int32> i_NumberLivingChildren, String v_AdressLocation, Nullable<Int32> i_NumberDeadChildren, String v_CurrentOccupation, Nullable<Int32> i_DepartmentId, Nullable<Int32> i_ProvinceId, Nullable<Int32> i_DistrictId, Nullable<Int32> i_Relationship, String v_OwnerName, String v_ExploitedMineral, Nullable<Int32> i_AltitudeWorkId, Nullable<Int32> i_PlaceWorkId, String v_EmpresaFacturacionId, String v_Sede, String generacionReporte, Nullable<Int32> tipoEmpresaCovidId, String empresaPrincipal, String empresaEmpleadora, String tecnicoCovid, Nullable<Int32> clinicaExternad, Nullable<Int32> correoEnviado, Nullable<Int32> encuestaCulminada, Nullable<Int32> laboratorioCulminada, Nullable<Int32> reasonExamId, Nullable<Int32> placeExamId, List<auxiliaryexamDto> auxiliaryexam, List<calendarDto> calendar, List<diagnosticrepositoryDto> diagnosticrepository, List<medicationDto> medication, List<procedurebyserviceDto> procedurebyservice, List<recommendationDto> recommendation, List<restrictionDto> restriction, personDto person, protocolDto protocol, List<servicecomponentDto> servicecomponent, List<servicemultimediaDto> servicemultimedia)
        {
			this.v_ServiceId = v_ServiceId;
			this.v_ProtocolId = v_ProtocolId;
			this.v_PersonId = v_PersonId;
			this.i_MasterServiceId = i_MasterServiceId;
			this.i_ServiceStatusId = i_ServiceStatusId;
			this.v_Motive = v_Motive;
			this.i_AptitudeStatusId = i_AptitudeStatusId;
			this.d_ServiceDate = d_ServiceDate;
			this.d_GlobalExpirationDate = d_GlobalExpirationDate;
			this.d_ObsExpirationDate = d_ObsExpirationDate;
			this.i_FlagAgentId = i_FlagAgentId;
			this.v_OrganizationId = v_OrganizationId;
			this.v_LocationId = v_LocationId;
			this.v_MainSymptom = v_MainSymptom;
			this.i_TimeOfDisease = i_TimeOfDisease;
			this.i_TimeOfDiseaseTypeId = i_TimeOfDiseaseTypeId;
			this.v_Story = v_Story;
			this.i_DreamId = i_DreamId;
			this.i_UrineId = i_UrineId;
			this.i_DepositionId = i_DepositionId;
			this.i_AppetiteId = i_AppetiteId;
			this.i_ThirstId = i_ThirstId;
			this.d_Fur = d_Fur;
			this.v_CatemenialRegime = v_CatemenialRegime;
			this.i_MacId = i_MacId;
			this.i_IsNewControl = i_IsNewControl;
			this.i_HasMedicalBreakId = i_HasMedicalBreakId;
			this.d_MedicalBreakStartDate = d_MedicalBreakStartDate;
			this.d_MedicalBreakEndDate = d_MedicalBreakEndDate;
			this.v_GeneralRecomendations = v_GeneralRecomendations;
			this.i_DestinationMedicationId = i_DestinationMedicationId;
			this.i_TransportMedicationId = i_TransportMedicationId;
			this.d_StartDateRestriction = d_StartDateRestriction;
			this.d_EndDateRestriction = d_EndDateRestriction;
			this.i_HasRestrictionId = i_HasRestrictionId;
			this.i_HasSymptomId = i_HasSymptomId;
			this.d_UpdateDate = d_UpdateDate;
			this.d_NextAppointment = d_NextAppointment;
			this.i_IsDeleted = i_IsDeleted;
			this.i_InsertUserId = i_InsertUserId;
			this.d_InsertDate = d_InsertDate;
			this.i_UpdateUserId = i_UpdateUserId;
			this.i_SendToTracking = i_SendToTracking;
			this.i_InsertUserMedicalAnalystId = i_InsertUserMedicalAnalystId;
			this.i_UpdateUserMedicalAnalystId = i_UpdateUserMedicalAnalystId;
			this.d_InsertDateMedicalAnalyst = d_InsertDateMedicalAnalyst;
			this.d_UpdateDateMedicalAnalyst = d_UpdateDateMedicalAnalyst;
			this.i_InsertUserOccupationalMedicalId = i_InsertUserOccupationalMedicalId;
			this.i_UpdateUserOccupationalMedicaltId = i_UpdateUserOccupationalMedicaltId;
			this.d_InsertDateOccupationalMedical = d_InsertDateOccupationalMedical;
			this.d_UpdateDateOccupationalMedical = d_UpdateDateOccupationalMedical;
			this.i_HazInterconsultationId = i_HazInterconsultationId;
			this.v_Gestapara = v_Gestapara;
			this.v_Menarquia = v_Menarquia;
			this.d_PAP = d_PAP;
			this.d_Mamografia = d_Mamografia;
			this.v_CiruGine = v_CiruGine;
			this.v_Findings = v_Findings;
			this.i_StatusLiquidation = i_StatusLiquidation;
			this.i_ServiceTypeOfInsurance = i_ServiceTypeOfInsurance;
			this.i_ModalityOfInsurance = i_ModalityOfInsurance;
			this.i_MaritalStatusId = i_MaritalStatusId;
			this.v_Mail = v_Mail;
			this.i_LevelOfId = i_LevelOfId;
			this.v_TelephoneNumber = v_TelephoneNumber;
			this.i_TypeOfInsuranceId = i_TypeOfInsuranceId;
			this.i_ResidenceInWorkplaceId = i_ResidenceInWorkplaceId;
			this.v_ResidenceTimeInWorkplace = v_ResidenceTimeInWorkplace;
			this.i_NumberLivingChildren = i_NumberLivingChildren;
			this.v_AdressLocation = v_AdressLocation;
			this.i_NumberDeadChildren = i_NumberDeadChildren;
			this.v_CurrentOccupation = v_CurrentOccupation;
			this.i_DepartmentId = i_DepartmentId;
			this.i_ProvinceId = i_ProvinceId;
			this.i_DistrictId = i_DistrictId;
			this.i_Relationship = i_Relationship;
			this.v_OwnerName = v_OwnerName;
			this.v_ExploitedMineral = v_ExploitedMineral;
			this.i_AltitudeWorkId = i_AltitudeWorkId;
			this.i_PlaceWorkId = i_PlaceWorkId;
			this.v_EmpresaFacturacionId = v_EmpresaFacturacionId;
			this.v_Sede = v_Sede;
			this.GeneracionReporte = generacionReporte;
			this.TipoEmpresaCovidId = tipoEmpresaCovidId;
			this.EmpresaPrincipal = empresaPrincipal;
			this.EmpresaEmpleadora = empresaEmpleadora;
			this.TecnicoCovid = tecnicoCovid;
			this.ClinicaExternad = clinicaExternad;
			this.CorreoEnviado = correoEnviado;
			this.EncuestaCulminada = encuestaCulminada;
			this.LaboratorioCulminada = laboratorioCulminada;
			this.ReasonExamId = reasonExamId;
			this.PlaceExamId = placeExamId;
			this.auxiliaryexam = auxiliaryexam;
			this.calendar = calendar;
			this.diagnosticrepository = diagnosticrepository;
			this.medication = medication;
			this.procedurebyservice = procedurebyservice;
			this.recommendation = recommendation;
			this.restriction = restriction;
			this.person = person;
			this.protocol = protocol;
			this.servicecomponent = servicecomponent;
			this.servicemultimedia = servicemultimedia;
        }
    }
}
