//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:27
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using Sigesoft.Node.WinClient.DAL;

namespace Sigesoft.Node.WinClient.BE
{

    /// <summary>
    /// Assembler for <see cref="usp_getregisterworkesResult"/> and <see cref="usp_getregisterworkesResultDto"/>.
    /// </summary>
    public static partial class usp_getregisterworkesResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="usp_getregisterworkesResultDto"/> converted from <see cref="usp_getregisterworkesResult"/>.</param>
        static partial void OnDTO(this usp_getregisterworkesResult entity, usp_getregisterworkesResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="usp_getregisterworkesResult"/> converted from <see cref="usp_getregisterworkesResultDto"/>.</param>
        static partial void OnEntity(this usp_getregisterworkesResultDto dto, usp_getregisterworkesResult entity);

        /// <summary>
        /// Converts this instance of <see cref="usp_getregisterworkesResultDto"/> to an instance of <see cref="usp_getregisterworkesResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="usp_getregisterworkesResultDto"/> to convert.</param>
        public static usp_getregisterworkesResult ToEntity(this usp_getregisterworkesResultDto dto)
        {
            if (dto == null) return null;

            var entity = new usp_getregisterworkesResult();

            entity.d_ServiceDate = dto.d_ServiceDate;
            entity.WorkerName = dto.WorkerName;
            entity.ProtocolName = dto.ProtocolName;
            entity.OrganizationName = dto.OrganizationName;
            entity.CurrentOccupation = dto.CurrentOccupation;
            entity.ServiceId = dto.ServiceId;
            entity.PersonId = dto.PersonId;
            entity.ComponentId = dto.ComponentId;
            entity.IIndex = dto.IIndex;
            entity.OrganizationId = dto.OrganizationId;
            entity.TelephoneNumber = dto.TelephoneNumber;
            entity.ServiceComponentId = dto.ServiceComponentId;
            entity.EncuestaCulminada = dto.EncuestaCulminada;
            entity.LaboratorioCulminada = dto.LaboratorioCulminada;
            entity.ClinicaExternad = dto.ClinicaExternad;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="usp_getregisterworkesResult"/> to an instance of <see cref="usp_getregisterworkesResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="usp_getregisterworkesResult"/> to convert.</param>
        public static usp_getregisterworkesResultDto ToDTO(this usp_getregisterworkesResult entity)
        {
            if (entity == null) return null;

            var dto = new usp_getregisterworkesResultDto();

            dto.d_ServiceDate = entity.d_ServiceDate;
            dto.WorkerName = entity.WorkerName;
            dto.ProtocolName = entity.ProtocolName;
            dto.OrganizationName = entity.OrganizationName;
            dto.CurrentOccupation = entity.CurrentOccupation;
            dto.ServiceId = entity.ServiceId;
            dto.PersonId = entity.PersonId;
            dto.ComponentId = entity.ComponentId;
            dto.IIndex = entity.IIndex;
            dto.OrganizationId = entity.OrganizationId;
            dto.TelephoneNumber = entity.TelephoneNumber;
            dto.ServiceComponentId = entity.ServiceComponentId;
            dto.EncuestaCulminada = entity.EncuestaCulminada;
            dto.LaboratorioCulminada = entity.LaboratorioCulminada;
            dto.ClinicaExternad = entity.ClinicaExternad;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="usp_getregisterworkesResultDto"/> to an instance of <see cref="usp_getregisterworkesResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<usp_getregisterworkesResult> ToEntities(this IEnumerable<usp_getregisterworkesResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="usp_getregisterworkesResult"/> to an instance of <see cref="usp_getregisterworkesResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<usp_getregisterworkesResultDto> ToDTOs(this IEnumerable<usp_getregisterworkesResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
