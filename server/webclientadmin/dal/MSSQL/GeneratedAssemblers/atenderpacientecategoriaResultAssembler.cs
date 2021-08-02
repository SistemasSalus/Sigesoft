//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:16:01
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using Sigesoft.Server.WebClientAdmin.DAL;

namespace Sigesoft.Server.WebClientAdmin.BE
{

    /// <summary>
    /// Assembler for <see cref="atenderpacientecategoriaResult"/> and <see cref="atenderpacientecategoriaResultDto"/>.
    /// </summary>
    public static partial class atenderpacientecategoriaResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="atenderpacientecategoriaResultDto"/> converted from <see cref="atenderpacientecategoriaResult"/>.</param>
        static partial void OnDTO(this atenderpacientecategoriaResult entity, atenderpacientecategoriaResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="atenderpacientecategoriaResult"/> converted from <see cref="atenderpacientecategoriaResultDto"/>.</param>
        static partial void OnEntity(this atenderpacientecategoriaResultDto dto, atenderpacientecategoriaResult entity);

        /// <summary>
        /// Converts this instance of <see cref="atenderpacientecategoriaResultDto"/> to an instance of <see cref="atenderpacientecategoriaResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="atenderpacientecategoriaResultDto"/> to convert.</param>
        public static atenderpacientecategoriaResult ToEntity(this atenderpacientecategoriaResultDto dto)
        {
            if (dto == null) return null;

            var entity = new atenderpacientecategoriaResult();

            entity.d_CircuitStartDate = dto.d_CircuitStartDate;
            entity.Paciente = dto.Paciente;
            entity.Empleador = dto.Empleador;
            entity.Cliente = dto.Cliente;
            entity.Protocolo = dto.Protocolo;
            entity.v_CurrentOccupation = dto.v_CurrentOccupation;
            entity.v_Value1 = dto.v_Value1;
            entity.i_IsVipId = dto.i_IsVipId;
            entity.v_ServiceId = dto.v_ServiceId;
            entity.v_PersonId = dto.v_PersonId;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="atenderpacientecategoriaResult"/> to an instance of <see cref="atenderpacientecategoriaResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="atenderpacientecategoriaResult"/> to convert.</param>
        public static atenderpacientecategoriaResultDto ToDTO(this atenderpacientecategoriaResult entity)
        {
            if (entity == null) return null;

            var dto = new atenderpacientecategoriaResultDto();

            dto.d_CircuitStartDate = entity.d_CircuitStartDate;
            dto.Paciente = entity.Paciente;
            dto.Empleador = entity.Empleador;
            dto.Cliente = entity.Cliente;
            dto.Protocolo = entity.Protocolo;
            dto.v_CurrentOccupation = entity.v_CurrentOccupation;
            dto.v_Value1 = entity.v_Value1;
            dto.i_IsVipId = entity.i_IsVipId;
            dto.v_ServiceId = entity.v_ServiceId;
            dto.v_PersonId = entity.v_PersonId;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="atenderpacientecategoriaResultDto"/> to an instance of <see cref="atenderpacientecategoriaResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<atenderpacientecategoriaResult> ToEntities(this IEnumerable<atenderpacientecategoriaResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="atenderpacientecategoriaResult"/> to an instance of <see cref="atenderpacientecategoriaResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<atenderpacientecategoriaResultDto> ToDTOs(this IEnumerable<atenderpacientecategoriaResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
